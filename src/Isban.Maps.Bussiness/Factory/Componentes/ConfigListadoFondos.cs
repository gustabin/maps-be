using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    internal class ConfigListadoFondos : ICrearComponente
    {
        private Lista<Item<string>> _componente;
        private FormularioResponse _entity;

        public ConfigListadoFondos(FormularioResponse _formulario, Lista<Item<string>> item)
        {
            _entity = _formulario;
            _componente = item;
        }  

        public void Crear()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var valores = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);

            if (valores != null && valores.Length > 0)
            {
                var defLista = valores.Where(x => x.ControlPadreId.HasValue && x.ControlAtributoPadreId.HasValue && !x.AtributoPadreId.HasValue);

                long listaPadreId = defLista.Select(x => x.IdComponente).FirstOrDefault();
                long? listaPropItemID = defLista.Where(x => x.AtributoDesc.ToLower().Equals("items")).Select(x => x.ControlId).FirstOrDefault();
                var listaDataType = defLista.Where(x => x.AtributoDesc.ToLower().Equals("tipo"))
                                        .Select(y => y.AtributoDataType)
                                        .FirstOrDefault()
                                        .ToType();

                var listaItemsTipo = defLista.Where(x => x.AtributoDesc.ToLower().Equals("nombre")).FirstOrDefault().AtributoValor.ToControlMaps(listaDataType);

                //setear las lista.

                defLista.ToList().ForEach(atr =>
                {
                    var propInfo = _componente.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                    if (propInfo != null && atr.AtributoDesc == "Ayuda")
                    {
                        if (string.IsNullOrEmpty(atr.AtributoValor))
                        {
                            propInfo.SetValue(_componente, atr.ValorAtributoCompExtendido);
                        }
                        else
                        {
                            propInfo.SetValue(_componente, atr.AtributoValor);
                        }
                    }
                    else
                    {
                        if (propInfo != null && !string.IsNullOrWhiteSpace(atr.AtributoValor))
                            propInfo.SetValue(_componente, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));

                    }


                });


                //obtener los items 
                var itemsLista = valores.Where(x => x.AtributoPadreId.HasValue
                                             && x.AtributoPadreId.Equals(listaPropItemID)
                                             && x.IdComponente.Equals(listaPadreId)
                                             && x.ControlPadreId.HasValue
                                             && x.ControlPadreId.Equals(defLista.FirstOrDefault().ControlPadreId)
                                             && x.ControlAtributoPadreId.HasValue
                                             && x.ControlAtributoPadreId.Equals(defLista.FirstOrDefault().ControlAtributoPadreId)
                                             ).GroupBy(x => x.ItemGroupId);

                //instanacio la lista de servicio
                var newItemLista = _componente.GetType().GetProperty("items", bindFlags).PropertyType.GetConstructor(Type.EmptyTypes).Invoke(null);
                var listaItems = _componente.GetType().GetProperty("items", bindFlags);
                listaItems.SetValue(_componente, newItemLista);

                //creo los items
                itemsLista.ToList().ForEach(itms =>
                {
                    //var srvInstance = Activator.CreateInstance(servtype);
                    var srvInstance = new Item<string>();

                    itms.ToList().ForEach(itm =>
                    {
                        var srvpropInfo = srvInstance.GetType().GetProperty(itm.AtributoDesc, bindFlags);

                        if (srvpropInfo != null)
                        {
                            var srvpropValue = itm.AtributoValor.ParseGenericVal(srvpropInfo.PropertyType);

                            if (srvpropInfo != null)
                                srvpropInfo.SetValue(srvInstance, srvpropValue);
                        }
                    });

                    _componente.Items.Add(srvInstance);
                });
            }
        }
    }
}
