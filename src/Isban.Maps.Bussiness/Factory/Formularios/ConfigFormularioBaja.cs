using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
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
    public class ConfigFormularioBaja : ICrearComponente
    {
        private static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        private static List<string> propExcluidas = new List<string> { "items", "nup", "segmento", "canal", "subcanal", "idsimulacion", "cabecera" };

        private FormularioResponse _entity;
        private DatoFirmaMaps _firma;
        private char _estadoAdhesion;

        public ConfigFormularioBaja(FormularioResponse entity, DatoFirmaMaps firma)
        {
            _entity = entity;
            _firma = firma;
        }

        public void Crear()
        {
            if (_entity.Estado == TipoEstadoFormulario.Simulacion
                && (_entity.IdServicio == null || _entity.IdServicio != null)
                && _entity.IdSimulacion == null)
            {
                CrearFormulario();
                CrearItems();
                Simular();
                PerfilDeInversor();
            }
            else
            {
                CrearFormulario();
                CrearItems();
                PerfilDeInversor();
                Confirmar();
            }
            #region Como deberia ser
            //switch (_entity.Estado)
            //{
            //    case TipoEstadoFormulario.Simulacion:
            //        Confirmar();
            //        break;
            //    default:
            //        CrearFormulario();
            //        CrearItems();
            //        Simular();
            //        PerfilDeInversor();
            //        break;
            //} 
            #endregion

            AsignarTitulo();  //En confirmacion o simulacion se debe mostrar el titulo, ver en pruebas si, dejando dentro de 'DEFAULT' basta.

            _entity.Items = _entity.Items?.OrderBy(x => (x as ControlSimple).Posicion).ToList();
        }

        /// <summary>
        /// Realiza la confirmación de la baja de MAPS
        /// </summary>
        private void Confirmar()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var orden = new ConsultaOrigenResponse();
            var bajaPDC = new SimulaPdcResponse();

            orden = daMapsControles.ConsultaOrigen(_entity); // ----> IDSimulacion Baja PDC

            #region PDC
            if (_entity.IdServicio.ToUpper() == Servicio.PoderDeCompra)
            {
                bajaPDC = BusinessHelper.SimularAltaBajaAdhesionPDC(_entity, _firma, CuentaPDC.Procesar, CuentaPDC.Baja, orden.Origen);
                if (bajaPDC.FechaEfectiva.HasValue)
                {
                    _entity.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta).Valor = bajaPDC.FechaEfectiva.Value.Date;
                }
                _entity.FechaEfectiva = bajaPDC?.FechaEfectiva.Value.Date; // ------> Si es PDC
                _entity.OrdenOrigen = bajaPDC.IDCuentaPDC;   // ------> Si es PDC
            }
            #endregion

            #region Maps Baja primera parte
            _entity.Comprobante = daMapsControles.BajaAdhesion(_entity)?.ToString(); // Se da de baja
            #endregion

            //Se actualiza los campos necesarios
            _entity.Estado = TipoEstadoFormulario.Confirmacion;

            #region Maps Baja segunda parte
            _entity.TextoJson = _entity.SerializarToJson(); //-----> Actualiza Json
            daMapsControles.BajaAdhesion(_entity);
            #endregion

            #region Registrar Orden
            if (_entity.IdServicio.ToUpper() != Servicio.PoderDeCompra)
            {
                var registraOrdenRequest = new RegistraOrdenRequest();

                registraOrdenRequest.CodEstadoProceso = TipoEstado.BajaOrden;
                registraOrdenRequest.IdSimulacion = _entity.IdSimulacion.Value;
                registraOrdenRequest.Ip = _entity.Ip;
                registraOrdenRequest.Usuario = _entity.Usuario;
                registraOrdenRequest.IdServicio = _entity.IdServicio;

                daMapsControles.RegistrarOrden(registraOrdenRequest);
            }
            #endregion
        }

        /// <summary>
        /// Realiza la simulación de la baja de MAPS
        /// </summary>
        private void Simular()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            SimulaPdcResponse simulacionPDC = null;

            if (_estadoAdhesion == 'A')
            {
                // ---> En caso que se trate PDC devolvera Datos 
                #region Consulta Orden 
                var consultaOrigen = daMapsControles.ConsultaOrigen(_entity);  // IDAlta de PDC
                #endregion

                #region PDC
                if (_entity.IdServicio.ToUpper() == Servicio.PoderDeCompra)
                {
                    simulacionPDC = BusinessHelper.SimularAltaBajaAdhesionPDC(_entity, _firma, CuentaPDC.Simular, CuentaPDC.Baja, consultaOrigen.Origen);

                    if (simulacionPDC.FechaEfectiva.HasValue)
                    {
                        _entity.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta).Valor = simulacionPDC.FechaEfectiva.Value.Date;
                    }
                    _entity.SimOrdenOrigen = simulacionPDC.IDSimCuentaPDC; // ----> Si es PDC
                }
                #endregion

                #region Maps
                var idSimulacion = daMapsControles.SimulacionBajaAdhesion(_entity);
                _entity.Estado = TipoEstadoFormulario.Simulacion;
                #endregion

                if ((_entity.IdServicio.ToUpper() == Servicio.PoderDeCompra && simulacionPDC.IDSimCuentaPDC.HasValue && idSimulacion.HasValue)
                    || idSimulacion.HasValue)
                {
                    _entity.IdSimulacion = idSimulacion.ParseGenericVal<long?>();
                }
            }
        }

        private void CrearItems()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            //obtener la adhesion con la consulta de adhesiones.
            var valoresAdhesion = daMapsControles.ObtenerConsultaDeAdhesiones(_entity).FirstOrDefault();

            if (valoresAdhesion != null && !string.IsNullOrWhiteSpace(valoresAdhesion.TextoJson))
            {
                var frm = valoresAdhesion.TextoJson.DeserializarToJson<FormularioRequest>().ToFormularioMaps();

                _estadoAdhesion = valoresAdhesion.Estado;
                _entity.IdAdhesion = frm.IdAdhesion;
                _entity.Segmento = frm.Segmento;
                _entity.IdServicio = frm.IdServicio;
                _entity.Nup = frm.Nup;
                _entity.Canal = frm.Canal;
                _entity.SubCanal = frm.SubCanal;
                _entity.Ip = frm.Ip;
                _entity.Usuario = frm.Usuario;

                if (_entity.IdServicio.ToUpper() != Servicio.PoderDeCompra)
                {
                    _entity.Items.AddRange(frm.Items);
                }
                else
                {
                    for (int i = 0; i < frm.Items.Count; i++)
                    {
                        if (frm.Items[i].Nombre.ToLower() != NombreComponente.Legal)
                            _entity.Items.Add(frm.Items[i]);
                    }
                }

                #region Agregar legal baja PDC
                if (_entity.IdServicio.ToUpper() == Servicio.PoderDeCompra)
                {
                    var idLegal = daMapsControles.ObtenerIdComponente(NombreComponente.LegalBajaPDC, _entity.Usuario, _entity.Ip);

                    if (!BusinessHelper.ValidarExistencia(NombreComponente.Legal, _entity))
                    {
                        _entity.Items.Add(BusinessHelper.AgregarComponenteLegal(_entity, idLegal, _entity.IdServicio));
                    }
                }
                #endregion
            }
        }

        private IEnumerable<ControlSimple> ObtenerItemsDeFormulario(ValorConsDeAdhesionesResp[] forms)
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

                _entity.IdComponente = frmPadreId;

                #region Seteo propiedades del formulario
                for (int i = 0; i < frmAttr.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(frmAttr[i].AtributoDesc))
                    {
                        var propInfo = _entity.GetType().GetProperty(frmAttr[i].AtributoDesc, bindFlags);

                        if (propInfo != null && !propExcluidas.Contains(propInfo.Name.ToLower().Trim()))
                            propInfo.SetValue(_entity, frmAttr[i].AtributoValor);
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
                                    /*List<string> ListaFechas = new List<string>(new string[] { NombreComponente.FechaHasta, NombreComponente.FechaDesde, NombreComponente.FechaHastaSafBP, NombreComponente.FechaDesdeSafBP, NombreComponente.FechaVigenciaPDC, NombreComponente.FechaAltaPdcAdhesion, NombreComponente.Fecha, NombreComponente.FechaSafBP, NombreComponente.FechaBaja });
                                    if (ListaFechas.Contains(atr.NombreComponente.Trim()))
                                    {*/
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
                                    /*}
                                    else
                                    {
                                        propInfo.SetValue(itemControl, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);
                                    }*/
                                }
                                catch (Exception ex)
                                {
                                    throw new InvalidCastException($"Componente: {atr.NombreComponente}, atributo: {atr.AtributoDesc}: El valor {atr.AtributoValor} no se puede convertir a {atr.AtributoDataType}", ex);
                                }
                            }
                        }

                        //agregar control al listado de items del formulario
                        _entity.Items.Add(itemControl as ControlSimple);
                    }
                }
            }
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

        private void AsignarTitulo()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            if (string.IsNullOrWhiteSpace(_entity.IdServicio))
            {
                var req = new ConsultaParametrizacionReq()
                {
                    CodigoSistema = "MAPS",
                    NomParametro = "TituloFormDefault"
                };
                _entity.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Suscripción";
            }
            else if (!string.IsNullOrWhiteSpace(_entity.IdServicio))
            {
                switch (_entity.IdServicio.ToUpper())
                {
                    case Servicio.SAF:
                        var req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormBajaSAF"
                        };
                        _entity.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Suscripción Automática de Fondos";
                        break;
                    case Servicio.PoderDeCompra:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormBajaPDC"
                        };
                        _entity.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Poder de Compra";
                        break;
                    case Servicio.Agendamiento:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormBajaAGD"
                        };
                        _entity.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                        break;
                    case Servicio.AgendamientoFH:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormBajaAGD"
                        };
                        _entity.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                        break;
                    default:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormDefault"
                        };
                        _entity.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Suscripción";
                        break;
                }
            }
        }
    }
}
