
namespace Isban.Maps.Bussiness
{
    using Entity.Base;
    using Entity.Constantes.Estructuras;
    using Entity.Controles;
    using Entity.Extensiones;
    using Entity.Response;
    using Factory.Formularios;
    using IBussiness;
    using IDataAccess;
    using Isban.Maps.Bussiness.Factory;
    using Isban.Maps.Entity;
    using Isban.Maps.Entity.Constantes.Enumeradores;
    using Isban.Maps.Entity.Controles.Compuestos;
    using Isban.Maps.Entity.Controles.Customizados;
    using Isban.Maps.Entity.Request;
    using Isban.Mercados;
    using Isban.Mercados.Extensions;
    using Isban.Mercados.LogTrace;
    using Isban.Mercados.Service.InOut;
    using Mercados.UnityInject;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class MapsServiciosBusiness : IMapsServiciosBusiness
    {
        Dictionary<string, string> ValoresParametrizados;
        EstadoAdhesion<string> ComponenteEstado;
        SeleccionVigencia ComponenteFechaCompuesta;
        Dictionary<string,ControlSimple> ComponenteDescripcion;

        /// <summary>
        /// Servicio que consulta las adhesiones relacionas al nup
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="firma"></param>
        /// <returns></returns>
        public virtual FormularioResponse ConsultaAdhesion(FormularioResponse entity, DatoFirmaMaps firma)
        {
            ValoresParametrizados = new Dictionary<string, string>();
            ComponenteDescripcion = new Dictionary<string, ControlSimple>();

            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            IServicesClient srvClient = DependencyFactory.Resolve<IServicesClient>();
            var serviceWebApi = DependencyFactory.Resolve<IServiceWebApiClient>();
            entity.Estado = TipoEstado.ConsultaAdhesion;

            daMapsControles.LogFormulario(entity);

            var frmValores = daMapsControles.ObtenerConfigDeFormulario(entity);

            //Obtencion de cuentas operativas relacionadas al nup
            var cuentasOperativasLista = BusinessHelper.ObtenerCuentasPorTipo(entity, "OP", firma, false);
            var cuentasOperativas = BusinessHelper.ConcatenarCuentas(cuentasOperativasLista, true);

            //Obtencion de cuentas titulos relacionadas al nup
            var cuentasTitulosLista = BusinessHelper.ObtenerCuentasPorTipo(entity, "TI", firma, false);
            var cuentasTitulos = BusinessHelper.ConcatenarCuentas(cuentasTitulosLista, false);

            var adhesiones = daMapsControles.ObtenerConsultaDeAdhesiones(entity, cuentasOperativas, cuentasTitulos);

            var frm = new FormularioResponse();

            frm.AsignarDatosBackend(frmValores, entity.IdServicio);

            var idDescripcionDinamica = daMapsControles.ObtenerIdComponente(NombreComponente.DescripcionDinamica, entity.Usuario, entity.Ip);         

            frm.Items.ForEach(c =>
            {
                var componente = c as ControlSimple;

                if (componente.Nombre.ToLower() == NombreComponente.ConsultaAdhesiones)
                {
                    var adh = c as ConsultaAdhesiones;

                    Array.ForEach(adhesiones, t =>
                     {
                         string Estado = (char.ToUpper(t.Estado) == 'A') ? TipoEstado.Activo : TipoEstado.Inactivo;
                         var fa = t.TextoJson.DeserializarToJson<FormularioRequest>().ToFormularioMaps();
                         switch (fa.IdServicio.ToUpper())
                         {
                             case Servicio.SAF:
                                 if (char.ToUpper(t.Estado) == 'A')
                                 {
                                     fa.Titulo = ObtenerValorParametrizado(new ConsultaParametrizacionReq
                                     {
                                         NomParametro = Keys.ConsultaSAFAlta
                                     });
                                 }
                                 else if (char.ToUpper(t.Estado) == 'B')
                                 {
                                     var req = new ConsultaParametrizacionReq()
                                     {
                                         CodigoSistema = "MAPS",
                                         NomParametro = "TituloFormBajaSAF"
                                     };
                                     fa.Titulo = ObtenerValorParametrizado(req);
                                 }
                                 break;
                             case Servicio.PoderDeCompra:
                                 if (char.ToUpper(t.Estado) == 'A')
                                 {
                                     fa.Titulo = ObtenerValorParametrizado(new ConsultaParametrizacionReq
                                     {
                                         NomParametro = Keys.ConsultaPDCAlta
                                     });
                                 }
                                 else if (char.ToUpper(t.Estado) == 'B')
                                 {
                                     var req = new ConsultaParametrizacionReq()
                                     {
                                         CodigoSistema = "MAPS",
                                         NomParametro = "TituloFormBajaPDC"
                                     };
                                     fa.Titulo = ObtenerValorParametrizado(req);
                                 }
                                 break;
                             case Servicio.Agendamiento:
                                 if (char.ToUpper(t.Estado) == 'A')
                                 {
                                     fa.Titulo = ObtenerValorParametrizado(new ConsultaParametrizacionReq
                                     {
                                         NomParametro = Keys.ConsultaAGDAlta
                                     });
                                 }
                                 else if (char.ToUpper(t.Estado) == 'B')
                                 {
                                     var req = new ConsultaParametrizacionReq()
                                     {
                                         CodigoSistema = "MAPS",
                                         NomParametro = "TituloFormBajaAGD"
                                     };
                                     fa.Titulo = ObtenerValorParametrizado(req);
                                 }
                                 break;
                             case Servicio.AgendamientoFH:
                                 if (char.ToUpper(t.Estado) == 'A')
                                 {
                                     fa.Titulo = ObtenerValorParametrizado(new ConsultaParametrizacionReq
                                     {
                                         NomParametro = Keys.ConsultaAGDAlta
                                     });
                                 }
                                 else if (char.ToUpper(t.Estado) == 'B')
                                 {
                                     var req = new ConsultaParametrizacionReq()
                                     {
                                         CodigoSistema = "MAPS",
                                         NomParametro = "TituloFormBajaAGD"
                                     };
                                     fa.Titulo = ObtenerValorParametrizado(req);
                                 }
                                 break;
                             case Servicio.Rtf:
                                 if (char.ToUpper(t.Estado) == 'A')
                                 {
                                     fa.Titulo = ObtenerValorParametrizado(new ConsultaParametrizacionReq
                                     {
                                         NomParametro = Keys.DetalleConsultaRTF
                                     });
                                 }
                                 else if (char.ToUpper(t.Estado) == 'B')
                                 {
                                     var req = new ConsultaParametrizacionReq()
                                     {
                                         CodigoSistema = Keys.CodigoSistemaMAPS,
                                         NomParametro = Keys.TituloFormBajaRTF
                                     };
                                     fa.Titulo = ObtenerValorParametrizado(req);
                                 }
                                 break;
                             default:
                                 if (char.ToUpper(t.Estado) == 'A')
                                 {
                                     fa.Titulo = ObtenerValorParametrizado(new ConsultaParametrizacionReq
                                     {
                                         NomParametro = Keys.ConsultaDefaultAlta
                                     });
                                 }
                                 else if (char.ToUpper(t.Estado) == 'B')
                                 {
                                     var req = new ConsultaParametrizacionReq()
                                     {
                                         CodigoSistema = "MAPS",
                                         NomParametro = "TituloFormDefault"
                                     };
                                     fa.Titulo = ObtenerValorParametrizado(req);
                                 }
                                 break;
                         }

                         if (BusinessHelper.ValidarExistencia(NombreComponente.Email, fa))
                         {
                             fa.Items
                             .Where(x => string.Compare((x as ControlSimple).Nombre, NombreComponente.Email, true) == 0)
                             .FirstOrDefault()
                             .Ayuda = ObtenerValorParametrizado(
                                                 new ConsultaParametrizacionReq
                                                 {
                                                     NomParametro = Keys.AyudaMail
                                                 });

                             BusinessHelper.ReemplazarControlEmail(fa);

                         }

                         if (!BusinessHelper.ValidarExistencia(NombreComponente.DescripcionDinamica, fa))
                         {
                             fa.Items.Add(ObtenerComponenteDescripcion(entity,idDescripcionDinamica,fa.IdServicio,daMapsControles));
                         }

                         if (!BusinessHelper.ValidarExistencia(NombreComponente.EstadoAdhesion, fa))
                         {
                        
                             fa.Items.Add(ObtenerComponenteEstado(entity,daMapsControles,Estado));
                         }

                         if (fa.IdServicio.ToUpper() == Servicio.AgendamientoFH && !BusinessHelper.ValidarExistencia(NombreComponente.Vigencia, fa))
                         {
                      
                             fa.Items.Add(ObtenerComponenteVigencia(entity,daMapsControles,fa));
                         }

                         fa.Items = fa.Items.OrderBy(x => (x as ControlSimple).Posicion).ToList();

                         if (char.ToUpper(t.Estado) == 'A')
                         {
                             adh.Activas.Add(fa);
                         }
                         else if (char.ToUpper(t.Estado) == 'B')
                         {
                             adh.Inactivas.Add(fa);
                         }


                     });
                }

            });

            //Obtengo el perfil inversor actual
            var consultaPerfilReq = new ConsultaPerfilInversorRequest() {
                Nup = entity.Nup,
                Encabezado = BusinessHelper.GenerarCabecera(entity.Canal, entity.SubCanal)
            };
            var reqSecurity = firma.MapperClass<RequestSecurity<ConsultaPerfilInversorRequest>>(TypeMapper.IgnoreCaseSensitive);
            reqSecurity.Datos = consultaPerfilReq;
            frm.PerfilInversor = serviceWebApi.ConsultaPerfilInversor(reqSecurity).Descripcion;
            daMapsControles.LogFormulario(frm);
            return frm;
        }

        /// <summary>
        /// Servicio que realiza la baja de una adhesion
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="firma"></param>
        /// <returns></returns>
        public virtual FormularioResponse BajaAdhesion(FormularioResponse entity, DatoFirmaMaps firma)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            long? idBaja = null;
            var frm = new FormularioResponse();
            var formularioBaja = new FormularioResponse();
            entity.Estado = TipoEstadoFormulario.Baja;

            daMapsControles.LogFormulario(entity, entity.IdSimulacion, entity.IdAdhesion, null);

            #region ConstruirFormulario
            var frmValues = daMapsControles.ObtenerConfigDeFormulario(entity);
            //Busca adhesion q se quiere dar de baja
            var adhesion = daMapsControles.ObtenerConsultaDeAdhesiones(entity).FirstOrDefault();
            formularioBaja.AsignarDatosBackend(frmValues, entity.IdServicio);
            #endregion

            #region No existe formulario
            if (adhesion == null || (adhesion != null && string.IsNullOrWhiteSpace(adhesion.TextoJson)))
            {
                formularioBaja.Error = (int)TiposDeError.NoSeEncontraronDatos;
                formularioBaja.Error_desc = "Adhesion inexistente.";
                formularioBaja.Estado = TipoEstadoFormulario.Simulacion;
                formularioBaja.Id = "frm-simulacion-1";
                formularioBaja.Nombre = "frm-simulacion";
                formularioBaja.FormularioId = null;
                formularioBaja.Items = null;

                #region Titulo
                if (string.IsNullOrWhiteSpace(entity.IdServicio))
                {
                    var req = new ConsultaParametrizacionReq()
                    {
                        CodigoSistema = "MAPS",
                        NomParametro = "TituloFormDefault"
                    };
                    formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Suscripción";
                }
                else if (!string.IsNullOrWhiteSpace(entity.IdServicio))
                {
                    switch (entity.IdServicio.ToUpper())
                    {
                        case Servicio.SAF:
                            var req = new ConsultaParametrizacionReq()
                            {
                                CodigoSistema = "MAPS",
                                NomParametro = "TituloFormBajaSAF"
                            };
                            formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Suscripción Automática de Fondos";
                            break;
                        case Servicio.PoderDeCompra:
                            req = new ConsultaParametrizacionReq()
                            {
                                CodigoSistema = "MAPS",
                                NomParametro = "TituloFormBajaPDC"
                            };
                            formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                            break;
                        case Servicio.Agendamiento:
                            req = new ConsultaParametrizacionReq()
                            {
                                CodigoSistema = "MAPS",
                                NomParametro = "TituloFormBajaAGD"
                            };
                            formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                            break;
                        case Servicio.AgendamientoFH:
                            req = new ConsultaParametrizacionReq()
                            {
                                CodigoSistema = "MAPS",
                                NomParametro = "TituloFormBajaAGD"
                            };
                            formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                            break;
                        case Servicio.Rtf:
                            req = new ConsultaParametrizacionReq()
                            {
                                CodigoSistema = Keys.TituloFormBajaRTF,
                                NomParametro = Keys.TituloFormBajaRTF
                            };
                            formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                            break;
                        default:
                            req = new ConsultaParametrizacionReq()
                            {
                                CodigoSistema = "MAPS",
                                NomParametro = "TituloFormDefault"
                            };
                            formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Suscripción";
                            break;
                    }
                }
                #endregion
            }
            #endregion
            else
            {
                #region Completar el formulario de baja
                frm = adhesion.TextoJson.DeserializarToJson<FormularioRequest>().ToFormularioMaps();

                formularioBaja.IdAdhesion = frm.IdAdhesion;
                formularioBaja.Segmento = frm.Segmento;
                formularioBaja.IdServicio = frm.IdServicio;
                formularioBaja.Nup = frm.Nup;
                formularioBaja.Canal = frm.Canal;
                formularioBaja.SubCanal = frm.SubCanal;
                formularioBaja.Ip = frm.Ip;
                formularioBaja.Usuario = frm.Usuario;

                if (formularioBaja.IdServicio.ToUpper() != Servicio.PoderDeCompra)
                {
                    formularioBaja.Items.AddRange(frm.Items);
                }
                else
                {
                    for (int i = 0; i < frm.Items.Count; i++)
                    {
                        if (frm.Items[i].Nombre.ToLower() != NombreComponente.Legal)
                            formularioBaja.Items.Add(frm.Items[i]);
                    }
                }

                #region Agregar legal baja PDC
                if (formularioBaja.IdServicio.ToUpper() == Servicio.PoderDeCompra)
                {
                    var idLegal = daMapsControles.ObtenerIdComponente(NombreComponente.LegalBajaPDC, entity.Usuario, entity.Ip);

                    if (!BusinessHelper.ValidarExistencia(NombreComponente.Legal, formularioBaja))
                    {
                        formularioBaja.Items.Add(BusinessHelper.AgregarComponenteLegal(entity, idLegal, formularioBaja.IdServicio));
                    }
                }
                #endregion
                #endregion

                #region Titulo
                switch (formularioBaja.IdServicio.ToUpper())
                {
                    case Servicio.SAF:
                        var req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormBajaSAF"
                        };
                        formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Suscripción Automática de Fondos";
                        break;
                    case Servicio.PoderDeCompra:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormBajaPDC"
                        };
                        formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                        break;
                    case Servicio.Agendamiento:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormBajaAGD"
                        };
                        formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                        break;

                    case Servicio.AgendamientoFH:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormBajaAGD"
                        };
                        formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                        break;

                    case Servicio.Rtf:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = Keys.CodigoSistemaMAPS,
                            NomParametro = Keys.TituloFormBajaRTF
                        };
                        formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); //"Baja de Poder de Compra";
                        break;
                    default:
                        req = new ConsultaParametrizacionReq()
                        {
                            CodigoSistema = "MAPS",
                            NomParametro = "TituloFormDefault"
                        };
                        formularioBaja.Titulo = daMapsControles.ObtenerValorParametrizado(req); // "Baja de Suscripción";
                        break;
                }
                #endregion

                #region simulacion
                if (entity.IdSimulacion == null)
                {
                    if (char.ToUpper(adhesion.Estado) == 'A')
                    {
                        var consultaOrigen = new ConsultaOrigenResponse();
                        var simulacionPDC = new SimulaPdcResponse();

                        // ---> En caso que se trate PDC devolvera Datos 
                        #region Consulta Orden 
                        consultaOrigen = daMapsControles.ConsultaOrigen(entity);  // IDAlta de PDC
                        #endregion

                        #region PDC
                        if (formularioBaja.IdServicio.ToUpper() == Servicio.PoderDeCompra)
                        {
                            simulacionPDC = BusinessHelper.SimularAltaBajaAdhesionPDC(entity, firma, CuentaPDC.Simular, CuentaPDC.Baja, consultaOrigen.Origen);

                            if (simulacionPDC.FechaEfectiva.HasValue)
                            {
                                frm.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta).Valor = simulacionPDC.FechaEfectiva.Value.Date;
                            }
                            entity.SimOrdenOrigen = simulacionPDC.IDSimCuentaPDC; // ----> Si es PDC
                        }
                        #endregion

                        #region Maps
                        var idSimulacion = daMapsControles.SimulacionBajaAdhesion(entity);
                        #endregion

                        if ((formularioBaja.IdServicio.ToUpper() == Servicio.PoderDeCompra && simulacionPDC.IDSimCuentaPDC.HasValue && idSimulacion.HasValue)
                            || idSimulacion.HasValue)
                        {
                            formularioBaja.IdSimulacion = idSimulacion.ParseGenericVal<long?>();
                            if (string.Compare(frm.IdServicio, Servicio.Rtf, true) == 0)
                                frm.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta).Valor = DateTime.Now;

                            formularioBaja.Estado = TipoEstadoFormulario.Simulacion;
                            formularioBaja.Id = "frm-simulacion-1";
                            formularioBaja.Nombre = "frm-simulacion";
                            formularioBaja.FormularioId = null;
                        }
                        else
                        {
                            formularioBaja.Error = (int)TiposDeError.ErrorSimulacionBaja;
                            formularioBaja.Error_desc = "No se puede simular la baja de la adhesion.";
                            formularioBaja.Estado = TipoEstadoFormulario.Simulacion;
                            formularioBaja.Id = "frm-simulacion-1";
                            formularioBaja.Nombre = "frm-simulacion";
                            formularioBaja.FormularioId = null;
                        }
                    }
                    else //Esta en estabo = 'Inactivo'
                    {
                        formularioBaja.Error = (int)TiposDeError.ErrorValidacion;
                        formularioBaja.Error_desc = "Adhesion se encuentra dado de baja.";
                        formularioBaja.Estado = TipoEstadoFormulario.Simulacion;
                        formularioBaja.Id = "frm-simulacion-1";
                        formularioBaja.Nombre = "frm-simulacion";
                        formularioBaja.FormularioId = null;
                    }
                }
                #endregion

                #region Confirmacion
                else if (!entity.IdSimulacion.Equals(null)) //Se da de baja la adhesion y actualiza Json
                {
                    var orden = new ConsultaOrigenResponse();
                    var bajaPDC = new SimulaPdcResponse();

                    #region Consulta Orden
                    orden = daMapsControles.ConsultaOrigen(entity); // ----> IDSimulacion Baja PDC
                    #endregion

                    #region PDC
                    if (formularioBaja.IdServicio.ToUpper() == Servicio.PoderDeCompra)
                    {
                        bajaPDC = BusinessHelper.SimularAltaBajaAdhesionPDC(entity, firma, CuentaPDC.Procesar, CuentaPDC.Baja, orden.Origen);
                        if (bajaPDC.FechaEfectiva.HasValue)
                        {
                            frm.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta).Valor = bajaPDC.FechaEfectiva.Value.Date;
                        }
                        entity.FechaEfectiva = bajaPDC?.FechaEfectiva.Value.Date; // ------> Si es PDC
                        entity.OrdenOrigen = bajaPDC.IDCuentaPDC;   // ------> Si es PDC
                    }
                    #endregion

                    #region Maps Baja primera parte
                    idBaja = daMapsControles.BajaAdhesion(entity); // Se da de baja
                    #endregion

                    //Se actualiza los campos necesarios
                    formularioBaja.IdSimulacion = entity.IdSimulacion;
                    formularioBaja.Comprobante = idBaja.ToString();
                    formularioBaja.Estado = TipoEstadoFormulario.Confirmacion;
                    if (string.Compare(frm.IdServicio, Servicio.Rtf, true) == 0)
                        frm.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta).Valor = DateTime.Now;
                    formularioBaja.FormularioId = null;

                    #region Maps Baja segunda parte
                    entity.TextoJson = formularioBaja.SerializarToJson(); //-----> Actualiza Json
                    idBaja = daMapsControles.BajaAdhesion(entity);
                    #endregion

                    #region Registrar Orden
                    if (formularioBaja.IdServicio.ToUpper() != Servicio.PoderDeCompra)
                    {
                        var registraOrdenRequest = new RegistraOrdenRequest();

                        registraOrdenRequest.CodEstadoProceso = TipoEstado.BajaOrden;
                        registraOrdenRequest.IdSimulacion = entity.IdSimulacion.Value;
                        registraOrdenRequest.Ip = entity.Ip;
                        registraOrdenRequest.Usuario = entity.Usuario;
                        registraOrdenRequest.IdServicio = entity.IdServicio;

                        daMapsControles.RegistrarOrden(registraOrdenRequest);
                    }
                    #endregion
                }
                #endregion
            }

            daMapsControles.LogFormulario(formularioBaja, formularioBaja.IdSimulacion, formularioBaja.IdAdhesion, idBaja);
            formularioBaja.Items = formularioBaja.Items?.OrderBy(x => (x as ControlSimple).Posicion).ToList();

            #region Baja SMC
            if (formularioBaja.IdServicio == Servicio.DolarMEPReverso ||
                formularioBaja.IdServicio == Servicio.DolarMEP ||
                formularioBaja.IdServicio == Servicio.DolarMEPGD30)
            {
                var parametrosSMC = new UpdateOrderRequest
                {
                    OrderId = 0,
                    MapsId = formularioBaja.IdAdhesion.Value,
                    Status = "CD",
                    Obs = "BAJA VMEP",
                    Usuario = formularioBaja.Usuario,
                    Ip = formularioBaja.Ip,
                };

                if (formularioBaja.IdServicio == Servicio.DolarMEP ||
                    formularioBaja.IdServicio == Servicio.DolarMEPGD30)
                {
                    parametrosSMC.Obs = "BAJA CMEP";
                }

                DependencyFactory.Resolve<ISmcDA>().UpdateOrder(parametrosSMC);
            }
            #endregion

            return formularioBaja;
        }

        /// <summary>
        /// Servicio que realiza la alta de una adhesion
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="firma"></param>
        /// <returns></returns>
        /// TODO: validar en que momentos es necesario abrir una transacción.
        public virtual FormularioResponse AltaAdhesion(FormularioResponse entity, DatoFirmaMaps firma)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            IServicesClient srvClient = DependencyFactory.Resolve<IServicesClient>();
            string frmOrigen = null;
            FormularioResponse formSiguiente = null;

            #region cuentas parte vieja
            #region Obtener Cuentas operativas               
            var ListaCtasOper = BusinessHelper.ObtenerCuentasPorTipo(entity, "OP", firma, false);

            if (ListaCtasOper.Length > 0)
            {
                var CtasOperatSinBloq = BusinessHelper.ValidarCuentasBloquedas(ListaCtasOper);
                if (CtasOperatSinBloq.Length > 0)
                {
                    entity.ListaCtasOperativas = string.Join(",", CtasOperatSinBloq.Where(a => a.DescripcionTipoCta != "REPATRIACION").Select(x => x.SucursalCta + "|" + x.NroCta + "|" + x.CodigoMoneda));
                    entity.ListaCtasRepatriacion = string.Join(",", CtasOperatSinBloq.Where(a => a.DescripcionTipoCta == "REPATRIACION").Select(x => x.SucursalCta + "|" + x.NroCta + "|" + x.CodigoMoneda));

                }
            }
            #endregion

            #region Obtener Cuentas Titulo
            var ListaCtasTit = BusinessHelper.ObtenerCuentasPorTipo(entity, "TI", firma, false);
            if (ListaCtasTit.Length > 0)
            {
                var CtasTitSinBloq = BusinessHelper.ValidarCuentasBloquedas(ListaCtasTit);
                if (CtasTitSinBloq.Length > 0)
                    entity.ListaCtasTitulo = string.Join(",", CtasTitSinBloq.Select(x => x.NroCta));
            }
            #endregion

            #region Cuentas PDC
            if (entity.Segmento.ToLower() != Segmento.BancaPrivada)
            {
                var listaCtasPDC = BusinessHelper.ObtenerCuentasPDC(entity, firma).ToArray();
                if (listaCtasPDC.Length > 0)
                {
                    entity.ListaCtasPDC = string.Join(",", listaCtasPDC.Select(x => x.NroCta));
                }
            }
            #endregion 
            #endregion

            if (string.Compare(entity.Estado, TipoEstadoFormulario.Simulacion, true) != 0)
            {
                var formActual = entity.ShallowCopy();

                if (!string.IsNullOrWhiteSpace(entity.IdServicio))
                {
                    frmOrigen = daMapsControles.ObtenerOrigen(entity); //TODO: validar los tipos de datos.
                }

                entity.FormularioId = BusinessHelper.ObtenerSiguienteFormulario(entity, frmOrigen);

                #region construir formulario
                var frmValues = daMapsControles.ObtenerConfigDeFormulario(entity);

                var aItemsEliminar = new List<ControlSimple>();

                entity.AsignarDatosBackend(frmValues, entity.IdServicio);

                #region Refactor para usar la factory de componentes strategy
                var componentes = entity.Items.Where(x => (x as ControlSimple).Validado == false).ToArray();
                formSiguiente = entity.ShallowCopy();
                formSiguiente.Items.Clear();
                formSiguiente.Items.AddRange(componentes);

                FormularioHelper.CrearItems(formSiguiente, firma);
                FormularioHelper.AsignarPadreAHijo(formSiguiente);
                formSiguiente.Items.AddRange(formActual.Items);

                BusinessHelper.DepurarControles(formSiguiente);

                //Obtengo el perfil inversor actual
                var serviceWebApi = DependencyFactory.Resolve<IServiceWebApiClient>();
                var consultaPerfilReq = new ConsultaPerfilInversorRequest()
                {
                    Nup = entity.Nup,
                    Encabezado = BusinessHelper.GenerarCabecera(entity.Canal, entity.SubCanal)
                };
                var reqSecurity = firma.MapperClass<RequestSecurity<ConsultaPerfilInversorRequest>>(TypeMapper.IgnoreCaseSensitive);
                reqSecurity.Datos = consultaPerfilReq;
                formSiguiente.PerfilInversor = serviceWebApi.ConsultaPerfilInversor(reqSecurity).Descripcion;
                #endregion


                #region Valores por Control
                //for (int i = 0; i < componentes.Length; i++)
                //{
                //    var currItem = componentes[i] as ControlSimple;

                //    var datos = daMapsControles.ObtenerDatosPorComponente(currItem, entity);

                //    if (datos != null)
                //    {

                //        #region Parche para no duplicar codigo de creacion de componentes. Se debe usar la factory
                //        switch (currItem.Nombre)
                //        {
                //            case NombreComponente.FondoCompuesto:
                //                var fc = new EstrategiaComp(new ConfigFondoCompuesto(entity, (FondoCompuesto)currItem));
                //                fc.Crear();
                //                break;

                //            default:
                //                currItem.AsignarDatosBackend(datos, null, currItem);
                //                break;
                //        } 
                //        #endregion


                //        #region Email
                //        if (string.Compare(currItem.Nombre, NombreComponente.Email, true) == 0)
                //        {
                //            if (string.Compare(entity.Segmento, Segmento.Retail, true) == 0)
                //            {
                //                ((InputText<string>)currItem).Valor = srvClient.GetMailXNup(entity.Canal, entity.SubCanal, entity.Nup);
                //                currItem.Bloqueado = true;
                //            }
                //            else if (string.Compare(entity.Segmento, Segmento.BancaPrivada, true) == 0)
                //            {
                //                currItem.Bloqueado = false;
                //            }
                //        }
                //        #endregion

                //        #region CuentaTitulo
                //        if (string.Compare(currItem.Nombre, NombreComponente.CuentaTitulo) == 0)
                //        {
                //            var ComponenteCuentaTitulo = currItem as Lista<ItemCuentaTitulos<string>>;
                //            var cuentasTitulos = BusinessHelper.ObtenerCuentasPorTipo(entity, "TI", firma, true); // Tipo == 8 && CodProducto == 60
                //            cuentasTitulos = BusinessHelper.ValidarCuentasBloquedas(cuentasTitulos);
                //            ComponenteCuentaTitulo.Items = new List<ItemCuentaTitulos<string>>();

                //            if (cuentasTitulos.Length == 0) //En caso que no existan cuentas titulo
                //            {
                //                ComponenteCuentaTitulo.Bloqueado = true;
                //                ComponenteCuentaTitulo.Validado = false;
                //            }

                //            for (int j = 0; j < cuentasTitulos.Length; j++)
                //            {
                //                var ctaTi = new ItemCuentaTitulos<string>();
                //                ctaTi.NumeroCtaTitulo = ctaTi.Valor = cuentasTitulos[j].NroCta.TrimStart(new char[] { '0' });
                //                ctaTi.Desc = string.Empty;
                //                ComponenteCuentaTitulo.Items.Add(ctaTi);
                //            }
                //        }
                //        #endregion

                //        #region CuentaOperativa
                //        if (string.Compare(currItem.Nombre, NombreComponente.CuentaOperativa, true) == 0)
                //        {
                //            var ComponenteCuentaOperativa = currItem as Lista<ItemCuentaOperativa<string>>;
                //            var cuentasOperativas = BusinessHelper.ObtenerCuentasPorTipo(entity, "OP", firma, true); // Tipo != 8 && CodProducto != 60
                //                                                                                                     //Se filtra por las cuentas que se puede operar
                //            var cuentasValidas = BusinessHelper.ValidarCuentas(cuentasOperativas, entity);
                //            ComponenteCuentaOperativa.Items = new List<ItemCuentaOperativa<string>>();

                //            if (cuentasValidas.Length == 0) //En caso que no existan cuentas para asociar
                //            {
                //                ComponenteCuentaOperativa.Bloqueado = true;
                //                ComponenteCuentaOperativa.Validado = false;
                //            }

                //            for (int l = 0; l < cuentasValidas.Length; l++)
                //            {
                //                var ctaOp = new ItemCuentaOperativa<string>();
                //                ctaOp.TipoCtaOperativa = cuentasValidas[l].TipoCta.ToString().PadLeft(2, '0');
                //                ctaOp.NumeroCtaOperativa = ctaOp.Valor = cuentasValidas[l].NroCta.TrimStart(new char[] { '0' });
                //                ctaOp.Producto = cuentasValidas[l].CodProducto;
                //                ctaOp.SubProducto = cuentasValidas[l].CodSubproducto;
                //                ctaOp.SucursalCtaOperativa = cuentasValidas[l].SucursalCta;
                //                ctaOp.CodigoMoneda = cuentasValidas[l].CodigoMoneda;
                //                ctaOp.Desc = cuentasValidas[l].DescripcionTipoCta;
                //                ctaOp.Titulares = BusinessHelper.ObtenerTitulares(cuentasValidas[l].Titulares);
                //                ctaOp.ValorPadre = cuentasValidas[l].CodigoMoneda;
                //                ComponenteCuentaOperativa.Items.Add(ctaOp);
                //            }
                //        }
                //        #endregion

                //        if (currItem.IdPadreDB != null && currItem.IdPadreDB != 0 && currItem.IdPadreDB != entity.IdComponente)
                //        {
                //            var itemPadre = (from ControlSimple itemContenedor in entity.Items
                //                             where itemContenedor.IdComponente == currItem.IdPadreDB
                //                             select itemContenedor).FirstOrDefault();

                //            entity.AsignarControlHijoAControlPadre(itemPadre, currItem);

                //            aItemsEliminar.Add(currItem);
                //        }
                //    }
                //}
                #endregion

                //foreach (var item in aItemsEliminar)
                //{
                //    entity.Items.Remove(item);
                //}

                //BusinessHelper.DepurarControles(entity);

                //entity.PerfilInversor = srvClient.GetPerfil(entity.Nup, firma);
                #endregion

            }
            else
            {
                formSiguiente = entity.ShallowCopy();
                SimularConfirmarAdhesion(formSiguiente, firma);
            }

            //entity.Items = entity.Items.OrderBy(x => (x as ControlSimple).Posicion).ToList();
            formSiguiente.Items = formSiguiente.Items.OrderBy(x => (x as ControlSimple).Posicion).ToList();

            #region Validacion VMEP
            var tieneMep =(formSiguiente.Items.Where(x => x.Id == "servicio-1").FirstOrDefault() as Lista<ItemServicio<string>>)?.Items?.Where(x => x.Valor == Servicio.DolarMEPReverso).Count() > 0;
            if (tieneMep)
            {
                var req = firma.MapperClass<RequestSecurity<ValidaRestriccionMEPRequest>>(TypeMapper.IgnoreCaseSensitive);
                req.Datos = new ValidaRestriccionMEPRequest()
                {
                    Ip = entity.Ip,
                    Usuario = entity.Usuario,
                    Nup = entity.Nup,
                    Segmento = entity.Segmento
                };

                ValidaRestriccionMEPResponse data = null;
                try
                {
                    data = DependencyFactory.Resolve<IServiceWebApiClient>().ValidarRestriccionMEP(req);
                }
                catch (Exception ex)
                {
                    LoggingHelper.Instance.Error($"{DateTime.Now.ToString("dd/MM/yyyy")} Error Validacion MEP: | {ex.Message} | {ex.InnerException.Message}");
                }

                if (data != null)
                {
                    if (data.ListaCuentas.Count == 0 && data.CodigoRestriccion == 0)
                    {
                        (formSiguiente.Items.Where(x => x.Id == "servicio-1").FirstOrDefault() as Lista<ItemServicio<string>>)?.Items?.RemoveAll(x => x.Valor == Servicio.DolarMEPReverso);
                    }
                }
            }
            #endregion

            SetearRelaciones(formSiguiente);

            daMapsControles.LogFormulario(formSiguiente);

            return formSiguiente;
        }

        private void SetearRelaciones(FormularioResponse formSiguiente)
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

        /// <summary>
        /// Método para simular o confirmar el formulario. Graba en la base de datos el formulario final.
        /// </summary>
        /// <param name="frm"></param>
        private void SimularConfirmarAdhesion(FormularioResponse frm, DatoFirmaMaps firma)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            IServicesClient srvClient = DependencyFactory.Resolve<IServicesClient>();
            long? cuentaBp = null;
            string TextoDisclaimer = string.Empty;

            if (frm.Estado.ToLower().Trim().Equals(TipoEstadoFormulario.Simulacion) && !frm.IdSimulacion.HasValue)
            {
                if (frm.Segmento.ToLower() == Segmento.BancaPrivada)
                {
                    var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
                    {
                        Nup = frm.Nup.ParseGenericVal<long?>(),
                        CuentaBp = 0
                    });

                    var ctaOperativa = frm.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
                    var ctaOperativaSeleccionada = ctaOperativa.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
                    cuentaBp = BusinessHelper.ValidarCuentas(atisResp, ctaOperativaSeleccionada.NumeroCtaOperativa.ParseGenericVal<long>());
                }

                frm.IdSimulacion = daMapsControles.SimularAdhesion(frm, cuentaBp).ParseGenericVal<long?>();
                frm.Bloqueado = true;
                frm.Validado = true;
            }
            else if (frm.IdSimulacion.HasValue)
            {
                frm.Estado = TipoEstadoFormulario.Confirmacion;
                frm.Id = "frm-confirmacion-1";
                frm.Nombre = "frm-confirmacion";
                frm.Bloqueado = true;
                frm.Validado = true;

                frm.Comprobante = daMapsControles.ConfirmarAdhesion(frm, TextoDisclaimer).ToString();
                frm.IdAdhesion = frm.Comprobante.ParseGenericVal<long?>();

                //se reconfirma para que se actualice el ID de comprobante en el json que se guarda.
                daMapsControles.ActualizarComprobanteAJson(frm);

            }
        }

        public ObtenerFormAdhesionesResp ObtenerFormAdhesiones(ObtenerFormAdhesionesReq entity)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var result = daMapsControles.ObtenerFormAdhesiones(entity);
            //var output = new FormularioResponse();
            var output = new ObtenerFormAdhesionesResp()
            {
                TextoJson = result.TextoJson
            };
            //output = deserealizedObject.ToObject<FormularioResponse>();

            return output;
        }

        public ObtenerIdAdhesionResp ObtenerIdAdhesion(ObtenerIdAdhesionReq entity)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var result = daMapsControles.ObtenerIdAdhesion(entity);

            return result;
        }

        public List<ConsultaFondosAGDResponse> ConsultaFondosAGD(ConsultaFondosAGDRequest entity)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var result = daMapsControles.ConsultaFondosAGD(entity);
            result.ForEach(fondo => {
                fondo.PuedeSuscribir = fondo.Suscripcion == "S";
                fondo.PuedeRescatar = fondo.Rescate == "S";
            });
            

            return result;
        }

        public ObtenerRTFDisponiblesResponse ObtenerRTFDisponiblesPorCliente(RequestSecurity<RTFWorkflowOnDemandReq> entity)
        {
            var listaRtf = DependencyFactory.Resolve<IMapsControlesDA>().ConsultaArchivosRTF(entity.Datos);

            var ultimoPeriodo = DependencyFactory.Resolve<IMapsControlesDA>().ConsultaUltimoPeriodoRTF(entity.Datos);

            var listaCuentas = new List<Cuenta>();

            var reqCuentasD = new RequestSecurity<GetCuentas>();

            var datosClienteD = new GetCuentas
            {
                DatoConsulta = entity.Datos.Nup,
                Segmento = "RTL",
                TipoBusqueda = "A",
                CuentasRespuesta = "TI",
                Ip = "1.1.1.1",
                Usuario = "B049684",
                Canal = entity.Canal,
                SubCanal = entity.SubCanal
            };

            reqCuentasD.Datos = datosClienteD;

            var cuentas = DependencyFactory.Resolve<IServiceWebApiClient>().GetCuentasFisicasYjuridicas(reqCuentasD);

            if (cuentas != null)
            {
                foreach (var cuenta in cuentas)
                {
                    cuenta.Cuentas?.ForEach(c => listaCuentas.Add(new Cuenta { NumeroCuenta = c.NroCta, Segmento = c.SegmentoCuenta }));
                }
            }

            var response = new ObtenerRTFDisponiblesResponse();
            response.ListaRTF = listaRtf != null ? listaRtf.Distinct().ToList() : listaRtf;
            response.ListaCuentas = listaCuentas;
            response.UltimoAnioGenerado = ultimoPeriodo.Anio;
            response.UltimoPeriodoGenerado = ultimoPeriodo.Periodo;


            return response;
        }

        public List<ArchivoRTF> ObtenerPdfPorCuentaRTF(RequestSecurity<RTFWorkflowOnDemandReq> entity)
        {
            IMapsControlesDA da = DependencyFactory.Resolve<IMapsControlesDA>();

            var cuentas = da.ObtenerArchivoRTF(entity.Datos);

            var pathParameter = da.ObtenerValorParametrizado(new ConsultaParametrizacionReq { NomParametro = "RTF_PATH" });

            foreach (var cuenta in cuentas)
            {
                cuenta.Archivo = cuenta.Archivo = DireccionBase64String(Path.Combine(pathParameter, cuenta.Path));
            }

            return cuentas;
        }


        public string DireccionBase64String(string direccionString)
        {
            var BinaryFile = File.ReadAllBytes(direccionString);
            return Convert.ToBase64String(BinaryFile);
        }


        private string ObtenerValorParametrizado(ConsultaParametrizacionReq request)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

            if (ValoresParametrizados.ContainsKey(request.NomParametro))
            {
                return ValoresParametrizados[request.NomParametro];
            }
            else
            {
                var valor = daMapsControles.ObtenerValorParametrizado(request);

                ValoresParametrizados.Add(request.NomParametro, valor);

                return valor;
            }          
        }

        private ControlSimple ObtenerComponenteDescripcion(FormularioResponse entity,long idDescripcion,string idServicio, IMapsControlesDA daMapsControles)
        {

            if (ComponenteDescripcion.ContainsKey(idServicio))
            {
                return ComponenteDescripcion[idServicio];
            }
            else
            {
                var valor = BusinessHelper.AgregarComponenteDescripcion(entity, idDescripcion, idServicio);

                ComponenteDescripcion.Add(idServicio, valor);

                return valor;
            }
        }


        private ControlSimple ObtenerComponenteVigencia(FormularioResponse entity, IMapsControlesDA daMapsControles,FormularioResponse fa)
        {

            //if(ComponenteFechaCompuesta == null)
            //{
                var idVigencia = daMapsControles.ObtenerIdComponente(NombreComponente.Vigencia, entity.Usuario, entity.Ip);
                var idFechaDesde = daMapsControles.ObtenerIdComponente(NombreComponente.FechaDesde, entity.Usuario, entity.Ip);
                var idFechaHasta = daMapsControles.ObtenerIdComponente(NombreComponente.FechaHasta, entity.Usuario, entity.Ip);
                var idPeriodos = daMapsControles.ObtenerIdComponente(NombreComponente.Periodos, entity.Usuario, entity.Ip);

                var fechaCompuesta = BusinessHelper.AgregarComponenteVigencia(entity, idVigencia, idPeriodos, idFechaDesde, idFechaHasta);

                ComponenteFechaCompuesta = new SeleccionVigencia
                {
                    idFechaDesde = idFechaDesde,
                    idFechaHasta = idFechaHasta,
                    idPeriodos = idPeriodos,
                    idVigencia = idVigencia,
                    FechaCompuesta = fechaCompuesta
                };
            //}


            var fechaEje = fa.Items.Where(x => x.Nombre == NombreComponente.Fecha)?.Where(y => string.Compare(y.Id, "fecha-2", true) == 0).FirstOrDefault();

            var compFechaHasta = ComponenteFechaCompuesta.FechaCompuesta.Items.FirstOrDefault(d => d.Nombre == NombreComponente.FechaHasta);
            (compFechaHasta as Fecha).FechaMin = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            (compFechaHasta as Fecha).FechaMax = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            (compFechaHasta as Fecha).Valor = (fechaEje as Fecha)?.Valor ?? DateTime.Now;

            var compFechaDesde = ComponenteFechaCompuesta.FechaCompuesta.Items.FirstOrDefault(d => d.Nombre == NombreComponente.FechaDesde);
            (compFechaDesde as Fecha).FechaMin = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            (compFechaDesde as Fecha).FechaMax = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            (compFechaDesde as Fecha).Valor = (fechaEje as Fecha)?.Valor ?? DateTime.Now;

            return ComponenteFechaCompuesta.FechaCompuesta;
        }


        private ControlSimple ObtenerComponenteEstado(FormularioResponse entity, IMapsControlesDA daMapsControles, string estado)
        {
            //ComponenteEstado = new EstadoAdhesion<string>();

            //if (ComponenteEstado == null)
            //{
            var idEstadoAdhesion = daMapsControles.ObtenerIdComponente(NombreComponente.EstadoAdhesion, entity.Usuario, entity.Ip);

            var componenteEstado = BusinessHelper.AgregarComponenteEstado(entity, idEstadoAdhesion);
            componenteEstado.Valor = estado;
        //}
        //    ComponenteEstado.Valor = estado;

          return componenteEstado;   
        }

    }
}
