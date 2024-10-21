using Isban.Maps.Entity.Response;

namespace Isban.Maps.Entity.Controles
{
    public interface IAsignarDatos
    {
        void AsignarDatosBackend(ValorCtrlResponse[] entity, string idServicio, ControlSimple obj);
    }
}