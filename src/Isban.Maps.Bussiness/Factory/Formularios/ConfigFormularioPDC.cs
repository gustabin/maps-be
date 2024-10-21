using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Bussiness.Factory.Formularios;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Enumeradores;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Compuestos;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Business.Formularios.Factory
{

    public class ConfigFormularioPDC : ICrearComponente
    {
        private static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        private static List<string> propExcluidas = new List<string> { "items", "nup", "segmento", "canal", "subcanal", "idsimulacion", "cabecera" };

        private FormularioResponse _entity;
        private DatoFirmaMaps _firma;
        private string estadoFormulario;
        private readonly ValorCtrlResponse[] _datosForm;

        public ConfigFormularioPDC(FormularioResponse formulario)
        {
            _entity = formulario;
        }
        public ConfigFormularioPDC(FormularioResponse formulario, DatoFirmaMaps firma)
        {
            _entity = formulario;
            _firma = firma;
        }             
        public ConfigFormularioPDC(FormularioResponse formulario, DatoFirmaMaps firma, ValorCtrlResponse[] datos)
        {
            _entity = formulario;
            _firma = firma;
            _datosForm = datos;

        }

        public void Crear()
        {
            //Limpio los items para no enviarlos nuevamente, ya que se guardo en otro paso.
            _entity.Items.Clear();

            CrearFormulario();        //ok

            switch (estadoFormulario)
            {
                case TipoEstadoFormulario.Simulacion:
                    Simular();
                    break;
                case TipoEstadoFormulario.Confirmacion:
                    Confirmar();
                    break;
                default:
                    CrearItems();
                    AsignarPadreAHijo();
                    PerfilDeInversor();
                    //if (_entity.IdServicio != null)
                    //{
                    //    ObtenerListaDeCuentasOperativas();
                    //    ObtenerListaDeCuentasTitulo();
                    //}
                    break;
            }

            _entity.Items = _entity.Items?.OrderBy(x => (x as ControlSimple).Posicion).ToList();
        }

        private void AsignarPadreAHijo()
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


        private void Confirmar()
        {
            var daConf = DependencyFactory.Resolve<IMapsControlesDA>();
            var itemsConf = daConf.ObtenerDatosDeConfirmacion(_entity);

            _entity.Items.AddRange(ObtenerItemsDeFormulario(itemsConf));

            #region PDC 
            var consultaOrigen = daConf.ConsultaOrigen(_entity); // ---> Devuelve el id de simulacion de PDC               
            var altaPDC = BusinessHelper.SimularAltaBajaAdhesionPDC(_entity, _firma, CuentaPDC.Procesar, CuentaPDC.Alta, consultaOrigen.Origen);
            #endregion

            if (altaPDC.FechaEfectiva.HasValue)
            {
                _entity.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaDesde).Valor = altaPDC.FechaEfectiva.Value.Date;
            }

            #region Maps          
            _entity.Bloqueado = true;
            _entity.Validado = true;
            _entity.OrdenOrigen = altaPDC.IDCuentaPDC;
            _entity.Comprobante = daConf.ConfirmarAdhesion(_entity, null).ToString();
            _entity.IdAdhesion = _entity.Comprobante.ParseGenericVal<long?>();
            //se reconfirma para que se actualice el ID de comprobante en el json que se guarda.
            daConf.ActualizarComprobanteAJson(_entity);
            //Eliminación del primer legal
            var componente = _entity.Items.Where(x =>
                        string.Compare((x as ControlSimple).Config, NombreComponente.PrimerLegalPDC, true) == 0)
                        .ToList();

            if (componente.Count > 0) //Existe
            {
                _entity.Items.Remove(_entity.Items.Where(y => (y as ControlSimple).Config == NombreComponente.PrimerLegalPDC).FirstOrDefault());
            }
            #endregion
            /*
            #region Registrar Orden
            var registraOrdenRequest = new RegistraOrdenRequest();

            registraOrdenRequest.CodEstadoProceso = TipoEstado.Procesado;
            registraOrdenRequest.IdSimulacion = _entity.IdSimulacion.Value;
            registraOrdenRequest.Ip = _entity.Ip;
            registraOrdenRequest.Usuario = _entity.Usuario;
            registraOrdenRequest.IdServicio = _entity.IdServicio;

            daConf.RegistrarOrden(registraOrdenRequest);
            #endregion*/
        }

        private void Simular()
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var items = daMapsControles.ObtenerDatosDeSimulacion(_entity);
            //obtener los items de los formulario y agregarlos al de simulacion
            _entity.Items.Clear();
            _entity.Items.AddRange(ObtenerItemsDeFormulario(items));

            #region PDC
            var simulacionPDC = BusinessHelper.SimularAltaBajaAdhesionPDC(_entity, _firma, CuentaPDC.Simular, CuentaPDC.Alta);
            #endregion

            if (!simulacionPDC.IDSimCuentaPDC.HasValue)
            {
                _entity.Error = (int)TiposDeError.ErrorSimulacionAlta;
                _entity.Error_desc = "No se puede simular adhesión en PDC";
            }
            else
            {
                if (simulacionPDC.FechaEfectiva.HasValue)
                {
                    _entity.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaDesde).Valor = simulacionPDC.FechaEfectiva.Value.Date;
                }

                #region Maps
                _entity.SimOrdenOrigen = simulacionPDC.IDSimCuentaPDC;
                _entity.IdSimulacion = daMapsControles.SimularAdhesion(_entity).ParseGenericVal<long?>();
                #endregion

                if (!_entity.IdSimulacion.HasValue)
                {
                    _entity.Error = (int)TiposDeError.ErrorSimulacionAlta;
                    _entity.Error_desc = "No se puede simular adhesión";
                }
            }

            _entity.Bloqueado = true;
            _entity.Validado = true;
        }

        /// <summary>
        /// Devuelve de un formulario la lista de items del mismo.
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        private IEnumerable<ControlSimple> ObtenerItemsDeFormulario(List<ConsultarDatosSimulacionConfirmacionResp> forms)
        {
            List<ControlSimple> resItems = new List<ControlSimple>();

            foreach (var form in forms)
            {
                var mapsForm = form.TextoJson.DeserializarToJson<FormularioRequest>().ToFormularioMaps();

                resItems.AddRange(mapsForm.Items);
            }

            return resItems;
        }

        private void CrearFormulario()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var valores = daMapsControles.ObtenerConfigDeFormulario(_entity);

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
                                propInfo.SetValue(form, frmAttr[i].ValorAtributoCompExtendido);
                            else
                                propInfo.SetValue(form, frmAttr[i].AtributoValor);
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
                                try
                                {
                                    List<string> ListaFechas = new List<string>(new string[] { NombreComponente.FechaHasta, NombreComponente.FechaDesde, NombreComponente.FechaHastaSafBP, NombreComponente.FechaDesdeSafBP, NombreComponente.FechaVigenciaPDC, NombreComponente.FechaAltaPdcAdhesion, NombreComponente.Fecha, NombreComponente.FechaSafBP, NombreComponente.FechaBaja });
                                    if (ListaFechas.Contains(atr.NombreComponente.Trim()))
                                    {
                                        if (atr.AtributoValor.ToLower().Equals("today"))
                                        {
                                            propInfo.SetValue(itemControl, DateTime.Now);
                                        }
                                        else if (atr.AtributoValor.ToLower().Equals("tomorrow"))
                                        {
                                            propInfo.SetValue(itemControl, DateTime.Now.AddDays(1D));
                                        }
                                        else
                                            propInfo.SetValue(itemControl, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(itemControl, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new InvalidCastException($"Componente: {atr.NombreComponente}, atributo: {atr.AtributoDesc}: El valor {atr.AtributoValor} no se puede convertir a {atr.AtributoDataType}", ex);
                                }
                            }
                        }

                        //agregar control al listado de items del formulario
                        form.Items.Add(itemControl as ControlSimple);
                    }
                }
            }

            estadoFormulario = _entity.Estado;

            #region Quitar cuando este el WIZARD
            if (estadoFormulario == TipoEstadoFormulario.Simulacion && _entity.IdSimulacion != null)
            {
                //TODO: quitar esto para cuando este el form anterior y siguiente

                estadoFormulario = TipoEstadoFormulario.Confirmacion;
                _entity.Estado = TipoEstadoFormulario.Confirmacion;
                _entity.Id = "frm-confirmacion-1";
                _entity.Nombre = "frm-confirmacion";
                _entity.Bloqueado = true;
                _entity.Validado = true;
            }
            #endregion
        }

        private void CrearItems()
        {
            FormularioHelper.CrearItems(_entity, _firma);          
        }

        private void PerfilDeInversor()
        {
            //IServicesClient srvClient = DependencyFactory.Resolve<IServicesClient>();
            //_entity.PerfilInversor = srvClient.GetPerfil(_entity.Nup, _firma);


            var serviceWebApi = DependencyFactory.Resolve<IServiceWebApiClient>();

            var consultaPerfilReq = new ConsultaPerfilInversorRequest()
            {
                Nup = _entity.Nup,
                Encabezado = BusinessHelper.GenerarCabecera(_entity.Canal, _entity.SubCanal)
            };
            var reqSecurity = _firma.MapperClass<RequestSecurity<ConsultaPerfilInversorRequest>>(TypeMapper.IgnoreCaseSensitive);
            reqSecurity.Datos = consultaPerfilReq;
            _entity.PerfilInversor = serviceWebApi.ConsultaPerfilInversor(reqSecurity).Descripcion;
        }

        private void ObtenerListaDeCuentasTitulo()
        {
            var ListaCtasTit = BusinessHelper.ObtenerCuentasPorTipo(_entity, "TI", _firma, false);
            if (ListaCtasTit.Length > 0)
            {
                var CtasTitSinBloq = BusinessHelper.ValidarCuentasBloquedas(ListaCtasTit);
                if (CtasTitSinBloq.Length > 0)
                    _entity.ListaCtasTitulo = string.Join(",", CtasTitSinBloq.Select(x => x.NroCta));
            }
        }

        //private void ObtenerListaDeCuentasOperativas()
        //{
        //    var ListaCtasOper = BusinessHelper.ObtenerCuentasPorTipo(_entity, "OP", _firma, true);

        //    if (ListaCtasOper.Length > 0)
        //    {
        //        var CtasOperatSinBloq = BusinessHelper.ValidarCuentasBloquedas(ListaCtasOper);
        //        if (CtasOperatSinBloq.Length > 0)
        //            _entity.ListaCtasOperativas = string.Join(",", CtasOperatSinBloq.Select(x => x.SucursalCta + "|" + x.NroCta + "|" + x.CodigoMoneda));
        //    }
        //}
    }
}
