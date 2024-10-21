using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Independientes;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    internal class ConfigLegal : ICrearComponente
    {
        private FormularioResponse entity;
        private Lista<ItemLegal<string>> _componente;

        public ConfigLegal(FormularioResponse formulario, Lista<ItemLegal<string>> componente)
        {
            entity = formulario;
            _componente = componente;
        }

        public void Crear()
        {
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            _componente.IdComponente = _componente.IdComponente == 0 ? daMapsControles.ObtenerIdComponente(_componente.Nombre, entity.Usuario, entity.Ip) : _componente.IdComponente;

            var defLista = daMapsControles.ObtenerDatosPorComponente(_componente, entity);

            //Obtengo los items padres
            var itemsPadre = defLista.Where(ip => ip.ItemGroupId != null
                                               && ip.ItemSubGrupId == null
                                               && ip.ControlAtributoPadreId != null
                                                ).GroupBy(gr => gr.ItemGroupId);

            //Setear valores de la lista legal
            foreach (var atr in defLista.Where(x => x.ItemGroupId == null && x.ItemSubGrupId == null).ToList())
            {
                var propInfo = this.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                if (propInfo != null && atr.AtributoValor != null)
                {
                    propInfo.SetValue(this, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                }
            }

            //A cada padre, le asigno sus hijos.
            itemsPadre.ToList().ForEach(ip =>
                {
                    var itemPadreInstance = new ItemLegal<string>();

                    //Seteo los valores de las propiedades del _componente padre
                    ip.Where(x => string.IsNullOrWhiteSpace(x.ItemSubGrupId)).ToList().ForEach(iPadre =>
                    {
                        var ipPropInfo = itemPadreInstance.GetType().GetProperty(iPadre.AtributoDesc, bindFlags);
                        var ipPropValue = iPadre.AtributoValor.ParseGenericVal(ipPropInfo.PropertyType);

                        ipPropInfo.SetValue(itemPadreInstance, ipPropValue);

                    });

                    //Seteo la propiedad _componente del tipo lista o list.
                    var newItemLista = itemPadreInstance.GetType().GetProperty("items", bindFlags).PropertyType.GetConstructor(Type.EmptyTypes).Invoke(null);
                    var listaItems = itemPadreInstance.GetType().GetProperty("items", bindFlags);
                    listaItems.SetValue(itemPadreInstance, newItemLista);


                    //Instancio los items genericos, internos de legal
                    var itemsHijos = defLista.Where(x => !string.IsNullOrWhiteSpace(x.ItemGroupId)
                                                   && x.ItemGroupId.Equals(ip.Key)
                                                   && !string.IsNullOrWhiteSpace(x.ItemSubGrupId))
                               .GroupBy(y => y.ItemSubGrupId);

                    itemsHijos.ToList().ForEach(ih =>
                {
                    var itemGenericInstance = Activator.CreateInstance(typeof(ItemGeneric));

                    //Seteo las propiedades
                    ih.ToList().ForEach(ish =>
                {
                    var ishPropInfo = itemGenericInstance.GetType().GetProperty(ish.AtributoDesc, bindFlags);
                    var ishpropValue = new object();
                    if (ish.ItemSubGrupId != null && ish.AtributoDesc == "TextoLink")
                    {
                        ishpropValue = ish.ValorAtributoCompExtendido.ParseGenericVal(ishPropInfo.PropertyType);
                    }
                    else
                    {
                        ishpropValue = ish.AtributoValor?.ParseGenericVal(ishPropInfo.PropertyType);

                    }

                    ishPropInfo.SetValue(itemGenericInstance, ishpropValue);

                });

                    //agrego un elemento fondos la lista.
                    var propItemsInstance = itemPadreInstance.GetType().GetProperty("items", bindFlags).GetValue(itemPadreInstance, null);
                    itemPadreInstance.GetType().GetProperty("items", bindFlags).PropertyType.GetMethod("add", bindFlags).Invoke(propItemsInstance, new[] { itemGenericInstance });

                });

                    //var newItem = (ItemLegal<string>)Convert.ChangeType(itemPadreInstance, typeof(ItemLegal<string>));
                    _componente.Items.Add(itemPadreInstance);
                });


        }
    }
}
