using Isban.Maps.Business.Componente.Factory;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Helpers;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.UnityInject;
using System.Linq;
using System;
using System.Text;
using Isban.Maps.IBussiness;
using Isban.Maps.Business.Factory;
using Isban.Maps.Business.Formularios.Factory;
using Isban.Maps.Bussiness.Factory.Formularios;
using Isban.Maps.Entity.Controles.Compuestos;
using Isban.Maps.Entity.Controles.Independientes;

namespace Isban.Maps.Bussiness.Wizard
{
    public class WizardMaps : IWizardMaps//TODO: mover todos los metodos a una interfaz.
    {
        private FormularioResponse _formulario;
        private DatoFirmaMaps _firma;
        private long? idWizard;
        private string _sessionID;
        private long? _FormularioID;
        private string _OperacionesDisponibles;

        public WizardMaps(FormularioResponse formulario)
        {
            _formulario = formulario;
        }
        public WizardMaps(FormularioResponse formulario, DatoFirmaMaps firma)
        {
            _formulario = formulario;
            _firma = firma;
        }

        public FormularioResponse SiguientePaso()
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            EstrategiaComp context = null;
            ObtenerFormularioIdActual();

            daMapsControles.LogFormulario(_formulario);
            _formulario.Config = GenerarConfiguracion();

            #region Parche
            if (_formulario.IdServicio != null)
            {
                var req = _formulario.MapperClass<PasoWizardReq>(TypeMapper.IgnoreCaseSensitive);
                _FormularioID = _formulario.FormularioId = daMapsControles.ConsultaPasoSiguiente(req)?.FormularioId;

                context = ObtenerContexto();
                context.Crear();
            }
            else
            {
                context = new EstrategiaComp(new ConfigFormularioSAF(_formulario, _firma));
                context.Crear();

            }
            #endregion

            #region como deberia ser
            //switch (_formulario.Estado)
            //{
            //    case TipoEstadoFormulario.Simulacion:
            //    case TipoEstadoFormulario.Confirmacion:
            //    case TipoEstadoFormulario.Carga:
            //        var req = _formulario.MapperClass<PasoWizardReq>(TypeMapper.IgnoreCaseSensitive);
            //        _FormularioID = _formulario.FormularioId = daMapsControles.ConsultaPasoSiguiente(req)?.FormularioId;

            //        context = ObtenerContexto();
            //        context.Crear();

            //        break;
            //    case TipoEstadoFormulario.Consulta:

            //        //if (_formulario.IdServicio != null)
            //        //    throw new BusinessException("Request de Consulta de Servicios incorrecto");

            //        context = new EstrategiaComp(new ConfigFormularioSAF(_formulario, _firma));
            //        context.Crear();

            //        break;
            //    default:
            //        throw new Exception("Estado de Formulario incorrecto");
            //} 
            #endregion
            _formulario.Config = GenerarConfiguracion();

            return _formulario;
        }

        private EstrategiaComp ObtenerContexto()
        {
            EstrategiaComp context = null;

            switch (_formulario.IdServicio)
            {
                case Servicio.SAF:
                    context = new EstrategiaComp(new ConfigFormularioSAF(_formulario, _firma));

                    break;
                case Servicio.PoderDeCompra:
                    context = new EstrategiaComp(new ConfigFormularioPDC(_formulario, _firma));

                    break;
                case Servicio.Agendamiento:
                    context = new EstrategiaComp(new ConfigFomularioAGD(_formulario, _firma));

                    break;
                case Servicio.AgendamientoFH:
                    context = new EstrategiaComp(new ConfigFomularioAGD(_formulario, _firma));

                    break;
                case Servicio.Repatriacion:
                    context = new EstrategiaComp(new ConfigFormularioCTR(_formulario, _firma));

                    break;
                case Servicio.AltaCuenta:
                    context = new EstrategiaComp(new ConfigFormularioCTR(_formulario, _firma));

                    break;
                default:
                    context = new EstrategiaComp(new ConfigFormularioSAF(_formulario, _firma));
                    break;
            }

            return context;
        }

        private void ObtenerFormularioIdActual()
        {
            #region quitar cuando t-banco pueda recibir FormularioId
            //TODO: cuando t-banco pueda ver FormularioId adapatar esto para que se asigne correctamente.

            var frmID = _formulario.Config?.Split('|')?.Where(x => x.Contains("FormularioId")).FirstOrDefault();

            if (frmID != null && frmID.Split(':').Length > 1 && !string.IsNullOrWhiteSpace(frmID.Split(':')[1]))
            {
                _FormularioID = _formulario.FormularioId = frmID.Split(':')[1].ParseGenericVal<long?>();

            }
            #endregion
            else if (_formulario.FormularioId == null && _formulario.IdServicio != null)
            {
                var da = DependencyFactory.Resolve<IMapsControlesDA>();
                _formulario.FormularioId = da.ObtenerFormularioIdOrigenFlujo(_formulario);
            }

        }

        private string GenerarConfiguracion()
        {
            string configuracion = $"FormularioId:{_FormularioID}|SessionId:{_sessionID}|IdWizard:{idWizard}";
            configuracion += $"|Fondo:{_formulario.CodigoDeFondo}|MonedaFondo:{_formulario.MonedaFondo}";

            if (_formulario.FechaDeEjecucion.HasValue)
                configuracion += $"|FechaDeEjecucion:{ _formulario.FechaDeEjecucion.Value.ToShortDateString()}";

            if (!string.IsNullOrWhiteSpace(_formulario.Operacion))
                configuracion += $"|Operacion:{_formulario.Operacion}";

            if (!string.IsNullOrWhiteSpace(_OperacionesDisponibles))
                configuracion += $"|OperacionesDisponibles:{_OperacionesDisponibles}";

            return configuracion;
        }

        public FormularioResponse PasoAnterior()
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

            //TODO: el request puede ser similar al SiguientePaso()
            var req = new PasoWizardReq();

            daMapsControles.ConsultaPasoAnterior(req);

            return _formulario;
        }

        /// <summary>
        /// Guarda el Json del paso completado y validado
        /// </summary>
        public void RegistrarPasoWizard()
        {
            ObtenerSessionId();
            ObtenerFondoActual();
            ObtenerFechaEjecucionActual();
            ObtenerFormularioIdActual();
            ObtenerOperacion();
            if (!string.IsNullOrEmpty(_formulario.CodigoDeFondo))
            {
                ObtenerOperacionesDisponibles();
            }

            if (_formulario.Validado && _formulario.Items.Count > 0
                && string.Compare(_formulario.Estado, TipoEstadoFormulario.Consulta, true) != 0
                && _formulario.IdServicio != null && _sessionID != null)
            {
                var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

                var ctaOperativa = _formulario.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa)?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
                var ctaTitulo = _formulario.Items.GetControlMaps<Lista<ItemCuentaTitulos<string>>>(NombreComponente.CuentaTitulo)?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
                var moneda = _formulario.Items.GetControlMaps<Lista<ItemMoneda<string>>>(NombreComponente.Moneda)?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();

                var registrarReq = _formulario.MapperClass<RegistrarPasoWizard>(TypeMapper.IgnoreCaseSensitive);

                if (string.Compare(_formulario.IdServicio, Servicio.Agendamiento, true) == 0 || string.Compare(_formulario.IdServicio, Servicio.AgendamientoFH, true) == 0)
                {
                    var listaDeFondos = _formulario.Items.GetControlMaps<Lista<ItemGrupoAgd>>(NombreComponente.ListaFondos);
                    if (listaDeFondos != null)
                    {
                        var fondoSeleccionado = listaDeFondos.Items.SelectMany(x => x.Items.Where(y => y.Seleccionado)).FirstOrDefault();
                        registrarReq.CodigoFondo = fondoSeleccionado?.Valor;
                    }
                }
                else if (string.IsNullOrWhiteSpace(_formulario.CodigoDeFondo) && string.IsNullOrWhiteSpace(_formulario.MonedaFondo))
                {
                    var fondo = _formulario.Items.GetControlMaps<Lista<Item<string>>>(NombreComponente.ListadoFondos);
                    registrarReq.CodigoFondo = fondo?.Items.Where(x => x.Seleccionado == true).FirstOrDefault().Valor;
                }
                else
                {
                    registrarReq.CodigoFondo = _formulario.CodigoDeFondo;
                }

                //registrarReq.CodigoFondo = fondo?.Items.Where(x => x.Seleccionado == true).FirstOrDefault().Valor;
                registrarReq.CuentaOperativa = ctaOperativa?.NumeroCtaOperativa.ParseGenericVal<long?>();
                registrarReq.CuentaTitulo = ctaTitulo?.NumeroCtaTitulo == null ? ctaTitulo?.Valor.ParseGenericVal<long?>() : ctaTitulo?.NumeroCtaTitulo.ParseGenericVal<long?>();
                registrarReq.EstadoFormulario = _formulario.Estado;
                registrarReq.IdAdhesion = _formulario.IdSimulacion;
                registrarReq.CodigoAltaAdhesion = _formulario.IdAdhesion;
                if (string.IsNullOrWhiteSpace(_formulario.MonedaFondo))
                    registrarReq.CodigoMoneda = moneda?.Valor;
                else
                    registrarReq.CodigoMoneda = _formulario.MonedaFondo;
                registrarReq.SucursalCtaOperativa = ctaOperativa?.SucursalCtaOperativa;
                registrarReq.TipoCtaOperativa = ctaOperativa?.TipoCtaOperativa.ParseGenericVal<long?>();
                registrarReq.TextoJson = _formulario.SerializarToJson();
                registrarReq.CodigoBajaAdhesion = null;
                registrarReq.SessionId = _sessionID;
                registrarReq.FormularioId = _formulario.FormularioId;

                _formulario.FormAnterior = idWizard = daMapsControles.RegistrarPasoWizard(registrarReq);

            }
        }

        private void ObtenerOperacion()
        {
            var operacion = _formulario.Items.GetControlMaps<Lista<Item<string>>>(NombreComponente.Operacion);

            if (operacion != null)
            {
                var seleccionado = operacion.Items.Where(x => x.Seleccionado).FirstOrDefault();
                _formulario.Operacion = seleccionado.Valor;
            }
            else
            {
                var operConfig = _formulario.Config?.Split('|')?.Where(x => x.Contains("Operacion")).FirstOrDefault();
                if (operConfig != null && !string.IsNullOrWhiteSpace(operConfig.Split(':')[1]))
                    _formulario.Operacion = operConfig.Split(':')[1];
            }
        }

        private void ObtenerOperacionesDisponibles()
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var req = new OperacionesDisponiblesFondosReq();
            req.CodigoFondo = _formulario.CodigoDeFondo;
            req.Ip = _formulario.Ip;
            req.Usuario = _formulario.Usuario;
            var operaciones = daMapsControles.ObtenerOperacionesFondo(req);
            _OperacionesDisponibles = "";
            if (operaciones.Suscripcion == SiNo.SI)
            {
                _OperacionesDisponibles += Operaciones.OperSuscripcion;
            }
            if (operaciones.Rescate == SiNo.SI)
            {
                if (!String.IsNullOrEmpty(_OperacionesDisponibles))
                {
                    _OperacionesDisponibles += "-";
                }
                _OperacionesDisponibles += Operaciones.OperRescate;
            }
        }

        private void ObtenerFechaEjecucionActual()
        {
            var fecha = _formulario.Items.GetControlMaps<Fecha>(NombreComponente.Fecha);

            if (fecha != null)
            {
                _formulario.FechaDeEjecucion = fecha.Valor;
            }
            else
            {
                var fechaEjecucion = _formulario.Config?.Split('|')?.Where(x => x.Contains("FechaEjecucion")).FirstOrDefault();
                if (fechaEjecucion != null)
                    _formulario.FechaDeEjecucion = new DateTime(int.Parse(fechaEjecucion.Split(':')[2]), int.Parse(fechaEjecucion.Split(':')[1]), int.Parse(fechaEjecucion.Split(':')[0]));
            }
        }

        private void ObtenerFondoActual()
        {
            var compfondo = _formulario.Items.Where(x => x.Nombre == NombreComponente.ListaFondos).FirstOrDefault() as Lista<ItemGrupoAgd>;

            if (compfondo == null)
            {
                var fondocompuesto = _formulario.Items.Where(x => x.Nombre == NombreComponente.FondoCompuesto).FirstOrDefault() as FondoCompuesto;
                compfondo = fondocompuesto?.Items.Where(x => x.Nombre == NombreComponente.ListaFondos).FirstOrDefault() as Lista<ItemGrupoAgd>;
            }


            if (compfondo != null)
            {
                var fondoSel = compfondo.Items.SelectMany(x => x.Items.Where(y => y.Seleccionado)).FirstOrDefault();
                _formulario.CodigoDeFondo = fondoSel.Valor;
                _formulario.MonedaFondo = fondoSel.CodMonedaEmision;
            }
            else
            {
                var fondo = _formulario.Config?.Split('|')?.Where(x => x.Contains("Fondo")).FirstOrDefault();
                var monedaFondo = _formulario.Config?.Split('|')?.Where(x => x.Contains("MonedaFondo")).FirstOrDefault();

                if ((fondo != null && monedaFondo != null)
                    && fondo.Split(':').Length > 1 && !string.IsNullOrWhiteSpace(fondo.Split(':')[1])
                    && monedaFondo.Split(':').Length > 1 && !string.IsNullOrWhiteSpace(monedaFondo.Split(':')[1]))
                {
                    _formulario.CodigoDeFondo = fondo.Split(':')[1];
                    _formulario.MonedaFondo = monedaFondo.Split(':')[1];
                }
            }

        }

        //private void ObtenerMonedaFondoActual()
        //{
        //    var monedaFondo = _formulario.Config?.Split('|')?.Where(x => x.Contains("MonedaFondo")).FirstOrDefault();

        //    if (monedaFondo != null && monedaFondo.Split(':').Length > 1 && !string.IsNullOrWhiteSpace(monedaFondo.Split(':')[1]))
        //    {
        //        _formulario.MonedaFondo = monedaFondo.Split(':')[1];
        //    }
        //}

        private FormularioResponse QuitarFondoSinLegal()
        {
            FormularioResponse copyForm = null;
            var fondoComp = _formulario.Items.Where(x => x.Nombre == NombreComponente.FondoCompuesto).FirstOrDefault() as FondoCompuesto;
            var legal = fondoComp?.Items.Where(x => x.Nombre == NombreComponente.LegalAgendamiento).FirstOrDefault() as Lista<ItemLegal<string>>;

            if (legal != null && legal.Items.Count <= 0)
            {
                copyForm = _formulario.ShallowCopy();
                copyForm.Items.Remove(copyForm.Items.Where(x => x.Nombre == NombreComponente.FondoCompuesto).FirstOrDefault());
            }
            else
            {
                copyForm = _formulario;
            }

            return copyForm;
        }

        private void ObtenerSessionId()
        {

            //var ctaOperativas = _formulario.Items.Where(x => x.Nombre == NombreComponente.CuentaOperativa).FirstOrDefault();
            //var c = ctaOperativas as Lista<ItemCuentaOperativa<string>>;
            //var ctaOperativa = c.Items.Where(x => x.Seleccionado).FirstOrDefault();
            ItemCuentaTitulos<string> ctaTitulo = null;
            var ctaOperativa = _formulario.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa)?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();


            if (string.Compare(_formulario.Segmento, Segmento.BancaPrivada, true) == 0)
            {
                ctaTitulo = new ItemCuentaTitulos<string> { Valor = string.Empty };
            }
            else
            {
                var comp = _formulario.Items.Where(x => x.Nombre == NombreComponente.CuentaTitulo).FirstOrDefault() as Lista<ItemCuentaTitulos<string>>;
                ctaTitulo = comp?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();

            }
            var session = _formulario.Config?.Split('|')?.Where(x => x.Contains("SessionId")).FirstOrDefault();

            if (session != null && session.Split(':').Length > 1 && string.IsNullOrWhiteSpace(session.Split(':')[1]))
            {

                //if (_formulario.IdServicio != Servicio.Agendamiento && ctaOperativa?.Valor != null && ctaTitulo?.Valor != null && _formulario.IdServicio != null
                //&& _formulario.Nup != null && _formulario.Segmento != null)
                //{
                //    StringBuilder cadena = new StringBuilder();

                //    cadena.Append(_formulario.Nup);
                //    cadena.Append(_formulario.Segmento);
                //    cadena.Append(_formulario.IdServicio);
                //    cadena.Append(ctaOperativa.Valor);
                //    cadena.Append(ctaTitulo.Valor);

                //    _formulario.SessionId = _sessionID = Crypto.ObtenerMD5(cadena.ToString());
                //}
                //else if (_formulario.IdServicio == Servicio.Agendamiento)
                //{
                StringBuilder cadena = new StringBuilder();

                cadena.Append(_formulario.Nup);
                cadena.Append(_formulario.Segmento);
                cadena.Append(_formulario.IdServicio);
                cadena.Append(DateTime.Now.Ticks);

                _formulario.SessionId = _sessionID = Crypto.ObtenerMD5(cadena.ToString());
                //}
            }
            else
            {
                if (session != null && session.Split(':').Length > 1 && !string.IsNullOrWhiteSpace(session.Split(':')[1]))
                {
                    _formulario.SessionId = _sessionID = session.Split(':')[1];
                }
            }
        }

        public void PasoActual()
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var req = new ObtenerPasoReq();   //TODO: llenar con valores

            daMapsControles.ObtenerPaso(req);
        }
    }
}