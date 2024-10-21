
namespace Isban.Maps.IDataAccess
{
    using Entity;
    using Entity.Base;
    using Entity.Controles;
    using Entity.Interfaces;
    using Entity.Response;
    using Isban.Maps.Entity.Request;
    using System;
    using System.Collections.Generic;

    public interface IMapsControlesDA
    {
        string ConnectionString { get; }

        ChequeoAcceso Chequeo(EntityBase entity);

        string GetInfoDB(long id);

        void LogFormulario(FormularioResponse entity, long? CodSimuAdhe = null, long? CodAltaAdhe = null, long? CodBajaAdhe = null);

        long? ObtenerSiguienteFormulario(IFormulario entity, string componentes, string frmOrigen);

        ValorCtrlResponse[] ObtenerDatosPorComponente(ControlSimple type, FormularioResponse entity);

        ValorCtrlResponse[] ObtenerConfigDeFormulario(FormularioResponse entity, bool soloServicios = false);

        ValorCtrlResponse[] ObtenerConfigDeFormulario(IFormulario entity);

        ValorConsDeAdhesionesResp[] ObtenerConsultaDeAdhesiones(FormularioResponse entity, string cuentasOperativas = null, string cuentasTitulos = null);

        long? BajaAdhesion(FormularioResponse entity);

        long? RegistrarOrden(RegistraOrdenRequest entity);

        long? SimulacionBajaAdhesion(FormularioResponse entity);

        PasoWizardRes ConsultaPasoSiguienteBaja(PasoWizardReq entity);

        long? SimularAdhesion(FormularioResponse entity, long? cuentaTitulo = null);

        decimal ConfirmarAdhesion(FormularioResponse entity, string TextoDisclaimer = null, string CuentaTitulo = null);

        decimal? ValidarCuenta(decimal? cuentaTitulo, decimal? cuentaOperativa, int? tipoCuentaOperativa, int? sucursalCuentaOperativa, FormularioResponse entity);

        void ActualizarComprobanteAJson(FormularioResponse frm);

        long ObtenerIdComponente(string componente, string usuario, string ip);

        ConsultaOrigenResponse ConsultaOrigen(FormularioResponse entity);

        string GetTextoDisclaimer(FormularioResponse frm);

        string ObtenerOrigen(FormularioResponse entity);

        string ObtenerValorParametrizado(ConsultaParametrizacionReq entity);

        PasoWizardRes ConsultaPasoSiguiente(PasoWizardReq entity);

        PasoWizardRes ConsultaPasoAnterior(PasoWizardReq entity);

        ObtenerPasoResponse ObtenerPaso(ObtenerPasoReq entity);

        ObtenerFormAdhesionesResp ObtenerFormAdhesiones(ObtenerFormAdhesionesReq entity);

        ObtenerIdAdhesionResp ObtenerIdAdhesion(ObtenerIdAdhesionReq entity);

        long? RegistrarPasoWizard(RegistrarPasoWizard entity);

        void GuardarConfirmacionJson(GuardarConfirmacionJsonReq entity);

        UsuarioRacf ObtenerUsuarioRacf();

        List<ConsultarDatosSimulacionConfirmacionResp> ObtenerDatosDeSimulacion(FormularioResponse _entity);

        List<ConsultarDatosSimulacionConfirmacionResp> ObtenerDatosDeConfirmacion(FormularioResponse _entity);

        long? ObtenerFormularioIdOrigenFlujo(FormularioResponse formulario);

        long? ObtenerFormularioIdOrigenFlujoBaja(FormularioResponse _formulario);
        ValorCtrlResponse[] ObtenerTodosLosPasos(MapsBase datos);

        DetalleDeFondoResp ObtenerDetalleDelFondo(DetalleDeFondoReq entity);

        OperacionesDisponiblesFondosResp ObtenerOperacionesFondo(OperacionesDisponiblesFondosReq entity);

        DetalleDeFondoResp ObtenerInfoFondo(InfoFondoReq entity);

        List<CuentaAdheridaRTF> ConsultaArchivosRTF(RTFWorkflowOnDemandReq entity);

        List<ArchivoRTF> ObtenerArchivoRTF(RTFWorkflowOnDemandReq entity);

        UltimoPeriodoRTF ConsultaUltimoPeriodoRTF(RTFWorkflowOnDemandReq entity);

        List<ConsultaFondosAGDResponse> ConsultaFondosAGD(ConsultaFondosAGDRequest entity);

        ConsultaCuentasMEP ObtenerCuentasMEP(string nup);

        ConsultaRestriccionAdhesion ObtenerRestriccionAdhesion(ValidaRestriccionMEPRequest entity);
    }
}
