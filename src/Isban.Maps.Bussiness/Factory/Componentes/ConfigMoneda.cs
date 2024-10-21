using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    public class ConfigMoneda : ICrearComponente
    {
        private Lista<ItemMoneda<string>> _componente;
        private FormularioResponse _entity;

        public ConfigMoneda(FormularioResponse _formulario, Lista<ItemMoneda<string>> item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var valores = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);

            var itemsLista = valores.Where(x => x.AtributoPadreId.HasValue && !string.IsNullOrWhiteSpace(x.ItemGroupId)).GroupBy(x => x.ItemGroupId);
            //Type itemTipoValor = ObtenerDataTypeDeItem(itemsLista);

            foreach (var itms in itemsLista)
            {
                var srvInstance = new ItemMoneda<string>();

                foreach (var atr in itms.ToList())
                {
                    var propInfo = srvInstance.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                    if (atr.AtributoValor != null)
                    {
                        propInfo.SetValue(srvInstance, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                    }
                }

                _componente.Items.Add(srvInstance);

            }


        }
    }

}