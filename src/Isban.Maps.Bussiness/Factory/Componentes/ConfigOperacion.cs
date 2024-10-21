using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    public class ConfigOperacion : ICrearComponente
    {
        private Lista<Item<string>> _componente;
        private FormularioResponse _entity;

        public ConfigOperacion(FormularioResponse _formulario, Lista<Item<string>> item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var list = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);
            var itemsLista = list.Where(x => x.ItemSubGrupId != null && x.AtributoDesc != "ValorPadre").GroupBy(x => x.ItemSubGrupId);
            //Type itemTipoValor = ObtenerDataTypeDeItem(itemsLista);

            itemsLista.ToList().ForEach(itms =>
            {
                var srvInstance = new Item<string>();

                PropertyInfo srvpropInfo;
                object srvpropValue;

                itms.ToList().ForEach(itm =>
                {
                    srvpropInfo = srvInstance.GetType().GetProperty(itm.AtributoDesc, bindFlags);
                    srvpropValue = itm.AtributoValor.ParseGenericVal(srvpropInfo.PropertyType);
                    if (srvpropInfo != null && srvpropValue != null)
                        srvpropInfo.SetValue(srvInstance, srvpropValue);
                });

                var newItem = (Item<string>)Convert.ChangeType(srvInstance, typeof(Item<string>));
                _componente.Items.Add(newItem);
            });
        }
    }   
}
