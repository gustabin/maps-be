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
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Bussiness.Factory.Formularios
{
    public class ConfigFomularioAGD : ICrearComponente
    {
        private DatoFirmaMaps _firma;
        private FormularioResponse _entity;
        private ValorCtrlResponse[] _datosForm;
        private string estadoFormulario;
        private static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        private static List<string> propExcluidas = new List<string> { "items", "nup", "segmento", "canal", "subcanal", "idsimulacion", "cabecera", "perfilinversor" };


        public ConfigFomularioAGD(FormularioResponse _formulario, DatoFirmaMaps firma)
        {
            _entity = _formulario;
            _firma = firma;
        }

        public ConfigFomularioAGD(FormularioResponse formulario, DatoFirmaMaps firma, ValorCtrlResponse[] datos)
        {
            _entity = formulario;
            _firma = firma;
            _datosForm = datos;

        }
        public void Crear()
        {
            ObtenerFondoSeleccionado();

            //Limpio los items para no enviarlos nuevamente, ya que se guardo en otro paso.
            _entity.Items.Clear();

            CrearFormulario();//ok
            
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
                    if(_entity.IdServicio == Servicio.AgendamientoFH) ObtenerParametrosAGDFH();
                    Parallel.Invoke(
                        () => AsignarPadreAHijo(),
                        () => PerfilDeInversor()
                        );

                    break;
            }
            _entity.Items = _entity.Items?.OrderBy(x => (x as ControlSimple).Posicion).ToList();
        }
        
        private void ObtenerParametrosAGDFH()
        {

            if (_entity.Operacion != null && _entity.Operacion == "suscripcion") _entity.Operacion = Operaciones.OperSuscripcion;

            var fondo = BusinessHelper.ObtenerInfoFondo(_entity);

            var monedaRequest = _entity.MonedaFondo;
            var controlMoneda = _entity.Items.FirstOrDefault(a => a.Id == "moneda-seleccionada-1") as InputText<string>;

            if (controlMoneda != null)
            {
                controlMoneda.Valor = fondo?.Moneda;
            }


            if (!string.IsNullOrEmpty(monedaRequest))
            {

                var cuentaOperativa = _entity.Items?.FirstOrDefault(i => i.Id == "cuenta-operativa-1") as Lista<ItemCuentaOperativa<string>>;

                if (cuentaOperativa != null)
                    cuentaOperativa.Items = cuentaOperativa.Items?.Where(c => c.CodigoMoneda == monedaRequest)?.ToList();

            }

            var fondoRequest = _entity.CodigoDeFondo;

            var controlFondo = _entity.Items.FirstOrDefault(a => a.Id == "fondo-seleccionado-1") as InputText<string>;


            if (fondoRequest != null && controlFondo != null)
            {
                controlFondo.Valor = fondo?.DescFondo;
            }

            var operacionRequest = _entity.Operacion;
            var controlOperacion = _entity.Items.FirstOrDefault(a => a.Id == "operacion-seleccionada-1") as InputText<string>;


            if (operacionRequest != null && controlOperacion != null)
            {
                controlOperacion.Valor = operacionRequest;
            }

            if(!string.IsNullOrEmpty(_entity.CuentaTitulos))
            {
                var cuentaRequest = _entity.CuentaTitulos.PadLeft(12, '0');

                var ctaTitulo = _entity.Items.GetControlMaps<Lista<ItemCuentaTitulos<string>>>(NombreComponente.CuentaTitulo);

                var ctaTituloSeleccionada = ctaTitulo?.Items?.FirstOrDefault(x => x.NumeroCtaTitulo.PadLeft(12, '0') == cuentaRequest);

                if(ctaTituloSeleccionada != null)
                {
                    ctaTituloSeleccionada.Seleccionado = true;
                    ctaTitulo.Bloqueado = true;
                    ctaTitulo.Validado = true;
                }
            }

            if (!string.IsNullOrEmpty(_entity.CuentaOperativa))
            {
                var cuentaRequest = _entity.CuentaOperativa.PadLeft(12, '0');

                var ctaTitulo = _entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);

                var ctaTituloSeleccionada = ctaTitulo?.Items?.FirstOrDefault(x => x.NumeroCtaOperativa.PadLeft(12, '0') == cuentaRequest);

                if (ctaTituloSeleccionada != null)
                {
                    ctaTituloSeleccionada.Seleccionado = true;
                    ctaTitulo.Bloqueado = true;
                    ctaTitulo.Validado = true;
                }
            }


        }

        private void SeleccionarVigencia()
        {
            if (_entity.FechaDeEjecucion != null && _entity.FechaDeEjecucion.HasValue && (_entity.IdServicio == Servicio.Agendamiento || _entity.IdServicio == Servicio.AgendamientoFH))
            {
                var fechaDesde = _entity.Items.GetControlMaps<Fecha>(NombreComponente.FechaDesde);
                var fechaHasta = _entity.Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta);
                var valor = _entity.FechaDeEjecucion.Value;

                //Validacion feriados
                var reqCtas = _entity.MapperClass<DiasNoHabilesRequest>(TypeMapper.IgnoreCaseSensitive);
                reqCtas.FiltroPais = "AR";

                var reqSecurityGetCuenta = _firma.MapperClass<RequestSecurity<DiasNoHabilesRequest>>(TypeMapper.IgnoreCaseSensitive);

                reqSecurityGetCuenta.Datos = reqCtas;
                reqSecurityGetCuenta.Canal = "04";
                reqSecurityGetCuenta.SubCanal = "0099";

                var dias = DependencyFactory.Resolve<IServiceWebApiClient>().ConsultaDiasNoHabiles(reqSecurityGetCuenta);

                if (dias != null && dias.ListaFeriados.Count() > 0 && valor != null)
                {
                    if (dias.ListaFeriados.Select(x => DateTime.Parse(x.Fecha)).ToList().Contains(valor))
                    {
                        valor = valor.AddBusinessDays(1);
                        _entity.FechaDeEjecucion = valor;
                    }

                }

                if (fechaDesde != null && fechaHasta != null)
                {
                    fechaDesde.Valor = _entity.FechaDeEjecucion.Value;
                    fechaHasta.Valor = _entity.FechaDeEjecucion.Value;
                }

                fechaDesde.Bloqueado = true;
                fechaDesde.Validado = true;
                fechaHasta.Bloqueado = true;
                fechaHasta.Validado = true;

            }

            var periodo = _entity.Items.GetControlMaps<Lista<Item<decimal>>>(NombreComponente.Periodos);
            if (periodo != null)
            {
                var seleccion = periodo.Items.Where(x => x.Desc == "Otro Intervalo").FirstOrDefault();
                seleccion.Seleccionado = true;
                periodo.Bloqueado = true;
                periodo.Validado = true;
            }

            var fechaCompuesta = _entity.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia);
            if (fechaCompuesta != null)
            {
                fechaCompuesta.Bloqueado = true;
                fechaCompuesta.Validado = true;
            }
        }

        private void ObtenerFondoSeleccionado()
        {
            var fondo = _entity.Items.GetControlMaps<Lista<ItemGrupoAgd>>(NombreComponente.ListaFondos);

            if (fondo != null)
            {
                var fondoSeleccionado = fondo.Items.SelectMany(x => x.Items.Where(y => y.Seleccionado)).FirstOrDefault();
                if (fondoSeleccionado != null)
                {
                    _entity.CodigoDeFondo = fondoSeleccionado.Valor;
                    _entity.MonedaFondo = fondoSeleccionado.CodMonedaEmision;
                }
            }

        }

        private void CrearFormulario()
        {
            estadoFormulario = FormularioHelper.CrearFormulario(_entity, _firma);
        }

        private void CrearItems()
        {
            //FormularioHelper.CrearItems(_entity, _firma);

            var moneda = new Task(SeleccionarMoneda);
            moneda.Start();

            var vigencia = new Task(SeleccionarVigencia);
            vigencia.Start();

            var min = new Task(SetearValoresMinimo);
            min.Start();

            var operDisp = new Task(SetearOperacionesDisponibles);
            operDisp.Start();

            Task.WaitAll(new Task[] { moneda, vigencia, min, operDisp });

        }

        private void SetearValoresMinimo()
        {
            var importe = _entity.Items.GetControlMaps<InputNumber<decimal?>>(NombreComponente.SaldoMinimo);

            if (importe != null)
            {
                IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

                var req = new DetalleDeFondoReq
                {
                    CodigoDeFondo = _entity.CodigoDeFondo,
                    Operacion = ObtenerCodigoOperacion(_entity.Operacion)
                };

                var detalles = daMapsControles.ObtenerDetalleDelFondo(req);

                importe.MinValor = detalles.MinValor;
                importe.MaxValor = detalles.MaxValor;

                var info = new StringBuilder(detalles.Informacion);

                info.Replace("@moneda", detalles.Moneda);
                info.Replace("@minimo", detalles.MinValor.ToString("N", System.Globalization.CultureInfo.GetCultureInfo("es-ar")));
                info.Replace("@monena", detalles.Moneda);
                info.Replace("@maximo", detalles.MaxValor.ToString("N", System.Globalization.CultureInfo.GetCultureInfo("es-ar")));
                info.Replace("@operacion", detalles.Operacion);

                importe.Informacion = info.ToString();

                importe.Simbolo = string.Compare(detalles.Moneda, "U$S") == 0 ? "USD" : "ARS";
            }
        }

        private string ObtenerCodigoOperacion(string operacion)
        {
            string result = null;

            switch (operacion.ToLower())
            {
                case "suscripción":
                    result = Operaciones.Suscripcion;
                    break;
                case "suscripcion":
                    result = Operaciones.Suscripcion;
                    break;
                case "rescate":
                    result = Operaciones.Rescate;
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        private void SetearOperacionesDisponibles()
        {
            var ctrlOperacion = _entity.Items.GetControlMaps<Lista<Item<string>>>(NombreComponente.Operacion);
            var operaciones = _entity.Config?.Split('|')?.Where(x => x.Contains("OperacionesDisponibles")).FirstOrDefault()?.Split(':')[1];

            if (ctrlOperacion != null && operaciones != null)
            {
                var operacionesDisponibles = operaciones.Split('-').ToList();
                ctrlOperacion.Items.RemoveAll(item => !operacionesDisponibles.Any(o => o.ToLower().Trim() == item.Desc.ToLower().Trim()));
                if (ctrlOperacion.Items.Count == 1)
                {
                    ctrlOperacion.Items.FirstOrDefault().Seleccionado = true;
                    ctrlOperacion.Bloqueado = true;
                }
            }
        }

        private void SeleccionarMoneda()
        {
            var ctrlMoneda = _entity.Items.GetControlMaps<Lista<ItemMoneda<string>>>(NombreComponente.Moneda);
            var moneda = _entity.Config?.Split('|')?.Where(x => x.Contains("MonedaFondo")).FirstOrDefault();

            if (moneda != null && ctrlMoneda != null)
            {
                if (moneda.Split(':').Length > 1 && !string.IsNullOrWhiteSpace(moneda.Split(':')[1]))
                {
                    string valorMoneda = moneda.Split(':')[1];

                    var monedaSeleccionada = ctrlMoneda.Items.Where(x => x.CodigoIso == valorMoneda).FirstOrDefault();

                    if (monedaSeleccionada != null)
                    {
                        monedaSeleccionada.Seleccionado = true;
                        ctrlMoneda.Bloqueado = true;
                    }
                }
            }
        }

        private void PerfilDeInversor()
        {
            //Evito buscar nuevamente el perfil si ya lo tiene.
            if (_entity.PerfilInversor == null)
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
        }

        private void Confirmar()
        {
            var daConf = DependencyFactory.Resolve<IMapsControlesDA>();
            var itemsConf = daConf.ObtenerDatosDeConfirmacion(_entity);
            _entity.Items.Clear();
            _entity.Items.AddRange(ObtenerItemsDeFormulario(itemsConf));

            CambiarListaFondosPorFondoCompuesto();

            _entity.Comprobante = daConf.ConfirmarAdhesion(_entity, string.Empty).ToString();
            _entity.IdAdhesion = _entity.Comprobante.ParseGenericVal<long?>();
            _entity.Titulo = FormularioHelper.ObtenerTituloSimulación(_entity.IdServicio);

            //se reconfirma para que se actualice el ID de comprobante en el json que se guarda.
            daConf.ActualizarComprobanteAJson(_entity);

        }

        [Obsolete("Se debe quitar cuando se defina como deberia ser el fondo")]
        private void CambiarListaFondosPorFondoCompuesto()
        {
            var fondo = _entity.Items.GetControlMaps<Lista<ItemGrupoAgd>>(NombreComponente.ListaFondos);
            var legal = _entity.Items.GetControlMaps<Lista<ItemLegal<string>>>(NombreComponente.LegalAgendamiento);

            if (fondo != null)
            {
                var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
                var fondoCompuesto = new FondoCompuesto();
                fondoCompuesto.Nombre = NombreComponente.FondoCompuesto;
                fondoCompuesto.Tipo = NombreComponente.FondoCompuesto;
                fondoCompuesto.Bloqueado = true;
                fondoCompuesto.Validado = true;
                fondoCompuesto.IdComponente = daMapsControles.ObtenerIdComponente(NombreComponente.FondoCompuesto, _entity.Usuario, _entity.Ip);
                var fact = new EstrategiaComp(new ConfigFondoCompuesto(_entity, fondoCompuesto));
                fact.Crear();

                fondoCompuesto.Items.Clear();
                fondoCompuesto.Items.Add(fondo);
                fondoCompuesto.Items.Add(legal);

                _entity.Items.Remove(fondo);
                _entity.Items.Remove(legal);
                _entity.Items.Add(fondoCompuesto);
            }
        }

        private void Simular()
        {
            long? cuentaBp = null;
            var daSim = DependencyFactory.Resolve<IMapsControlesDA>();
            var items = daSim.ObtenerDatosDeSimulacion(_entity);
            _entity.Items.Clear();
            _entity.Items.AddRange(ObtenerItemsDeFormulario(items));

            if (_entity.Segmento.ToLower() == Segmento.BancaPrivada)
            {
                var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
                {
                    Nup = _entity.Nup.ParseGenericVal<long?>(),
                    CuentaBp = 0
                });

                //ver si se puede hacer una busqueda recursiva dentro del json para evitar linq
                var ctaOperativa = _entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
                var ctaOperativaSeleccionada = ctaOperativa.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
                cuentaBp = BusinessHelper.ValidarCuentas(atisResp, ctaOperativaSeleccionada.NumeroCtaOperativa.ParseGenericVal<long>());
            }

            ComprobarFeriados();




            //obtener los items de los formulario y agregarlos al de simulacion

            _entity.IdSimulacion = daSim.SimularAdhesion(_entity, cuentaBp).ParseGenericVal<long?>();
            _entity.Bloqueado = true;
            _entity.Validado = true;
            _entity.Titulo = FormularioHelper.ObtenerTituloSimulación(_entity.IdServicio);

        }

        private void ComprobarFeriados()
        {
            try
            {
 
            var fechaEjecucion = _entity.Items.Where(x => x.Nombre == NombreComponente.Fecha)?.Where(y => string.Compare(y.Id, "fecha-2", true) == 0).FirstOrDefault() as Fecha;

            var valor = fechaEjecucion?.Valor;

            var reqCtas = _entity.MapperClass<DiasNoHabilesRequest>(TypeMapper.IgnoreCaseSensitive);
            reqCtas.FiltroPais = "AR";

            var reqSecurityGetCuenta = _firma.MapperClass<RequestSecurity<DiasNoHabilesRequest>>(TypeMapper.IgnoreCaseSensitive);

            reqSecurityGetCuenta.Datos = reqCtas;
            reqSecurityGetCuenta.Canal = "04";
            reqSecurityGetCuenta.SubCanal = "0099";

            var dias = DependencyFactory.Resolve<IServiceWebApiClient>().ConsultaDiasNoHabiles(reqSecurityGetCuenta);

            if (dias != null && dias.ListaFeriados.Count() > 0 && valor != null)
            {
                foreach (var dia in dias.ListaFeriados)
                {

                    var feriado = Convert.ToDateTime(dia.Fecha);

                    if (feriado.Date == valor.GetValueOrDefault().Date)
                    {
                        valor = valor.GetValueOrDefault().AddBusinessDays(1);
                    }

                }
            }

            fechaEjecucion.Valor = valor;

            }
            catch (Exception)
            {

              
            }

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
        public void Validar()
        {
            //
        }
    }

}
