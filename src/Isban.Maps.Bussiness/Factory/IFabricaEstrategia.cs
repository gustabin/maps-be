using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Response;

namespace Isban.Maps.Bussiness.Factory
{
    public interface IFabricaEstrategia
    {
        ControlSimple Fabricar(string estrategia, FormularioResponse entity, ControlSimple item, DatoFirmaMaps firma);
    }
}