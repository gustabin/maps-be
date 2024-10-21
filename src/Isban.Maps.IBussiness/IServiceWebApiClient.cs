using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Mercados.Service.InOut;
using System.Collections.Generic;

namespace Isban.Maps.IBussiness
{
    public interface IServiceWebApiClient
    {
        ClienteDDC[] GetCuentas(RequestSecurity<GetCuentas> entity);

        ClienteDDC[] GetCuentasFisicasYjuridicas(RequestSecurity<GetCuentas> entity);
        List<Cliente> GetCuentasAptas(RequestSecurity<RequestCuentasAptas> entity);
        List<Cliente> GetClientePDC(GetClientePDC req);
        SimulaPdcResponse PDCSimulacionAltasBajas(RequestSecurity<SimulaPdcRequest> entity);

        ConsultaPerfilInversorResponse ConsultaPerfilInversor(RequestSecurity<ConsultaPerfilInversorRequest> request);
        LoadSaldosResponse ObtenerSaldoBP(RequestSecurity<LoadSaldosRequest> request);

        ValidaRestriccionMEPResponse ValidarRestriccionMEP(RequestSecurity<ValidaRestriccionMEPRequest> request);

        ValidaRestriccionMEPResponse ValidarRestriccionCMEP(RequestSecurity<ValidaRestriccionCMEPRequest> request);

        List<ClienteCuentaDDC> ConsultaCuentasCustodia(RequestSecurity<GetCuentas> entity, bool filtarOperativas);
        void VincularCuentasActivas(RequestSecurity<VincularCuentasActivasReq> request);

        void InsertarCuentasVinculadas(RequestSecurity<InsertarCuentasVinculadasReq> request);

        DiasNoHabilesResponse ConsultaDiasNoHabiles(RequestSecurity<DiasNoHabilesRequest> req);

        ValidarCNV907Response ValidarCNV907(RequestSecurity<ValidarCNV907Request> request);

    }
}
