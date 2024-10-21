using Isban.Maps.Business.Componente.Factory;
using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness.Factory.Componentes;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Compuestos;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Controles.Independientes;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.LogTrace;
using Isban.Mercados.UnityInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Isban.Maps.Bussiness.Factory.Formularios
{
    public static class FormularioHelper
    {
        private static List<string> propExcluidas = new List<string> { "items", "nup", "segmento", "canal", "subcanal", "idsimulacion", "cabecera" };
        [Obsolete("Usar como por defecto CrearFormulario que hace todo junto. Quitar este método a futuro.")]
        public static void CrearItems(FormularioResponse _entity, DatoFirmaMaps _firma)
        {
            #region Creación por Item
            Parallel.ForEach(_entity.Items, item =>
            {
                //foreach (var item in _entity.Items)
                //{
                switch (item.Nombre)
                {
                    case NombreComponente.CuentaOperativa: //"cuenta-operativa"
                        var co = new EstrategiaComp(new ConfigCuentaOperativa(_entity, (Lista<ItemCuentaOperativa<string>>)item, _firma));
                        co.Crear();

                        break;
                    case NombreComponente.CuentaTitulo: //"cuenta-titulo"
                        var ct = new EstrategiaComp(new ConfigCuentaTitulo(_entity, (Lista<ItemCuentaTitulos<string>>)item, _firma));
                        ct.Crear();

                        break;
                    case NombreComponente.Moneda: //"moneda"
                        var mo = new EstrategiaComp(new ConfigMoneda(_entity, (Lista<ItemMoneda<string>>)item));
                        mo.Crear();

                        break;
                    case NombreComponente.ListadoAsesoramiento:
                    case NombreComponente.ListaPep:
                    case NombreComponente.ListadoFondos:
                        var lfondo = new EstrategiaComp(new ConfigListadoFondos(_entity, (Lista<Item<string>>)item));
                        lfondo.Crear();

                        break;
                    case NombreComponente.ListadoGenerico:
                    case NombreComponente.Operacion: //"operacion"
                        var oper = new EstrategiaComp(new ConfigOperacion(_entity, (Lista<Item<string>>)item));
                        oper.Crear();

                        break;
                    case NombreComponente.Periodos: //"periodos"
                        var per = new EstrategiaComp(new ConfigPeriodos(_entity, (Lista<Item<decimal>>)item));
                        per.Crear();

                        break;
                    case NombreComponente.Servicio:
                        var ser = new EstrategiaComp(new ConfigServicio(_entity, (Lista<ItemServicio<string>>)item));
                        ser.Crear();

                        break;
                    case NombreComponente.Email:
                        var email = new EstrategiaComp(new ConfigEmail(_entity, (InputText<string>)item));
                        email.Crear();

                        break;
                    case NombreComponente.Alias:
                        var alias = new EstrategiaComp(new ConfigAlias(_entity, (InputText<string>)item));
                        alias.Crear();
                        break;
                    case NombreComponente.SaldoMinimo:
                    case NombreComponente.MontoSuscripcionMinimo:
                    case NombreComponente.MontoSuscripcionMaximo:
                        var salMin = new EstrategiaComp(new ConfigSaldoMinimo(_entity, (InputNumber<decimal?>)item));
                        salMin.Crear();
                        break;

                    case NombreComponente.FechaEjecucion:
                    case NombreComponente.FechaDesde:
                    case NombreComponente.FechaHasta:
                    case NombreComponente.Fecha:
                        var fecha = new EstrategiaComp(new ConfigFecha(_entity, (Fecha)item));
                        fecha.Crear();

                        break;
                    case NombreComponente.Legal:
                    case NombreComponente.LegalAgendamiento:
                        var legal = new EstrategiaComp(new ConfigLegal(_entity, (Lista<ItemLegal<string>>)item));
                        legal.Crear();
                        break;
                    case NombreComponente.Vigencia:
                    case NombreComponente.FechaCompuesta:
                        var fecComp = new EstrategiaComp(new ConfigFechaCompuesta(_entity, (FechaCompuesta)item));
                        fecComp.Crear();

                        break;
                    case NombreComponente.ImporteCompuesto:
                    case NombreComponente.MontoSuscripcion:
                        var impComp = new EstrategiaComp(new ConfigImporteCompuesto(_entity, (ImporteCompuesto)item));
                        impComp.Crear();
                        break;
                    case NombreComponente.FondoCompuesto:
                        var fonComp = new EstrategiaComp(new ConfigFondoCompuesto(_entity, (FondoCompuesto)item));
                        fonComp.Crear();
                        break;

                    case NombreComponente.ListaFondos://usado para fondo compuesto
                        var listaFondos = new EstrategiaComp(new ConfigListaFondosAGD(_entity, (Lista<ItemGrupoAgd>)item));
                        listaFondos.Crear();
                        break;
                    default:
                        ;
                        break;
                }

            });
            #endregion
        }

        public static void AsignarPadreAHijo(FormularioResponse _entity)
        {
            var aItemsEliminar = new List<ControlSimple>();

            foreach (var currItem in _entity.Items)
            {
                if (currItem.IdPadreDB != null && currItem.IdPadreDB != 0 && currItem.IdPadreDB != _entity.IdComponente)
                {
                    var itemPadre = (from ControlSimple itemContenedor in _entity.Items
                                     where itemContenedor.IdComponente == currItem.IdPadreDB
                                     select itemContenedor).FirstOrDefault();

                    _entity.AsignarControlHijoAControlPadre(itemPadre, currItem);

                    aItemsEliminar.Add(currItem);
                }

            }

            foreach (var item in aItemsEliminar)
            {
                _entity.Items.Remove(item);
            }
        }

        public static void DepurarControles(FormularioResponse form)
        {
            form.Items.ForEach(componente =>
            {//por componente depurar las listas para cuando viajen con un elemento seleccionado.
                try
                {
                    switch (componente.Nombre)
                    {
                        case NombreComponente.ListaFondos:

                            var fondos = componente as Lista<ItemGrupoAgd>;
                            var fondoSeleccionado = fondos.Items.SelectMany(x => x.Items.Where(y => y.Seleccionado == true));

                            if (fondoSeleccionado.Count() > 1)
                            {
                                throw new BusinessException("Fue seleccionado más de un fondo para uno o varios grupos");
                            }
                            else if (fondoSeleccionado.Count() == 1)
                            {
                                var itemGrupo = fondos.Items.Where(x => x.Items.Contains(fondoSeleccionado.FirstOrDefault())).FirstOrDefault();
                                itemGrupo.Items.RemoveAll(x => x.Seleccionado != true);

                                fondos.Items.Clear();
                                fondos.Items.Add(itemGrupo);
                                #region revisar si va acá
                                fondos.Bloqueado = true;
                                fondos.Validado = true;
                                #endregion
                            }
                            break;
                        case NombreComponente.CuentaOperativa:

                            var ctaOper = componente as Lista<ItemCuentaOperativa<string>>;

                            if (ctaOper.Items.Count > 1 && ctaOper.Items.Where(x => x.Seleccionado == true).Count() > 0)
                                ctaOper.Items.RemoveAll(x => x.Seleccionado != true);
                            break;

                        case NombreComponente.CuentaTitulo:
                            var ctaTit = componente as Lista<ItemCuentaTitulos<string>>;

                            if (ctaTit.Items.Count > 1 && ctaTit.Items.Where(x => x.Seleccionado == true).Count() > 0)
                                ctaTit.Items.RemoveAll(x => x.Seleccionado != true);
                            break;
                        case NombreComponente.Moneda:
                        case NombreComponente.ListaMoneda:
                            var monedas = componente as Lista<ItemMoneda<string>>;

                            if (monedas.Items.Count > 1 && monedas.Items.Where(x => x.Seleccionado == true).Count() > 0)
                                monedas.Items.RemoveAll(x => x.Seleccionado != true);
                            break;
                        case NombreComponente.ListadoFondos:
                        case NombreComponente.ListadoAsesoramiento:
                        case NombreComponente.ListaPep:
                        case NombreComponente.ListadoGenerico:
                        case NombreComponente.Operacion:
                            var operacion = componente as Lista<Item<string>>;

                            if (operacion.Items.Count > 1 && operacion.Items.Where(x => x.Seleccionado == true).Count() > 0)
                                operacion.Items.RemoveAll(x => x.Seleccionado != true);
                            break;

                        case NombreComponente.Vigencia:
                            var fechaCompuesta = componente as FechaCompuesta;

                            var periodos = fechaCompuesta.Items.GetControlMaps<Lista<Item<decimal>>>(NombreComponente.Periodos);
                            if (periodos.Items.Count > 1 && periodos.Items.Where(x => x.Seleccionado == true).Count() > 0)
                                periodos.Items.RemoveAll(x => x.Seleccionado != true);

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LoggingHelper.Instance.Error(ex, $"Error al depurar el componente lista {componente.Nombre}");
                }
            });

        }


        public static void SetearRelaciones(FormularioResponse formSiguiente)
        {
            formSiguiente.Items.ForEach(item =>
            {

                switch (item.Nombre)
                {
                    case NombreComponente.FondoCompuesto:
                        var comp = item as FondoCompuesto;
                        var fondoAGD = (comp.Items.Where(x => x.Nombre == NombreComponente.ListaFondos)).FirstOrDefault() as Lista<ItemGrupoAgd>;
                        var fondoSeleccionado = fondoAGD.Items.SelectMany(x => x.Items.Where(y => y.Seleccionado == true)).FirstOrDefault();
                        var ctrlMoneda = formSiguiente.Items.GetControlMaps<Lista<ItemMoneda<string>>>(NombreComponente.Moneda);

                        if (fondoSeleccionado != null && ctrlMoneda != null)
                        {
                            var monedaEmision = fondoSeleccionado?.CodMonedaEmision;
                            var monedaSeleccionada = ctrlMoneda.Items.Where(x => x.CodigoIso == monedaEmision).FirstOrDefault();

                            if (monedaSeleccionada != null)
                            {
                                monedaSeleccionada.Seleccionado = true;
                                ctrlMoneda.Bloqueado = true;
                            }
                        }
                        break;
         
                    default:
                        break;
                }

            });
        }

        public static string ObtenerTituloSimulación(string idServicio)
        {
            string titulo = null;
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

            switch (idServicio.ToUpper())
            {
                case Servicio.SAF:

                    titulo = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                    {
                        CodigoSistema = Keys.CodigoSistemaMAPS,
                        NomParametro = Keys.TituloFormConfirmacionSAF
                    });

                    break;

                case Servicio.PoderDeCompra:

                    titulo = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                    {
                        CodigoSistema = Keys.CodigoSistemaMAPS,
                        NomParametro = Keys.TituloFormConfirmacionPDC
                    });

                    break;

                case Servicio.Agendamiento:

                    titulo = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                    {
                        CodigoSistema = Keys.CodigoSistemaMAPS,
                        NomParametro = Keys.TituloFormConfirmacionAGD
                    });

                    break;
                case Servicio.AgendamientoFH:

                    titulo = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                    {
                        CodigoSistema = Keys.CodigoSistemaMAPS,
                        NomParametro = Keys.TituloFormConfirmacionAGD
                    });

                    break;
                case Servicio.Rtf:

                    titulo = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                    {
                        CodigoSistema = Keys.CodigoSistemaMAPS,
                        NomParametro = Keys.TituloFormConfirmacionRTF
                    });

                    break;
                case Servicio.Repatriacion:

                    titulo = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                    {
                        CodigoSistema = Keys.CodigoSistemaMAPS,
                        NomParametro = Keys.TituloFormConfirmacionCTR
                    });

                    break;

                case Servicio.AltaCuenta:

                    titulo = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                    {
                        CodigoSistema = Keys.CodigoSistemaMAPS,
                        NomParametro = Keys.TituloFormConfirmacionACT
                    });

                    break;
                default:

                    titulo = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                    {
                        CodigoSistema = Keys.CodigoSistemaMAPS,
                        NomParametro = Keys.TituloFormConfirmacionDefault
                    });

                    break;
            }

            return titulo;
        }

        public static FormularioResponse ObtenerListaDeServicios(FormularioResponse _entity)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var valores = daMapsControles.ObtenerConfigDeFormulario(_entity, true);
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

            var form = _entity;


            if (valores != null && valores.Length > 0)
            {
                //recupera los atributos del formulario
                var frmAttr = valores.Where(x => !x.ControlPadreId.HasValue && !x.ControlAtributoPadreId.HasValue).ToArray();

                //recupera el id del padre (id de formulario)
                var frmPadreId = frmAttr.Select(x => x.IdComponente).FirstOrDefault();

                //recupera los ids de los diferentes controles del formulario                
                var listFrmCtrlID = valores.Where(x => x.ControlPadreId.HasValue)
                                            .Select(x => x.IdComponente)
                                            .Distinct();

                form.IdComponente = frmPadreId;

                #region Seteo propiedades del formulario
                for (int i = 0; i < frmAttr.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(frmAttr[i].AtributoDesc))
                    {
                        var propInfo = form.GetType().GetProperty(frmAttr[i].AtributoDesc, bindFlags);

                        if (propInfo != null && !propExcluidas.Contains(propInfo.Name.ToLower().Trim()))
                        {
                            if (frmAttr[i].AtributoDesc == "Ayuda")
                            {
                                if (string.IsNullOrEmpty(frmAttr[i].AtributoValor))
                                {
                                    propInfo.SetValue(form, frmAttr[i].ValorAtributoCompExtendido);
                                }
                                else
                                {
                                    propInfo.SetValue(form, frmAttr[i].AtributoValor);
                                }
                            }
                            else
                            {
                                propInfo.SetValue(form, frmAttr[i].AtributoValor);
                            }

                        }
                    }
                }
                #endregion

                foreach (decimal frmCtrlID in listFrmCtrlID)
                {
                    //recupera los atributos del control                    
                    var ctrlAtributosControl = valores.Where(x => x.IdComponente == frmCtrlID);

                    Type ctrlTipo = null;
                    Type itemGenericType = null;
                    bool tineValor = ctrlAtributosControl.Any(x => x.AtributoDesc.ToLower().Equals("valor"));

                    if (tineValor)
                    {
                        itemGenericType = ctrlAtributosControl.Where(x => x.AtributoDesc.ToLower().Equals("valor")).Select(y => y.AtributoDataType).FirstOrDefault().ToType();
                    }
                    else
                    {
                        itemGenericType = ctrlAtributosControl.Where(x => x.AtributoDesc.ToLower().Equals("tipo")).Select(y => y.AtributoDataType).FirstOrDefault().ToType();
                    }

                    ctrlTipo = ctrlAtributosControl.Where(x => x.AtributoDesc.ToLower().Equals("nombre")).FirstOrDefault().AtributoValor.ToControlMaps(itemGenericType);

                    if (ctrlTipo != null)
                    {
                        var itemControl = Activator.CreateInstance(ctrlTipo);

                        ((ControlSimple)itemControl).IdComponente = ctrlAtributosControl.First().IdComponente;
                        ((ControlSimple)itemControl).IdPadreDB = ctrlAtributosControl.First().ControlPadreId;

                        foreach (var atr in ctrlAtributosControl.ToList())
                        {
                            var propInfo = itemControl.GetType().GetProperty(atr.AtributoDesc, bindFlags);
                            if (propInfo != null && atr.AtributoValor != null)
                            {
                                propInfo.SetValue(itemControl, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);

                            }
                        }
                        if (form.Items == null)
                            form.Items = new List<ControlSimple>();
                        //agregar control al listado de items del formulario
                        form.Items.Add(itemControl as ControlSimple);
                    }
                }
            }

            CrearItems(form, null);

            return form;
        }

        public static string CrearFormulario(FormularioResponse entity, DatoFirmaMaps firma)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var valores = daMapsControles.ObtenerConfigDeFormulario(entity);
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

            var form = entity;

            if (valores != null && valores.Length > 0)
            {
                //recupera los atributos del formulario
                //ok
                var frmAttr = valores.Where(x => !x.ControlPadreId.HasValue && !x.ControlAtributoPadreId.HasValue).ToArray();

                //recupera el id del padre (id de formulario)
                var frmPadreId = frmAttr.Select(x => x.IdComponente).FirstOrDefault();

                form.IdComponente = frmPadreId;

                #region Seteo propiedades del formulario
                for (int i = 0; i < frmAttr.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(frmAttr[i].AtributoDesc))
                    {
                        var propInfo = form.GetType().GetProperty(frmAttr[i].AtributoDesc, bindFlags);

                        if (propInfo != null && !propExcluidas.Contains(propInfo.Name.ToLower().Trim()))
                            propInfo.SetValue(form, frmAttr[i].AtributoValor);
                    }
                }
                #endregion
                //recupera los ids de los diferentes controles del formulario                
                var listFrmCtrlID = valores.Where(x => x.ControlPadreId.HasValue)
                                            .Select(x => x.IdComponente)
                                            .Distinct();

                foreach (decimal frmCtrlID in listFrmCtrlID)
                {
                    //recupera los atributos del control                    
                    var ctrlAtributosControl = valores.Where(x => x.IdComponente == frmCtrlID);
                    //1-saber que control es
                    //2-llamar a la fabrica de componentes con el ID de componente
                    var nombreComponente = ctrlAtributosControl.Where(x => x.AtributoDesc.ToLower().Equals("nombre")).FirstOrDefault();
                    var componente = new FabricaComponente().Fabricar(nombreComponente.AtributoValor);
                    componente.IdComponente = ctrlAtributosControl.First().IdComponente;
                    componente.IdPadreDB = ctrlAtributosControl.First().ControlPadreId;

                    foreach (var atr in ctrlAtributosControl.ToList())
                    {
                        var propInfo = componente.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                        if (propInfo != null && atr.AtributoValor != null)
                        {
                            try
                            {
                                List<string> ListaFechas = new List<string>(new string[] { NombreComponente.FechaHasta, NombreComponente.FechaDesde, NombreComponente.FechaHastaSafBP, NombreComponente.FechaDesdeSafBP, NombreComponente.FechaVigenciaPDC, NombreComponente.FechaAltaPdcAdhesion, NombreComponente.Fecha, NombreComponente.FechaSafBP, NombreComponente.FechaBaja });
                                if (ListaFechas.Contains(atr.NombreComponente.Trim()))
                                {
                                    if (string.Compare(atr.AtributoValor, "today", true) == 0)
                                    {
                                        propInfo.SetValue(componente, DateTime.Now);
                                    }
                                    else if (string.Compare(atr.AtributoValor, "tomorrow") == 0)
                                    {
                                        propInfo.SetValue(componente, DateTime.Now.AddDays(1D));
                                    }
                                    else
                                        propInfo.SetValue(componente, atr.AtributoValor.ParseGenericVal(propInfo.PropertyType), null);
                                }
                                else
                                {
                                    propInfo.SetValue(componente, atr.AtributoValor.ParseGenericVal(propInfo.PropertyType), null);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new InvalidCastException($"Componente: {atr.NombreComponente}, atributo: {atr.AtributoDesc}: El valor {atr.AtributoValor} no se puede convertir a {atr.AtributoDataType}", ex);
                            }
                        }
                    }
                    var estrategia = new FabricaEstrategia().Fabricar(nombreComponente.AtributoValor, entity, componente, firma);
                    //agregar control al listado de items del formulario
                    form.Items.Add(estrategia);
                    //}
                }
            }

            var estadoFormulario = entity.Estado;

            #region Quitar cuando este el WIZARD
            if (estadoFormulario == TipoEstadoFormulario.Simulacion && entity.IdSimulacion != null)
            {
                //TODO: quitar esto para cuando este el form anterior y siguiente

                estadoFormulario = TipoEstadoFormulario.Confirmacion;
                entity.Estado = TipoEstadoFormulario.Confirmacion;
                entity.Id = "frm-confirmacion-1";
                entity.Nombre = "frm-confirmacion";
                entity.Bloqueado = true;
                entity.Validado = true;
            }
            #endregion

            return estadoFormulario;
        }
    }
}

