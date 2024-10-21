namespace Isban.Maps.Host.Controller
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Isban.Maps.Entity;
    using Isban.Maps.Entity.Base;
    using Isban.Maps.IBussiness;
    using Isban.Mercados.Service;
    using Isban.Mercados.Service.InOut;
    using Mercados.UnityInject;
    using Mercados;

    public class ChequeoServiceController : ServiceWebApiBase
    {
        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public Response<List<ChequeoAcceso>> Chequeo(Request<EntityBase> entity)
        {
            var business = DependencyFactory.Resolve<IChequeoBusiness>();
            return NonCheckSecurityAndTrace(() => business.Chequeo(entity.Datos), entity);
        }

        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public Response<DatoFirmaMaps> ObtenerFirmaCertificada(RequestSecurity<DatoFirmaMaps> entity)
        {
            var business =DependencyFactory.Resolve<IChequeoBusiness>();
            var a = entity.SerializarToJson();
            return NonCheckSecurityAndTrace(() => business.ObtenerFirmaCertificada(entity), entity);
        }
    }
}