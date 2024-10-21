
namespace Isban.Maps.Entity.Controles
{
    using Constantes.Enumeradores;
    using Extensiones;
    using Independientes;
    using Interfaces;
    using Isban.Maps.Entity.Constantes.Estructuras;
    using Isban.Maps.Entity.Controles.Customizados;
    using Isban.Mercados.LogTrace;
    using Newtonsoft.Json;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class Lista<T> : ControlSimple
    {
        private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public Lista()
        {
            Items = new List<T>();
        }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PadreId { get; set; }

        [DataMember]
        public string TipoDataValor { get; set; }

        [DataMember]
        public List<T> Items { get; set; }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            #region Validacion Formulario Principal
            base.Validar();
            #endregion

            #region Control

            EsVacio("TipoDataValor", TipoDataValor);

            if (Items != null && Config != "cuenta-op-dolares-act")
            {
                TieneErrores = ValidarItems();
            }

            #endregion

            if (TieneErrores)
            {
                Error = (int)TiposDeError.ErrorValidacion;
                Error_desc = Errores;
                Error_tecnico += "Error de validación.";
                this.Validado = false;
                LoggingHelper.Instance.Error($"El formulario presenta los siguientes errores de validacion: {Errores}");
            }
            else
            {
                Error = (int)TiposDeError.NoError;
                Error_desc = null;
                Error_tecnico = null;
                this.Validado = true;
                this.Bloqueado = true;
            }

            return !TieneErrores;
        }

        private bool ValidarItems()
        {
            bool result = false;

            switch (this.Nombre.ToLower().Trim())
            {
                case TipoControl.Servicio:
                    result = ValidarSeleccion();
                    break;
                case TipoControl.Lista:
                    result = ValidarSeleccion();
                    break;
                case TipoControl.Moneda:
                    result = ValidarSeleccion();
                    break;
                case TipoControl.CuentaTitulo:
                    result = ValidarSeleccion();
                    break;

                case NombreComponente.ListaFondos:
                    //result = ValidarSeleccion();
                    break;
                case TipoControl.CuentaOperativa:
                    result = ValidarSeleccion();
                    break;
                case TipoControl.Fecha:
                    result = ValidarSeleccion();
                    break;
                case TipoControl.Operacion:
                    result = ValidarSeleccion();
                    break;
                case TipoControl.ConsultaAdhesiones:
                    result = ValidarSeleccion();
                    break;
                case TipoControl.Disclaimer:
                    //result = ValidarSeleccion();
                    break;
                case TipoControl.Legal:
                    //result = ValidarSeleccion();
                    result = ValidarSeleccionLegales();
                    break;
            }

            return result;
        }

        private bool ValidarSeleccion()
        {
            bool result = false;

            if (Items != null && Items.Count > 0)
            {
                if (Items != null && !Items.Any(x => (x as IItem).Seleccionado))
                {
                    result = true;
                    Errores += "Se debe seleccionar un item de la lista.";
                }

                if (Items != null && Items.Count(x => (x as IItem).Seleccionado) > 1)
                {
                    result = true;
                    Errores += "Se seleccionó más de una opción de la lista.";
                }
            }
            else
            {
                result = true;
                Errores += "No hay items para seleccionar. No se puede continuar.";
            }
            return result;
        }

        private bool ValidarSeleccionLegales()
        {
            bool result = false;

            if (Items != null && Items.Count > 0)
            {
                var itemLegal = Items as List<ItemLegal<string>>;

                itemLegal.ForEach(x =>
                {
                    if (x.Items.Any(y => y.Checked == false))
                    {
                        result = true;
                        Errores = "Se deben acepatar todos los legales.\n\r";
                    }
                });
            }

            return result;
        }

        public override void AsignarDatosBackend(ValorCtrlResponse[] entity, string idServicio = null, ControlSimple obj = null)
        {
            var vals = entity.ToList();

            if (vals != null && vals.Count() > 0)
            {
                string nombreControl = vals.Where(x => x.AtributoDesc.ToLower().Equals("nombre")).FirstOrDefault().AtributoValor;

                switch (nombreControl) //Componente
                {
                    case NombreComponente.CuentaOperativa: //"cuenta-operativa"
                        ConstruirItemsCuentaOperativa(vals);
                        break;
                    case NombreComponente.CuentaTitulo: //"cuenta-titulo"
                        ConstruirItemsCuentaTitulos(vals);
                        break;
                    case NombreComponente.ListaMoneda:
                        ConstruirItemsMoneda(vals);
                        break;
                    case NombreComponente.ListaFondos:
                        ConstruirItemsFondos(vals);
                        break;
                    case NombreComponente.Periodos: //"periodos"
                        ConstruirItemsPeriodos(vals);
                        break;
                    case NombreComponente.Operacion: //"operacion"
                        ConstruirItemsOperacion(vals);
                        break;
                    case NombreComponente.Legal:
                        GenerarListaLegal(vals, typeof(ItemLegal<string>));
                        break;
                    case NombreComponente.ListadoGenerico:
                        ConstruirListadoGenerico(vals);
                        break;
                    default:
                        ConstruirItems(vals);
                        break;
                }
            }
        }

        private void ConstruirItemsMoneda(IEnumerable<ValorCtrlResponse> list)
        {
            try
            {
                var itemsLista = list.Where(x => x.AtributoPadreId.HasValue && !string.IsNullOrWhiteSpace(x.ItemGroupId)).GroupBy(x => x.ItemGroupId);
                //Type itemTipoValor = ObtenerDataTypeDeItem(itemsLista);

                foreach (var itms in itemsLista)
                {
                    var srvInstance = Activator.CreateInstance(typeof(T));

                    foreach (var atr in itms.ToList())
                    {
                        var propInfo = srvInstance.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                        if (atr.AtributoValor != null)
                        {
                            propInfo.SetValue(srvInstance, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                        }
                    }

                    var newItem = (T)Convert.ChangeType(srvInstance, typeof(T));
                    this.Items.Add(newItem);

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ConstruirItemsCuentaTitulos(List<ValorCtrlResponse> list)
        {
            try
            {
                var itemsLista = list.Where(x => x.AtributoPadreId != null).GroupBy(x => x.ItemSubGrupId);
                //Type itemTipoValor = ObtenerDataTypeDeItem(itemsLista);   

                foreach (var itms in itemsLista)
                {
                    var srvInstance = Activator.CreateInstance(typeof(T));

                    foreach (var atr in itms.ToList())
                    {
                        var propInfo = srvInstance.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                        if (atr.AtributoValor != null)
                        {
                            propInfo.SetValue(srvInstance, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                        }
                    }

                    var newItem = (T)Convert.ChangeType(srvInstance, typeof(T));
                    this.Items.Add(newItem);

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ConstruirListadoGenerico(List<ValorCtrlResponse> list)
        {
            try
            {
                var itemsLista = list.Where(x => x.AtributoPadreId != null).GroupBy(x => x.ItemSubGrupId);

                foreach (var itms in itemsLista)
                {
                    var srvInstance = Activator.CreateInstance(typeof(T));   //TODO: el tipo lo deberia obtener de la configuración de la BD. Si esta mal rompería.

                    foreach (var atr in itms.ToList())
                    {
                        var propInfo = srvInstance.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                        if (atr.AtributoValor != null)
                        {
                            propInfo.SetValue(srvInstance, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                        }
                    }

                    var newItem = (T)Convert.ChangeType(srvInstance, typeof(T));
                    this.Items.Add(newItem);

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ConstruirItemsCuentaOperativa(List<ValorCtrlResponse> list)
        {
            try
            {
                var itemsLista = list.Where(x => x.AtributoPadreId != null).GroupBy(x => x.ItemSubGrupId);
                //Type itemTipoValor = ObtenerDataTypeDeItem(itemsLista);

                foreach (var itms in itemsLista)
                {
                    var srvInstance = Activator.CreateInstance(typeof(T));

                    foreach (var atr in itms.ToList())
                    {
                        var propInfo = srvInstance.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                        if (atr.AtributoValor != null)
                        {
                            propInfo.SetValue(srvInstance, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                        }
                    }

                    var newItem = (T)Convert.ChangeType(srvInstance, typeof(T));
                    this.Items.Add(newItem);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ConstruirItemsFondos(IEnumerable<ValorCtrlResponse> list) ///Revisarlo con Sebas.
        {
            try
            {
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

                    var newItem = (T)Convert.ChangeType(srvInstance, typeof(T));
                    this.Items.Add(newItem);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConstruirItemsPeriodos(IEnumerable<ValorCtrlResponse> list)
        {
            try
            {
                var itemsLista = list.Where(x => x.AtributoPadreId.HasValue && !string.IsNullOrWhiteSpace(x.ItemGroupId)).GroupBy(x => x.ItemGroupId);
                //Type itemTipoValor = ObtenerDataTypeDeItem(itemsLista);

                itemsLista.ToList().ForEach(itms =>
                {
                    var srvInstance = Activator.CreateInstance(typeof(T));

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

                    var newItem = (T)Convert.ChangeType(srvInstance, typeof(T));
                    this.Items.Add(newItem);
                });

            }
            catch (Exception)
            {

                throw;
            }
        }

        private Type ObtenerDataTypeDeItem(IEnumerable<IGrouping<string, ValorCtrlResponse>> itemsLista)
        {
            bool tineValor = itemsLista.FirstOrDefault().Any(x => x.AtributoDesc.ToLower().Equals("valor"));
            Type itemTipoValor = null;

            if (tineValor)
            {
                itemTipoValor = itemsLista.FirstOrDefault().Where(x => x.AtributoDesc.ToLower().Equals("valor")).Select(y => y.AtributoDataType).FirstOrDefault().ToType();
            }
            else
            {
                itemTipoValor = itemsLista.FirstOrDefault().Where(x => x.AtributoDesc.ToLower().Equals("tipo")).Select(y => y.AtributoDataType).FirstOrDefault().ToType();
            }

            return itemTipoValor;
        }

        private void ConstruirItemsOperacion(IEnumerable<ValorCtrlResponse> list)
        {
            var itemsLista = list.Where(x => x.ItemSubGrupId != null && x.AtributoDesc != "ValorPadre").GroupBy(x => x.ItemSubGrupId);
            //Type itemTipoValor = ObtenerDataTypeDeItem(itemsLista);

            itemsLista.ToList().ForEach(itms =>
            {
                var srvInstance = Activator.CreateInstance(typeof(T));

                PropertyInfo srvpropInfo;
                object srvpropValue;
                itms.ToList().ForEach(itm =>
                {
                    srvpropInfo = srvInstance.GetType().GetProperty(itm.AtributoDesc, bindFlags);
                    srvpropValue = itm.AtributoValor.ParseGenericVal(srvpropInfo.PropertyType);
                    if (srvpropInfo != null && srvpropValue != null)
                        srvpropInfo.SetValue(srvInstance, srvpropValue);
                });

                var newItem = (T)Convert.ChangeType(srvInstance, typeof(T));
                this.Items.Add(newItem);
            });
        }

        private void ConstruirItems(IEnumerable<ValorCtrlResponse> list)
        {
            if (list != null && list.Count() > 0)
            {
                var defLista = list.Where(x => x.ControlPadreId.HasValue && x.ControlAtributoPadreId.HasValue && !x.AtributoPadreId.HasValue);

                long listaPadreId = defLista.Select(x => x.IdComponente).FirstOrDefault();
                long? listaPropItemID = defLista.Where(x => x.AtributoDesc.ToLower().Equals("items")).Select(x => x.ControlId).FirstOrDefault();
                var listaDataType = defLista.Where(x => x.AtributoDesc.ToLower().Equals("tipo"))
                                        .Select(y => y.AtributoDataType)
                                        .FirstOrDefault()
                                        .ToType();

                var listaItemsTipo = defLista.Where(x => x.AtributoDesc.ToLower().Equals("nombre")).FirstOrDefault().AtributoValor.ToControlMaps(listaDataType);
                var listaInstace = this;

                //setear las lista.

                defLista.ToList().ForEach(atr =>
                {
                    var propInfo = listaInstace.GetType().GetProperty(atr.AtributoDesc, bindFlags);
                    if (propInfo != null && !string.IsNullOrWhiteSpace(atr.AtributoValor))
                        propInfo.SetValue(listaInstace, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));

                });

                //obtener los items 
                var itemsLista = list.Where(x => x.AtributoPadreId.HasValue
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
                    var srvInstance = Activator.CreateInstance(typeof(T));

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

                    var newItem = (T)Convert.ChangeType(srvInstance, typeof(T));
                    this.Items.Add(newItem);
                });
            }
        }

        private void GenerarListaLegal(IEnumerable<ValorCtrlResponse> defLista, Type listaItemsTipo)
        {
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
                var itemPadreInstance = Activator.CreateInstance(listaItemsTipo);

                //Seteo los valores de las propiedades del item padre
                ip.Where(x => string.IsNullOrWhiteSpace(x.ItemSubGrupId)).ToList().ForEach(iPadre =>
        {
            var ipPropInfo = itemPadreInstance.GetType().GetProperty(iPadre.AtributoDesc, bindFlags);
            var ipPropValue = iPadre.AtributoValor.ParseGenericVal(ipPropInfo.PropertyType);

            ipPropInfo.SetValue(itemPadreInstance, ipPropValue);

        });

                //Seteo la propiedad item del tipo lista o list.
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
                    ishpropValue = ish.AtributoValor.ParseGenericVal(ishPropInfo.PropertyType);
                }

                ishPropInfo.SetValue(itemGenericInstance, ishpropValue);
            });

                    //agrego un elemento fondos la lista.
                    var propItemsInstance = itemPadreInstance.GetType().GetProperty("items", bindFlags).GetValue(itemPadreInstance, null);
                    itemPadreInstance.GetType().GetProperty("items", bindFlags).PropertyType.GetMethod("add", bindFlags).Invoke(propItemsInstance, new[] { itemGenericInstance });

                });

                var newItem = (T)Convert.ChangeType(itemPadreInstance, typeof(T));

                this.Items.Add(newItem);
            });
        }
    }
}
