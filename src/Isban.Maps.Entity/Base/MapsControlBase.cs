
namespace Isban.Maps.Entity.Base
{
    using Response;

    public abstract class MapsControlBase : Validaciones
    {   
        public abstract void AsignarDatosBackend(ValorCtrlResponse[] entity, string idServicio = null);            
    }
}
