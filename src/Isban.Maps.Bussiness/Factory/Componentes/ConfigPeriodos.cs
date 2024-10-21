using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    public class ConfigPeriodos : ICrearComponente
    {
        private Lista<Item<decimal>> _componente;
        private FormularioResponse _entity;

        public ConfigPeriodos(FormularioResponse _formulario, Lista<Item<decimal>> item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var valores = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);
            var itemsLista = valores.Where(x => x.AtributoPadreId.HasValue && !string.IsNullOrWhiteSpace(x.ItemGroupId)).GroupBy(x => x.ItemGroupId);
            //Type itemTipoValor = ObtenerDataTypeDeItem(itemsLista);

            itemsLista.ToList().ForEach(itms =>
            {
                var srvInstance = new Item<decimal>();

                PropertyInfo srvpropInfo;
                object srvpropValue;
                itms.ToList().ForEach(itm =>
                {
                    srvpropInfo = srvInstance.GetType().GetProperty(itm.AtributoDesc, bindFlags);
                    if (srvpropInfo != null)
                    {
                        srvpropValue = itm.AtributoValor.ParseGenericVal(srvpropInfo.PropertyType);
                        if (srvpropValue != null)
                            srvpropInfo.SetValue(srvInstance, srvpropValue);
                    }
                });
                                
                _componente.Items.Add(srvInstance);

            });   
        }
    }
}
