
using Isban.Maps.Entity.Base;

namespace Isban.Maps.IBussiness
{
    using Entity.Response;
    public interface IMapsPDCServiciosBusiness
    {
        FormularioResponse PDCAltaAdhesion(FormularioResponse entity, DatoFirmaMaps firma);  
    }
}
