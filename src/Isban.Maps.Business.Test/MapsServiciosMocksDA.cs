using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Interfaces;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Isban.Maps.Entity;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Controles;

namespace Isban.Maps.Business.Test
{
    internal class MapsServiciosMocksDA : IMapsControlesDA
    {
        public string ConnectionString
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ValorCtrlResponse[] ObtenerDatosPorControl(ControlSimple type, FormularioResponse entity)
        {
            try
            {
                PropertyInfo propInfo = null;

                BindingFlags bf = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
                var tipoControl = type.GetType().GetProperty("tipo", bf).GetValue(type, null);

                IEnumerable<IResponseEntity> result = null;

                switch (tipoControl.ToString())
                {
                    case "fondo":
                        result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Fondos");
                        break;
                    case TipoControl.Lista:
                        switch (type.GetType().GetProperty("nombre", bf).GetValue(type, null).ToString())
                        {
                            case "moneda":
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Moneda");
                                break;
                            case "operacion":
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Operacion");
                                break;

                            case "cuenta-operativa":
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("CuentaOperativa");
                                break;

                            case "cuenta-titulo":
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("CuentaTitulos");
                                break;

                            case "periodos":
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Periodos");
                                break;

                        }
                        break;
                    case TipoControl.Servicio:
                        result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Servicio");
                        break;
                    case TipoControl.InputText:
                    case TipoControl.InputNumber:
                    case TipoControl.Fecha:
                    case TipoControl.Email:
                        propInfo = type.GetType().GetProperty("Nombre", bf);
                        switch (Convert.ToString(propInfo.GetValue(type)).ToLower())
                        {
                            case NombreComponente.Alias:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Alias");
                                break;
                            case NombreComponente.SaldoMinimo:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("SaldoMinimo");
                                break;
                            case NombreComponente.Fecha:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Fecha");
                                break;
                            case NombreComponente.Email:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Email");
                                break;
                            case NombreComponente.FechaDesde:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("FechaDesde");
                                break;
                            case NombreComponente.FechaHasta:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("FechaHasta");
                                break;
                            case NombreComponente.DescripcionDinamica:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("DescripcionDinamica");
                                break;
                            case NombreComponente.EstadoAdhesion:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("EstadoAdhesion");
                                break;
                            case NombreComponente.MontoSuscripcionMaximo:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("MontoSuscripcion");
                                break;
                            case NombreComponente.MontoSuscripcionMinimo:
                                result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("MontoSuscripcion");
                                break;
                        }
                        break;
                    case TipoControl.Legal:
                        result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Legales");
                        break;
                    case TipoControl.FechaCompuesta:
                        result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("Vigencia");

                        break;
                        /*case TipoControl.ImporteCompuesto:
                            propInfo = type.GetType().GetProperty("Nombre");
                            switch (Convert.ToString(propInfo.GetValue(type)).ToLower())
                            {
                                case NombreComponente.MontoSuscripcion:
                                    result = ObtenerValoresDelComponente<ValorCtrlMockResponse>("MontoSuscripcion");
                                    break;
                            }
                            break;*/
                }

                if (result == null)
                {
                    throw new Exception($"el componente {Convert.ToString(propInfo.GetValue(type)).ToLower()} es null");
                }
                return result.MapperEnumerable<ValorCtrlResponse>(TypeMapper.IgnoreCaseSensitive).ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ValorCtrlResponse[] ObtenerConfigDeFormulario(FormularioResponse entity)
        {
            ValorCtrlResponse[] formValues = null;
            var dirTest = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = string.Empty;

            if (entity.FormularioId != 7)
            {
                path = $@"ArchivosMocks\{"frm-saf-001"}.xml";
            }
            else
            {
                path = $@"ArchivosMocks\{"FormConsAdhesiones"}.xml";
            }
            var dirForm = Path.Combine(dirTest, path);


            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Raiz";
            xRoot.IsNullable = true;

            var xml = new XmlSerializer(typeof(List<ValorCtrlMockResponse>), xRoot);

            using (XmlReader reader = XmlReader.Create(dirForm,
                new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore }
                ))
            {
                var lst = (List<ValorCtrlMockResponse>)xml.Deserialize(reader);

                formValues = lst.MapperEnumerable<ValorCtrlResponse>(TypeMapper.IgnoreCaseSensitive).ToArray();
            }

            return formValues;
        }

        /// <summary>
        /// Lee los archivos XML con los datos de pruebas.
        /// </summary>
        /// <param name="nombreComponente"></param>
        /// <returns></returns>
        private IEnumerable<T> ObtenerValoresDelComponente<T>(string nombreComponente)
        {
            try
            {

                List<T> formValues = null;
                var dirTest = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var dirForm = Path.Combine(dirTest, string.Format(@"ArchivosMocks\{0}.xml", nombreComponente));

                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "Raiz";
                xRoot.IsNullable = true;

                var xml = new XmlSerializer(typeof(List<T>), xRoot);

                using (XmlReader reader = XmlReader.Create(dirForm,
                    new XmlReaderSettings() { DtdProcessing = DtdProcessing.Ignore }
                    ))
                {
                    formValues = ((List<T>)xml.Deserialize(reader));
                }

                return formValues;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public decimal ConfirmarAdhesion(FormularioResponse entity, string TextDisclaimer)
        {
            return 0;
        }

        public long? ObtenerSiguienteFormulario(IFormulario entity, string componentes, string frmOrigen)
        {
            return 0;
        }

        public long? BajaAdhesion(FormularioResponse entity)
        {
            return 0;
        }

        public long? SimulacionBajaAdhesion(FormularioResponse entity)
        {
            return null;
        }

        public decimal? ValidarCuenta(decimal? cuentaTitulo, decimal? cuentaOperativa, int? tipoCuentaOperativa, int? sucursalCuentaOperativa, FormularioResponse entity)
        {
            return null;
        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            throw new NotImplementedException();
        }

        public string GetInfoDB(long id)
        {
            throw new NotImplementedException();
        }

        public void ActualizarComprobanteAJson(FormularioResponse frm)
        {
            throw new NotImplementedException();
        }

        public void LogFormulario(FormularioResponse entity, long? CodSimuAdhe, long? CodAltaAdhe, long? CodBajaAdhe)
        {
            throw new NotImplementedException();
        }

        public long ObtenerIdComponente(string componente, string usuario, string ip)
        {
            return 0;
        }

        public long? RegistrarOrden(RegistraOrdenRequest entity) //, long? IdAdhesion, long? IdAdhesionPDC, long? IdSimulacionPDC, long? CuentaTitulo, string NroCtaOperativa, string SucursalCtaOperativa, string TipoCtaOperativa, string CodigoMoneda, string Estado, DateTime? Fecha, string CodProducto)
        {
            throw new NotImplementedException();
        }

        public ConsultaOrdenResponse ConsultaOrden(FormularioResponse entity, long? CodSimuAdhe = null)
        {
            throw new NotImplementedException();
        }

        public ValorConsDeAdhesionesResp[] ObtenerConsultaDeAdhesiones(FormularioResponse entity, string cuentasOperativas = null, string cuentasTitulos = null)
        {
            var result = ObtenerValoresDelComponente<ConsDeAdhesionesMockResp>("ConsultaDeAdhesiones");

            return result.MapperEnumerable<ValorConsDeAdhesionesResp>(TypeMapper.IgnoreCaseSensitive).ToArray();
        }
        public string GetTextoDisclaimer(FormularioResponse frm)
        {
            throw new NotImplementedException();
        }

        public string ObtenerOrigen(FormularioResponse entity)
        {
            throw new NotImplementedException();
        }

        public decimal SimularAdhesion(FormularioResponse entity, long? cuentaTitulo = null)
        {
            throw new NotImplementedException();
        }

        public virtual ConsultaOrdenResponse ConsultaAlta(FormularioResponse entity, long? CodSimuAdhe = null)
        {
            throw new NotImplementedException();
        }
        public string ObtenerValorParametrizado(ConsultaParametrizacionReq entity)
        {
            throw new NotImplementedException();
        }

    }
}