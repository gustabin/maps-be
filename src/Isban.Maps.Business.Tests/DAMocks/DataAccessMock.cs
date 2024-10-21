using Isban.Maps.Entity;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Interfaces;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Isban.Maps.Business.Tests
{
    public class DataAccessMock : IMapsControlesDA
    {
        public string ConnectionString
        {
            get
            {
                return "Data Source=RIO181H;enlist=true;User Id={0};Password={1};credentialId=30331";
            }
        }

        public List<CuentaAdheridaRTF> ConsultaArchivosRTF(RTFWorkflowOnDemandReq entity)
        {
            return new List<CuentaAdheridaRTF>();
        }

        public List<ArchivoRTF> ObtenerArchivoRTF(RTFWorkflowOnDemandReq entity)
        {
            return new List<ArchivoRTF>();
        }

        public UltimoPeriodoRTF ConsultaUltimoPeriodoRTF(RTFWorkflowOnDemandReq entity)
        {
            return new UltimoPeriodoRTF();
        }

        public void ActualizarComprobanteAJson(FormularioResponse frm)
        {
            throw new NotImplementedException();
        }

        public DetalleDeFondoResp ObtenerInfoFondo(InfoFondoReq entity)
        {
            return new DetalleDeFondoResp();
        }


        public decimal ConfirmarAdhesion(FormularioResponse frm, string asd, string asda)
        {
            return 0;
        }

        public void ActualizarTablaWizard(RegistrarPasoWizard entity)
        {
            throw new NotImplementedException();
        }

        public long? BajaAdhesion(FormularioResponse entity)
        {
            throw new NotImplementedException();
        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            return new ChequeoAcceso()
            {
                BasedeDatos = "TEST",
                ConnectionString = "Data Source = RIO181H; enlist = true; User Id = { 0 }; Password ={ 1}; credentialId = 30331",
                Hash = "",
                Ok = true,
                ServidorDB = "Test",
                ServidorWin = "unit test",
                UsuarioDB = "usuarioTest",
                UsuarioWin = "usuarioTestWin"
            };
        }

        public decimal ConfirmarAdhesion(FormularioResponse entity, string TextoDisclaimer = null)
        {
            throw new NotImplementedException();
        }

        public ConsultaOrigenResponse ConsultaOrigen(FormularioResponse entity)
        {
            throw new NotImplementedException();
        }

        public PasoWizardRes ConsultaPasoAnterior(PasoWizardReq entity)
        {
            throw new NotImplementedException();
        }

        public PasoWizardRes ConsultaPasoSiguiente(PasoWizardReq entity)
        {
            throw new NotImplementedException();
        }

        public PasoWizardRes ConsultaPasoSiguienteBaja(PasoWizardReq entity)
        {
            throw new NotImplementedException();
        }

        public string GetInfoDB(long id)
        {
            return "TEST";
        }

        public string GetTextoDisclaimer(FormularioResponse frm)
        {
            throw new NotImplementedException();
        }

        public void GuardarConfirmacionJson(GuardarConfirmacionJsonReq entity)
        {
            throw new NotImplementedException();
        }

        public void LogFormulario(FormularioResponse entity, long? CodSimuAdhe = default(long?), long? CodAltaAdhe = default(long?), long? CodBajaAdhe = default(long?))
        {
            ;
        }

        public ValorCtrlResponse[] ObtenerConfigDeFormulario(IFormulario entity)
        {
            throw new NotImplementedException();
        }

        public ValorCtrlResponse[] ObtenerConfigDeFormulario(FormularioResponse entity)
        {
            string testDirectory = TestContext.CurrentContext.TestDirectory;
            string filePath = entity.FormularioId == 6 ? @"DAMocks\Jsons\Simulacion.json" : @"DAMocks\Jsons\ValoresFormularioSAF.json";
            string path = Path.Combine(testDirectory, filePath);
            List<ValorCtrlResponse> items = null;

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                items = json.DeserializarToJson<List<ValorCtrlResponse>>();
            }

            return items.ToArray();
        }

        public ValorCtrlResponse[] ObtenerConfigDeFormulario(FormularioResponse entity, bool soloServicios = false)
        {
            throw new NotImplementedException();
        }

        public ValorCtrlResponse[] ObtenerConfigDeFormularioCargaCompletaPorServicio(FormularioResponse entity)
        {
            throw new NotImplementedException();
        }

        public ValorConsDeAdhesionesResp[] ObtenerConsultaDeAdhesiones(FormularioResponse entity, string cuentasOperativas = null, string cuentasTitulos = null)
        {
            throw new NotImplementedException();
        }

        public List<ConsultarDatosSimulacionConfirmacionResp> ObtenerDatosDeConfirmacion(FormularioResponse _entity)
        {
            throw new NotImplementedException();
        }

        public List<ConsultarDatosSimulacionConfirmacionResp> ObtenerDatosDeSimulacion(FormularioResponse _entity)
        {
            throw new NotImplementedException();
        }

        public ValorCtrlResponse[] ObtenerDatosPorComponente(ControlSimple type, FormularioResponse entity)
        {

            string testDirectory = TestContext.CurrentContext.TestDirectory;
            string filePath = $@"DAMocks\Jsons\{type.Nombre}.json";
            string path = Path.Combine(testDirectory, filePath);
            List<ValorCtrlResponse> items = null;

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                items = json.DeserializarToJson<List<ValorCtrlResponse>>();
            }

            return items.ToArray();

        }

        public DetalleDeFondoResp ObtenerDetalleDelFondo(DetalleDeFondoReq entity)
        {
            throw new NotImplementedException();
        }

        public ObtenerFormAdhesionesResp ObtenerFormAdhesiones(ObtenerFormAdhesionesReq entity)
        {
            throw new NotImplementedException();
        }

        public long? ObtenerFormularioIdOrigenFlujo(FormularioResponse entity)
        {
            throw new NotImplementedException();
        }

        public long? ObtenerFormularioIdOrigenFlujoBaja(FormularioResponse _formulario)
        {
            throw new NotImplementedException();
        }

        public ObtenerIdAdhesionResp ObtenerIdAdhesion(ObtenerIdAdhesionReq entity)
        {
            throw new NotImplementedException();
        }

        public long ObtenerIdComponente(string componente, string usuario, string ip)
        {
            return 1;
        }

        public OperacionesDisponiblesFondosResp ObtenerOperacionesFondo(OperacionesDisponiblesFondosReq entity)
        {
            throw new NotImplementedException();
        }

        public string ObtenerOrigen(FormularioResponse entity)
        {
            if (entity.FormularioId == 6)
                return "frm-6";
            else
                return string.Empty;

        }

        public ObtenerPasoResponse ObtenerPaso(ObtenerPasoReq entity)
        {
            throw new NotImplementedException();
        }

        public long? ObtenerSiguienteFormulario(IFormulario entity, string componentes, string frmOrigen)
        {
            long? result = null;

            switch (frmOrigen)
            {
                case "frm1":
                    result = null;
                    break;
                case "frm2":
                    result = 1;
                    break;
                case "frm3":
                    result = 2;
                    break;
                case "frm-6":
                    result = 6;
                    break;
                default:
                    throw new Exception();

            }

            return result;
        }

        public ValorCtrlResponse[] ObtenerTodosLosPasos(MapsBase datos)
        {
            throw new NotImplementedException();
        }

        public UsuarioRacf ObtenerUsuarioRacf()
        {
            var usr = new UsuarioRacf
            {
                Password = "12345679",
                Usuario = "B449999"
            };

            return usr;
        }

        public string ObtenerValorParametrizado(ConsultaParametrizacionReq entity)
        {
            switch (entity.NomParametro.ToLower())
            {
                case "ayuda":

                    return "ayuda test";

                case "etiqueta":
                    return "etiqueta test";

                default:
                    return "test";
            }
        }

        public long? RegistrarOrden(RegistraOrdenRequest entity)
        {
            throw new NotImplementedException();
        }

        public long? SimulacionBajaAdhesion(FormularioResponse entity)
        {
            throw new NotImplementedException();
        }

        public long? SimularAdhesion(FormularioResponse entity, long? cuentaTitulo = default(long?))
        {
            throw new NotImplementedException();
        }

        public decimal? ValidarCuenta(decimal? cuentaTitulo, decimal? cuentaOperativa, int? tipoCuentaOperativa, int? sucursalCuentaOperativa, FormularioResponse entity)
        {
            decimal cuenta = Convert.ToDecimal(entity.IdServicio == "SAF" ? cuentaOperativa.Value : cuentaTitulo.Value);

            if (cuenta == 0)
                return 0;
            else if (cuenta == 1)
                return 1;
            else
                return 0;
        }

        long? IMapsControlesDA.RegistrarPasoWizard(RegistrarPasoWizard entity)
        {
            throw new NotImplementedException();
        }

        public List<ConsultaFondosAGDResponse> ConsultaFondosAGD(ConsultaFondosAGDRequest entity)
        {
            throw new NotImplementedException();
        }

        public ConsultaCuentasMEP ObtenerCuentasMEP(string nup)
        {
            throw new NotImplementedException();
        }

        public ConsultaRestriccionAdhesion ObtenerRestriccionAdhesion(ValidaRestriccionMEPRequest entity)
        {
            throw new NotImplementedException();
        }
    }
}
