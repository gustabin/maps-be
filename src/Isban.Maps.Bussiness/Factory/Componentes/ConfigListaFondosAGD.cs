using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Bussiness.Factory.Componentes
{
    internal class ConfigListaFondosAGD : ICrearComponente
    {
        private FormularioResponse _entity;
        private Lista<ItemGrupoAgd> _componente;


        public ConfigListaFondosAGD(FormularioResponse entity, Lista<ItemGrupoAgd> item)
        {
            _entity = entity;
            _componente = item;
        }

        public void Crear()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var list = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);
            //Obtener itemGrupo
            var itemsLista = list.Where(x => x.AtributoPadreId.HasValue && !string.IsNullOrWhiteSpace(x.ItemGroupId)).GroupBy(x => x.ItemGroupId);

            foreach (var itms in itemsLista)
            {
                #region Grupo
                var srvInstance = new ItemGrupoAgd();

                foreach (var atr in itms.ToList())
                {
                    var propInfo = srvInstance.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                    if (propInfo != null && atr.AtributoValor != null)
                    {
                        propInfo.SetValue(srvInstance, atr.AtributoValor.ParseGenericVal(propInfo.PropertyType));
                    }
                }

                _componente.Items.Add(srvInstance);
                #endregion

                #region FondoGrupo
                //Obtener los itemsFondoGrupo por cada uno de los grupos.
                var itemsFondoGrupo = itms.Where(x => x.ItemSubGrupId != null && x.AtributoDesc != "ValorPadre").GroupBy(x => x.ItemSubGrupId);

                if (itemsFondoGrupo != null && itemsFondoGrupo.Count() > 0)
                {
                    foreach (var itemFondoGrupo in itemsFondoGrupo)
                    {
                        var srvInstanceFondoGrupo = new ItemFondoAgd();
                        var toolTipAgd = new TooltipAgd();

                        foreach (var itemAttrFondoGrupo in itemFondoGrupo.ToList())
                        {
                            var propInfoFondoGrupo = srvInstanceFondoGrupo.GetType().GetProperty(itemAttrFondoGrupo.AtributoDesc, bindFlags);

                            if (propInfoFondoGrupo != null && itemAttrFondoGrupo.AtributoValor != null)
                            {
                                propInfoFondoGrupo.SetValue(srvInstanceFondoGrupo, itemAttrFondoGrupo.AtributoValor.ParseGenericVal(propInfoFondoGrupo.PropertyType));
                            }

                            propInfoFondoGrupo = toolTipAgd.GetType().GetProperty(itemAttrFondoGrupo.AtributoDesc, bindFlags);

                            if (propInfoFondoGrupo != null && itemAttrFondoGrupo.AtributoValor != null)
                            {
                                propInfoFondoGrupo.SetValue(toolTipAgd, itemAttrFondoGrupo.AtributoValor.ParseGenericVal(propInfoFondoGrupo.PropertyType));
                            }

                        }
                        srvInstanceFondoGrupo.ToolTip = toolTipAgd;
                        srvInstance.Items.Add(srvInstanceFondoGrupo);
                    }
                }
                #endregion
            }

        }
    }
}
