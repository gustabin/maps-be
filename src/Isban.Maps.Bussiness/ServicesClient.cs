
namespace Isban.Maps.Bussiness
{
    using Isban.Maps.Bussiness.WSERIService;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.ServiceModel;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Entity.Base;
    using Isban.Maps.Entity.Controles.Independientes;
    using System;
    using System.Reflection;
    using Entity.Controles;
    using System.Linq;
    using Isban.Maps.Entity.Controles.Customizados;
    using IBussiness;
    using System.Globalization;

    public class ServicesClient : IServicesClient
    {
        public static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public string GetPerfil(string nup, DatoFirmaMaps firma)
        {
            var binding = new BasicHttpBinding()
            {
                Name = "ConsultaPerfilInversor",
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
            };
            PerfilInversor response = null;

            if (ConfigurationManager.AppSettings["ERI_URL"] != null)
            {
                var er = new Uri(ConfigurationManager.AppSettings["ERI_URL"]);

                var add = new EndpointAddress(er);
                var service = new ERIServiceClient(binding, add);

                WSERIService.ConsultaPerfilInversorRequest param = new WSERIService.ConsultaPerfilInversorRequest();

                param.Firma_datos_dentro = "N";
                param.Firma_datos_firmados = firma.Dato;
                param.Firma_formato = firma.TipoHash.Equals(Isban.Mercados.Security.Adsec.Entity.TipoHash.B64)
                    ? "B64"
                    : "PEM";
                param.Firma_hash = firma.Firma;

                param.Datos = new WSERIService.ConsultaPerfilInversor { Fecha = System.DateTime.Now, Nup = nup };

                try
                {
                    response = service.ConsultaPerfilInversor(param);
                }
                catch(Exception ex) // ----------------> Para cuando el servicio no responde.
                {
                    response = new PerfilInversor();
                    response.Descripcion = "Sin Perfil";
                }
            }

            return response != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(response.Descripcion.ToLower()) : string.Empty;

        }

        public  IList<ItemDisclaimer<string>> EvaluacionRiesgo(List<ControlSimple> list, string nup, DatoFirmaMaps firma)
        {
            var binding = new BasicHttpBinding()
            {
                Name = "EvaluacionDeRiesgo",
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
            };

            var er = new Uri(ConfigurationManager.AppSettings["ERI_URL"]);

            var add = new EndpointAddress(er);
            var service = new ERIServiceClient(binding, add);

            WSERIService.EvaluacionDeRiesgoRequest param = new WSERIService.EvaluacionDeRiesgoRequest();

            param.Firma_datos_dentro = "N";
            param.Firma_datos_firmados = firma.Dato;
            param.Firma_formato = firma.TipoHash.Equals(Isban.Mercados.Security.Adsec.Entity.TipoHash.B64)
                ? "B64"
                : "PEM";
            param.Firma_hash = firma.Firma;

            param.Datos = new WSERIService.EvaluacionDeRiesgo();
            param.Datos.CodCanal = ConfigurationManager.AppSettings["CodCanal"];
            param.Datos.Nup = nup;
            param.Datos.CanalId = ConfigurationManager.AppSettings["CanalId"];
            param.Datos.CanalTipo = ConfigurationManager.AppSettings["CanalTipo"];

            var oMoneda = list.Where(x => (x as ControlSimple).Nombre == "moneda").FirstOrDefault();

            IList<ItemDisclaimer<string>> result = new List<ItemDisclaimer<string>>();
            try
            {
                var propInfo = oMoneda.GetType().GetProperty("items", bindFlags);
                List<ItemMoneda<string>> itemsMoneda = propInfo.GetValue(oMoneda, null) as List<ItemMoneda<string>>;
                param.Datos.Moneda = itemsMoneda.Where(x => x.Seleccionado).FirstOrDefault().Valor;
                param.Datos.TipoOperacion = ConfigurationManager.AppSettings["Tipo_Operacion_Evaluacion_SAF"];

                WSERIService.EvaluaRiesgo response = service.EvaluacionDeRiesgo(param);

                XmlSerializer mySerializer = new XmlSerializer(typeof(WSERIService.MappingXML.Mensaje));

                //XmlDocument xDoc = new XmlDocument();
                //xDoc.LoadXml(response.Disclaimer);
                //XmlNodeList elemlist = xDoc.GetElementsByTagName("Mensaje");

                WSERIService.MappingXML.Mensaje oRespuesta = mySerializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(response.Disclaimer))) as WSERIService.MappingXML.Mensaje;
                WSERIService.MappingXML.Mensaje oRespuestaCumplimiento = mySerializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(response.DisclaimerCumplimiento))) as WSERIService.MappingXML.Mensaje;

                foreach (WSERIService.MappingXML.Disclaimer item in oRespuesta.Disclaimers)
                {
                    result.Add(
                    new ItemDisclaimer<string>
                    {
                        Valor = item.Text,
                        Desc = item.Titulo,
                        Tipodisclaimer = response.TipoDisclaimer
                    });
                }

                foreach (WSERIService.MappingXML.Disclaimer item in oRespuestaCumplimiento.Disclaimers)
                {
                    result.Add(
                    new ItemDisclaimer<string>
                    {
                        Valor = item.Text,
                        Desc = item.Titulo,
                        Tipodisclaimer = response.TipoDisclaimer
                    });
                }

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public  string GetMailXNup(string canal, string subcanal, string nup)
        {
            var binding = new BasicHttpBinding()
            {
                Name = "getEstadoCliente",
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
            };
            string email = string.Empty;

            if (ConfigurationManager.AppSettings["MYA_URL"] != null)
            {
                var er = new Uri(ConfigurationManager.AppSettings["MYA_URL"]);

                var add = new EndpointAddress(er);
                var service = new MYAService.ServicesClient(binding, add);
    
                //canal = "22";
                subcanal = subcanal.Substring(2, 2);
                string param = string.Format("<xml><header><Servicio>getEstadoCliente</Servicio><Canal>{0}</Canal><SubCanal>{1}</SubCanal><NUP>{2}</NUP></header><datosFirmados></datosFirmados></xml>", canal, subcanal, nup);

                //En caso que venga por error, maps debe continuar -----> Preguntar
                try
                {
                    string response = service.getEstadoCliente(param);
                    
                    XmlSerializer mySerializer = new XmlSerializer(typeof(MYAService.respuesta));

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(response);
                    XmlNodeList elemlist = xDoc.GetElementsByTagName("soapenv:Body");

                    MYAService.respuesta oRespuesta = mySerializer.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(elemlist[0].InnerXml))) as MYAService.respuesta;
                    
                    if (oRespuesta.CodRet == "0")
                    {
                        foreach (MYAService.Destino destino in oRespuesta.Destinos)
                        {
                            if (destino.DestinoTipo.ToUpper() == "MAIL")
                            {
                                if (email == string.Empty || destino.DestinoSecuencia == "1")
                                    email = destino.DestinoDescripcion;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    email = string.Empty;
                }
            }
            return email;
        }

    }
}
