using Isban.Maps.Entity;
using Isban.Maps.Entity.Base;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.Security.Adsec.Entity;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Isban.Maps.Bussiness
{
    [ExcludeFromCodeCoverage]
    public class ChequeoBusiness : IChequeoBusiness
    {
        private static X509Certificate2 objCert;
      
        public virtual List<ChequeoAcceso> Chequeo(EntityBase entity)
        {

            List<ChequeoAcceso> lista = new List<ChequeoAcceso>();
            var daSmc = DependencyFactory.Resolve<ISmcDA>();
            var daOpics = DependencyFactory.Resolve<IOpicsDA>();
            var daMaps = DependencyFactory.Resolve<IMapsControlesDA>();

            try
            {
                var item = daOpics.Chequeo(entity);
                ValidData(lista, daOpics.GetInfoDB, daOpics.ConnectionString, item, "OPICS");
            }
            catch (Exception ex)
            {
                CatchConnEx(lista, daOpics.GetInfoDB, daOpics.ConnectionString, "OPICS", ex);
            }
            try
            {
                var item = daMaps.Chequeo(entity);
                ValidData(lista, daMaps.GetInfoDB, daMaps.ConnectionString, item, "MAPS");
            }
            catch (Exception ex)
            {
                CatchConnEx(lista, daMaps.GetInfoDB, daMaps.ConnectionString, "MAPS", ex);
            }
            try
            {
                var item = daSmc.Chequeo(entity);
                ValidData(lista, daSmc.GetInfoDB, daSmc.ConnectionString, item, "SMC");
            }
            catch (Exception ex)
            {
                CatchConnEx(lista, daSmc.GetInfoDB, daSmc.ConnectionString, "SMC", ex);
            }

            return lista;
        }

       

        public virtual DatoFirmaMaps ObtenerFirmaCertificada(RequestSecurity<DatoFirmaMaps> request)
        {
            DatoFirmaMaps response = request.MapperClass<DatoFirmaMaps>(TypeMapper.IgnoreCaseSensitive);
            string[] whiteList = { @"25DEMAYO\WSI051086", @"25DEMAYO\WEBBPSIBEDESA01" };
            var serverActual = Chequeo(new EntityBase { Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault });

            if (whiteList.Contains(serverActual.FirstOrDefault().ServidorWin))
            {
                ObtenerCertificado();

                response.Dato = request.Dato;
                response.DatosFirmado = DatosFirmado.Si;
                response.TipoHash = TipoHash.B64;
                response.Firma = FirmarDatos(request.Dato);
            }
            else
            {
                response.Firma = "La firma se puede obtener solo desde el server de Desarrollo";

            }
            return response;
        }

        private static string FirmarDatos(string dato)
        {
            //Creamos el ContentInfo
            ContentInfo objContent = new ContentInfo(Encoding.ASCII.GetBytes(dato));

            //Creamos el objeto que representa los datos firmados
            SignedCms objSignedData = new SignedCms(objContent);

            //Creamos el "firmante"
            CmsSigner objSigner = new CmsSigner(objCert);

            //Firmamos los datos
            objSignedData.ComputeSignature(objSigner);

            //Obtenemos el resultado
            byte[] bytSigned = objSignedData.Encode();

            return Convert.ToBase64String(bytSigned);
        }

        private static void ValidData(List<ChequeoAcceso> lista, Func<long, string> getInfoDB, string connectionString, ChequeoAcceso item, string userDB)
        {
            item.ConnectionString = connectionString;
            try
            {
                if (!string.IsNullOrWhiteSpace(item.ConnectionString) &&
                item.ConnectionString.ToUpper().Contains("CREDENTIALID"))
                {
                    var id = item.ConnectionString.Substring(item.ConnectionString.ToUpper().LastIndexOf("CREDENTIALID")).Split('=')[1];
                    item.Hash = getInfoDB(Convert.ToInt64(id));
                }
                item.Ok = true;
            }
            catch (Exception ex)
            {
                item = new ChequeoAcceso { UsuarioDB = userDB, BasedeDatos = ex.Message, ServidorDB = ex.Message, ServidorWin = ex.Message, UsuarioWin = ex.Message };
            }
            lista.Add(item);
        }

        private static void CatchConnEx(List<ChequeoAcceso> lista, Func<long, string> getInfoDB, string connectionString, string userDB, Exception ex)
        {
            ChequeoAcceso item = new ChequeoAcceso { UsuarioDB = userDB, BasedeDatos = ex.Message, ServidorDB = ex.Message, ServidorWin = ex.Message, UsuarioWin = ex.Message };
            item.ConnectionString = connectionString;
            try
            {
                if (!string.IsNullOrWhiteSpace(item.ConnectionString) &&
                item.ConnectionString.ToUpper().Contains("CREDENTIALID"))
                {
                    var id = item.ConnectionString.Substring(item.ConnectionString.ToUpper().LastIndexOf("CREDENTIALID")).Split('=')[1];
                    item.Hash = getInfoDB(Convert.ToInt64(id));
                }
                item.Ok = true;
            }
            catch(Exception exc) {
                item = new ChequeoAcceso { UsuarioDB = userDB, BasedeDatos = exc.Message, ServidorDB = exc.Message, ServidorWin = exc.Message, UsuarioWin = exc.Message };
            }
            lista.Add(item);
        }

        private static void ObtenerCertificado()
        {
            string nameCert = string.Empty;

            try
            {     
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                path = Path.GetDirectoryName(path);
                nameCert = path + "\\" + "IATX\\desa.p12";
                //byte[] datosAFirmar = System.Text.Encoding.UTF8.GetBytes(fim);
                objCert = new X509Certificate2(nameCert, "clave853");

            }
            catch (Exception ex)
            {

                throw new Exception(nameCert, ex);
            }
        }
    }
}
