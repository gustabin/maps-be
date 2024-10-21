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
    public class ConfigServicio : ICrearComponente
    {
        private Lista<ItemServicio<string>> _componente;
        private FormularioResponse _entity;

        public ConfigServicio(FormularioResponse _formulario, Lista<ItemServicio<string>> item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
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
                var listaInstace = _componente;

                //setear las lista.

                defLista.ToList().ForEach(atr =>
                {
                    var propInfo = listaInstace.GetType().GetProperty(atr.AtributoDesc, bindFlags);
                    if (propInfo != null && !string.IsNullOrWhiteSpace(atr.AtributoValor))
                        propInfo.SetValue(listaInstace, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));

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
                var newItemLista = listaInstace.GetType().GetProperty("items", bindFlags).PropertyType.GetConstructor(Type.EmptyTypes).Invoke(null);
                var listaItems = listaInstace.GetType().GetProperty("items", bindFlags);
                listaItems.SetValue(listaInstace, newItemLista);

                //creo los items
                itemsLista.ToList().ForEach(itms =>
                {
                    //var srvInstance = Activator.CreateInstance(servtype);
                    var srvInstance = new ItemServicio<string>();

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
