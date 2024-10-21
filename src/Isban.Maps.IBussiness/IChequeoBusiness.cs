

namespace Isban.Maps.IBussiness
{
    using Entity;
    using Entity.Base;
    using Mercados.Service.InOut;
    using System.Collections.Generic;
    public interface IChequeoBusiness
    {
        List<ChequeoAcceso> Chequeo(EntityBase entity);
        DatoFirmaMaps ObtenerFirmaCertificada(RequestSecurity<DatoFirmaMaps> request);
    }

}
