
namespace Isban.Maps.Bussiness
{
    using Entity.Base;
    using Entity.Constantes.Estructuras;
    using Entity.Controles;
    using Entity.Extensiones;
    using Entity.Response;
    using IBussiness;
    using IDataAccess;
    using Isban.Maps.Entity.Constantes.Enumeradores;
    using Isban.Maps.Entity.Controles.Compuestos;
    using Isban.Maps.Entity.Controles.Customizados;
    using Isban.Maps.Entity.Request;
    using Isban.Mercados;
    using Isban.Mercados.LogTrace;
    using Isban.Mercados.Service.InOut;
    using Mercados.UnityInject;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MapsServiciosPDCBusiness : IMapsPDCServiciosBusiness
    {
        public virtual FormularioResponse PDCAltaAdhesion(FormularioResponse entity, DatoFirmaMaps firma)
        {
            LoggingHelper.Instance.Information($"Inicio de adhesion PDC: {entity.Nup}");
            LoggingHelper.Instance.Information($"Formulario : {JsonConvert.SerializeObject(entity)}");

            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            IServicesClient srvClient = DependencyFactory.Resolve<IServicesClient>();
            var frmOrigen = string.Empty;

            LoggingHelper.Instance.Information($"Comienzo validaciones. FormilarioTipoEstado:{entity.Estado}");
            if (!entity.Estado.ToLower().Equals(TipoEstadoFormulario.Simulacion))
            {
                if (!string.IsNullOrEmpty(entity.IdServicio))
                {
                    frmOrigen = daMapsControles.ObtenerOrigen(entity);
                    LoggingHelper.Instance.Information($"Formulario origen: {frmOrigen}");
                }

                entity.FormularioId = BusinessHelper.ObtenerSiguienteFormulario(entity, frmOrigen);
                LoggingHelper.Instance.Information($"Next formulario: {entity}");

                #region construir formulario
                var frmValues = daMapsControles.ObtenerConfigDeFormulario(entity);

                var aItemsEliminar = new List<ControlSimple>();

                LoggingHelper.Instance.Information($"Asigna Datos Backend");
                entity.AsignarDatosBackend(frmValues, entity.IdServicio);

                LoggingHelper.Instance.Information($"Asigna componentes");
                var componentes = entity.Items.Where(x => (x as ControlSimple).Validado == false).ToArray();

                #region Obtener Cuentas operativas
                var ListaCtasOper = BusinessHelper.ObtenerCuentasPorTipo(entity, "OP", firma, true);
                LoggingHelper.Instance.Information($"Obtiene cuentas OP: {JsonConvert.SerializeObject(ListaCtasOper)}");

                ClienteCuentaDDC[] cuentasOperativas = null;
                if (ListaCtasOper.Length > 0)
                {
                    cuentasOperativas = ListaCtasOper;
                }
                #endregion

                #region Obtener Cuentas Titulo
                var ListaCtasTit = BusinessHelper.ObtenerCuentasPDC(entity, firma).ToArray();
                LoggingHelper.Instance.Information($"Obtiene cuentas Titulo {JsonConvert.SerializeObject(ListaCtasTit)}");

                ClienteCuentaDDC[] cuentasValidas = null;
                if (ListaCtasTit.Length > 0)
                {
                    cuentasValidas = BusinessHelper.ValidarCuentas(ListaCtasTit, entity);
                }
                LoggingHelper.Instance.Information($"Cuentas validas: {JsonConvert.SerializeObject(cuentasValidas)}");

                #endregion

                for (int i = 0; i < componentes.Length; i++)
                {
                    var currItem = componentes[i] as ControlSimple;

                    LoggingHelper.Instance.Information($"Obtiene datos por componente.");
                    var datos = daMapsControles.ObtenerDatosPorComponente(currItem, entity);

                    if (datos != null)
                    {
                        LoggingHelper.Instance.Information($"Asigna componentes");
                        currItem.AsignarDatosBackend(datos, null, currItem);

                        #region Cuenta Titulo
                        if (currItem.Nombre.ToLower() == NombreComponente.CuentaTitulo)
                        {
                            var ComponenteCuentaTitulo = currItem as Lista<ItemCuentaTitulos<string>>;
                            //Se filtra por las cuentas que se puede operar
                            ComponenteCuentaTitulo.PadreId = null;
                            ComponenteCuentaTitulo.Items = new List<ItemCuentaTitulos<string>>();

                            if (cuentasValidas == null || cuentasValidas.Length == 0) //En caso que no existan cuentas titulo
                            {
                                ComponenteCuentaTitulo.Bloqueado = true;
                                ComponenteCuentaTitulo.Validado = false;
                            }
                            else
                            {
                                foreach (var cuenta in cuentasValidas)
                                {
                                    var ctaTi = new ItemCuentaTitulos<string>();
                                    ctaTi.NumeroCtaTitulo = ctaTi.Valor = cuenta.NroCta.TrimStart(new char[] { '0' }); //cuenta.NroCta;
                                    ctaTi.Titulares = BusinessHelper.ObtenerTitulares(cuenta.Titulares);
                                    ctaTi.Desc = "Cuenta custodia"; //cuenta.DescripcionTipoCta;
                                    ctaTi.ValorPadre = null;
                                    ComponenteCuentaTitulo.Items.Add(ctaTi);
                                }
                            }
                        }
                        #endregion

                        #region Cuenta Operativa
                        if (currItem.Nombre.ToLower() == NombreComponente.CuentaOperativa)
                        {
                            var ComponenteCuentaOperativa = currItem as Lista<ItemCuentaOperativa<string>>;

                            ComponenteCuentaOperativa.Items = new List<ItemCuentaOperativa<string>>();

                            if (cuentasOperativas == null || cuentasOperativas.Length == 0) //En caso que no existan cuentas para asociar
                            {
                                ComponenteCuentaOperativa.Bloqueado = true;
                                ComponenteCuentaOperativa.Validado = false;
                            }
                            else
                            {
                                foreach (var cuenta in cuentasOperativas)
                                {
                                    var ctaOp = new ItemCuentaOperativa<string>();
                                    ctaOp.TipoCtaOperativa = cuenta.TipoCta.ToString().PadLeft(2, '0');
                                    ctaOp.NumeroCtaOperativa = ctaOp.Valor = cuenta.NroCta.TrimStart(new char[] { '0' });
                                    ctaOp.Producto = cuenta.CodProducto;
                                    ctaOp.SubProducto = cuenta.CodSubproducto;
                                    ctaOp.SucursalCtaOperativa = cuenta.SucursalCta;
                                    ctaOp.CodigoMoneda = cuenta.CodigoMoneda;
                                    ctaOp.Desc = cuenta.DescripcionTipoCta;
                                    ctaOp.Titulares = BusinessHelper.ObtenerTitulares(cuenta.Titulares);
                                    ctaOp.ValorPadre = cuenta.CodigoMoneda;
                                    var saldo = BusinessHelper.ObtenerSaldoCuenta(cuenta, entity.Segmento, entity.Nup, entity.Canal, entity.SubCanal, entity.Ip, entity.Usuario);
                                    if (saldo != null)
                                        ctaOp.SaldoCta = saldo;
                                    ComponenteCuentaOperativa.Items.Add(ctaOp);
                                }
                            }
                        }
                        #endregion

                        if (currItem.IdPadreDB != null && currItem.IdPadreDB != 0 && currItem.IdPadreDB != entity.IdComponente)
                        {
                            var itemPadre = (from ControlSimple itemContenedor in entity.Items
                                             where itemContenedor.IdComponente == currItem.IdPadreDB
                                             select itemContenedor).FirstOrDefault();

                            entity.AsignarControlHijoAControlPadre(itemPadre, currItem);

                            aItemsEliminar.Add(currItem);
                        }
                    }
                }

                foreach (var item in aItemsEliminar)
                {
                    entity.Items.Remove(item);
                }

                //entity.PerfilInversor = srvClient.GetPerfil(entity.Nup, firma);


                var serviceWebApi = DependencyFactory.Resolve<IServiceWebApiClient>();
                LoggingHelper.Instance.Information($"Genera request perfil inversor");
                var consultaPerfilReq = new ConsultaPerfilInversorRequest()
                {
                    Nup = entity.Nup,
                    Encabezado = BusinessHelper.GenerarCabecera(entity.Canal, entity.SubCanal)
                };
                var reqSecurity = firma.MapperClass<RequestSecurity<ConsultaPerfilInversorRequest>>(TypeMapper.IgnoreCaseSensitive);
                reqSecurity.Datos = consultaPerfilReq;
                LoggingHelper.Instance.Information($"Consulta perfil inversor");
                entity.PerfilInversor = serviceWebApi.ConsultaPerfilInversor(reqSecurity).Descripcion;
                #endregion
            }

            LoggingHelper.Instance.Information($"Ordena componentes");
            entity.Items = entity.Items.OrderBy(x => (x as ControlSimple).Posicion).ToList();

            LoggingHelper.Instance.Information($"Simula/Confirma Adhesion");
            SimularConfirmarAdhesion(entity, firma);

            LoggingHelper.Instance.Information($"Log Formulario");
            daMapsControles.LogFormulario(entity);

            LoggingHelper.Instance.Information($"Fin de adhesion PDC");
            return entity;
        }

        private void SimularConfirmarAdhesion(FormularioResponse frm, DatoFirmaMaps firma)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            IServicesClient srvClient = DependencyFactory.Resolve<IServicesClient>();

            LoggingHelper.Instance.Information($"Comienzo simulacion/confirmacion PDC");
            #region simulacion
            if (frm.Estado.ToLower().Trim().Equals(TipoEstadoFormulario.Simulacion) && !frm.IdSimulacion.HasValue)
            {
                LoggingHelper.Instance.Information($"Simulacion PDC");
                var simulacionPDC = new SimulaPdcResponse();

                #region PDC
                LoggingHelper.Instance.Information($"Llamado al servicio SimularAltaBajaAdhesionPDC");
                simulacionPDC = BusinessHelper.SimularAltaBajaAdhesionPDC(frm, firma, CuentaPDC.Simular, CuentaPDC.Alta);
                LoggingHelper.Instance.Information($"Resultado: {JsonConvert.SerializeObject(simulacionPDC)}");
                #endregion

                if (!simulacionPDC.IDSimCuentaPDC.HasValue)
                {
                    frm.Error = (int)TiposDeError.ErrorSimulacionAlta;
                    frm.Error_desc = "No se puede simular adhesión en PDC";
                }
                else
                {
                    if (simulacionPDC.FechaEfectiva.HasValue)
                    {
                        frm.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaDesde).Valor = simulacionPDC.FechaEfectiva.Value.Date;
                    }

                    #region Maps
                    frm.SimOrdenOrigen = simulacionPDC.IDSimCuentaPDC;
                    frm.IdSimulacion = daMapsControles.SimularAdhesion(frm).ParseGenericVal<long?>();
                    frm.Bloqueado = true;
                    frm.Validado = true;
                    #endregion

                    if (!frm.IdSimulacion.HasValue)
                    {
                        frm.Error = (int)TiposDeError.ErrorSimulacionAlta;
                        frm.Error_desc = "No se puede simular adhesión";
                    }
                }
            }
            #endregion
            #region confirmacion
            else if (frm.IdSimulacion.HasValue)
            {
                LoggingHelper.Instance.Information($"Confirmacion PDC");
                var consultaOrigen = new ConsultaOrigenResponse();
                var altaPDC = new SimulaPdcResponse();

                #region PDC
                LoggingHelper.Instance.Information($"Consulta origen PDC");
                consultaOrigen = daMapsControles.ConsultaOrigen(frm); // ---> Devuelve el id de simulacion de PDC                        
                LoggingHelper.Instance.Information($"IdSimPDC: {JsonConvert.SerializeObject(consultaOrigen)}");
                LoggingHelper.Instance.Information($"Invoca servicio SimularAltaBajaAdhesionPDC");
                altaPDC = BusinessHelper.SimularAltaBajaAdhesionPDC(frm, firma, CuentaPDC.Procesar, CuentaPDC.Alta, consultaOrigen.Origen);
                LoggingHelper.Instance.Information($"AltaPDC: {altaPDC}");
                #endregion

                if (altaPDC.FechaEfectiva.HasValue)
                {
                    frm.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia).Items.GetControlMaps<Fecha>(NombreComponente.FechaDesde).Valor = altaPDC.FechaEfectiva.Value.Date;
                }

                #region Maps
                LoggingHelper.Instance.Information($"Seteo de propiedades y confirmacion MAPS");
                frm.Estado = TipoEstadoFormulario.Confirmacion;
                frm.Id = "frm-confirmacion-1";
                frm.Nombre = "frm-confirmacion";
                frm.Bloqueado = true;
                frm.Validado = true;
                frm.OrdenOrigen = altaPDC.IDCuentaPDC;
                LoggingHelper.Instance.Information($"Confirma adhesion MAPS");
                frm.Comprobante = daMapsControles.ConfirmarAdhesion(frm, null).ToString();
                frm.IdAdhesion = frm.Comprobante.ParseGenericVal<long?>();
                //se reconfirma para que se actualice el ID de comprobante en el json que se guarda.
                LoggingHelper.Instance.Information($"Actualiza Comprobante MAPS");
                daMapsControles.ActualizarComprobanteAJson(frm);
                //Eliminación del primer legal
                var componente = frm.Items.Where(x =>
                        string.Compare((x as ControlSimple).Config, NombreComponente.PrimerLegalPDC, true) == 0)
                        .ToList();
                if (componente.Count > 0) //Existe
                {
                    frm.Items.Remove(frm.Items.Where(y => (y as ControlSimple).Config == NombreComponente.PrimerLegalPDC).FirstOrDefault());
                }
                #endregion
                /*
                #region Registrar Orden
                var registraOrdenRequest = new RegistraOrdenRequest();

                registraOrdenRequest.CodEstadoProceso = TipoEstado.Procesado;
                registraOrdenRequest.IdSimulacion = frm.IdSimulacion.Value;
                registraOrdenRequest.Ip = frm.Ip;
                registraOrdenRequest.Usuario = frm.Usuario;
                registraOrdenRequest.IdServicio = frm.IdServicio;

                daMapsControles.RegistrarOrden(registraOrdenRequest);
                #endregion*/
            }
            LoggingHelper.Instance.Information($"Fin SimularConfirmarAdhesion");
            #endregion
        }
    }
}

