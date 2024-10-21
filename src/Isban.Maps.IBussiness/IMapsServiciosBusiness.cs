

using Isban.Maps.Entity.Base;

namespace Isban.Maps.IBussiness
{
    using Entity.Response;
    using Isban.Maps.Entity.Request;
    using Isban.Mercados.Service.InOut;
    using System.Collections.Generic;

    public interface IMapsServiciosBusiness
    {
        FormularioResponse AltaAdhesion(FormularioResponse entity, DatoFirmaMaps firma);
        FormularioResponse BajaAdhesion(FormularioResponse entity, DatoFirmaMaps firma);
        FormularioResponse ConsultaAdhesion(FormularioResponse entity, DatoFirmaMaps firma);
        ObtenerFormAdhesionesResp ObtenerFormAdhesiones(ObtenerFormAdhesionesReq entity);
        ObtenerIdAdhesionResp ObtenerIdAdhesion(ObtenerIdAdhesionReq entity);

        ObtenerRTFDisponiblesResponse ObtenerRTFDisponiblesPorCliente(RequestSecurity<RTFWorkflowOnDemandReq> entity);

        List<ArchivoRTF> ObtenerPdfPorCuentaRTF(RequestSecurity<RTFWorkflowOnDemandReq> entity);

        List<ConsultaFondosAGDResponse> ConsultaFondosAGD(ConsultaFondosAGDRequest entity);
    }
}
