
namespace Isban.Maps.IBussiness
{
    using Entity.Controles.Customizados;
    using Isban.Maps.Entity.Controles;
    using Isban.Maps.Entity.Controles.Compuestos;
    using Newtonsoft.Json.Linq;

    public interface IControlesBusiness
    {
        JObject ObtenerControlInputNumber(InputNumber<int> datos);
        //JObject ObtenerControlFondos(Lista<ItemFondo<string>> datos);
        JObject ObtenerControlFondos(Fondo<string> datos);
    }
}
