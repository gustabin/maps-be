using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Bussiness.Factory;
using Isban.Maps.Bussiness.Factory.Formularios;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
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
    public class ConfigFormularioSAF : ICrearComponente
    {
        private static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        private static List<string> propExcluidas = new List<string> { "items", "nup", "segmento", "canal", "subcanal", "idsimulacion", "cabecera" };

        private FormularioResponse _entity;
        private DatoFirmaMaps _firma;
        private string estadoFormulario;
        ValorCtrlResponse[] _datosForm;
        
        public ConfigFormularioSAF(FormularioResponse formulario)
        {
            _entity = formulario;
        }

        public ConfigFormularioSAF(FormularioResponse formulario, DatoFirmaMaps firma)
        {
            _entity = formulario;
            _firma = firma;
        }

        public ConfigFormularioSAF(FormularioResponse formulario, DatoFirmaMaps firma, ValorCtrlResponse[] datos)
        {
            _entity = formulario;
            _firma = firma;
            _datosForm = datos;

        }

        /// <summary>
        /// Configura un nuevo formulario del tipo Formulario Response 
        /// </summary>
        /// <param name="entity"></param>
        public void Crear()
        {
            //Limpio los items para no enviarlos nuevamente, ya que se guardo en otro paso.
            _entity.Items.Clear();

            CrearFormulario();

            switch (estadoFormulario)
            {
                case TipoEstadoFormulario.Simulacion:

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

                    //obtener los items de los formulario y agregarlos al de simulacion

                    _entity.IdSimulacion = daSim.SimularAdhesion(_entity, cuentaBp).ParseGenericVal<long?>();
                    _entity.Bloqueado = true;
                    _entity.Validado = true;
                    _entity.Titulo = FormularioHelper.ObtenerTituloSimulación(_entity.IdServicio);

                    break;
                case TipoEstadoFormulario.Confirmacion:

                    long? cuentaBpSMC = null;
                    var daConf = DependencyFactory.Resolve<IMapsControlesDA>();
                    var itemsConf = daConf.ObtenerDatosDeConfirmacion(_entity);

                    _entity.Items.AddRange(ObtenerItemsDeFormulario(itemsConf));
                    _entity.Comprobante = daConf.ConfirmarAdhesion(_entity, string.Empty).ToString();
                    _entity.IdAdhesion = _entity.Comprobante.ParseGenericVal<long?>();
                    _entity.Titulo = FormularioHelper.ObtenerTituloSimulación(_entity.IdServicio);
                    //se reconfirma para que se actualice el ID de comprobante en el json que se guarda.
                    daConf.ActualizarComprobanteAJson(_entity);

                    //Guardar solicitud MEP en SMC
                    #region Log
                    Mercados.LogTrace.LoggingHelper.Instance.Warning(_entity.SerializarToJson(), "SMC | buscar datos para sp");
                    #endregion

                    if (_entity.IdServicio == Servicio.DolarMEPReverso ||
                        _entity.IdServicio == Servicio.DolarMEP ||
                        _entity.IdServicio == Servicio.DolarMEPGD30)
                    {
                        var ctaOperativa = _entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
                        var ctaTitulo = _entity.Items.GetControlMaps<Lista<ItemCuentaTitulos<string>>>(NombreComponente.CuentaTitulo);

                        var ctaOperativaSeleccionada = ctaOperativa?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
                        var ctaTituloSeleccionada = ctaTitulo?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
                        var ItemsaldoMinimo = _entity.Items.Where(x => (x as ControlSimple).Nombre == NombreComponente.SaldoMinimo).FirstOrDefault();

                        if (_entity.Segmento.ToLower() == Segmento.BancaPrivada)
                        {
                            var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
                            {
                                Nup = _entity.Nup.ParseGenericVal<long?>(),
                                CuentaBp = 0
                            });

                            cuentaBpSMC = BusinessHelper.ValidarCuentas(atisResp, ctaOperativaSeleccionada.NumeroCtaOperativa.ParseGenericVal<long>());
                        }

                        var parametrosSMC = new InsertOrderRequest
                        {
                            Nup = _entity.Nup,
                            OperationType = "SELL_DOLLAR_MEP",
                            CustodyAccount = _entity.Segmento.ToLower() == Segmento.Retail ? ctaTituloSeleccionada?.NumeroCtaTitulo.ParseGenericVal<decimal?>() : cuentaBpSMC.ParseGenericVal<decimal?>(),
                            Amount = (ItemsaldoMinimo as InputNumber<decimal?>)?.Valor,
                            Currency = "USD",
                            SecurityId = "9519",
                            DebitBranch = ctaOperativaSeleccionada?.SucursalCtaOperativa.ParseGenericVal<decimal?>(),
                            DebitProduct = "07",
                            DebitSubProduct = "0001",
                            DebitAccountType = ctaOperativaSeleccionada?.TipoCtaOperativa.ParseGenericVal<decimal?>(),
                            DebitOperativeAccount = ctaOperativaSeleccionada?.NumeroCtaOperativa.ParseGenericVal<decimal?>(),
                            CreditBranch = ctaOperativaSeleccionada?.SucursalCtaOperativa.ParseGenericVal<decimal?>(),
                            CreditProduct = "07",
                            CreditSubProduct = "0001",
                            CreditAccountType = 09,
                            CreditOperativeAccount = ctaOperativaSeleccionada?.NumeroCtaOperativa.ParseGenericVal<decimal?>(),
                            Usuario = _entity.Usuario,
                            Ip = _entity.Ip,
                            Segment = _entity.Segmento,
                            Channel = _entity.Canal,
                            SubChannel = _entity.SubCanal,
                            MapsId = _entity.IdAdhesion.Value
                        };

                        if (_entity.IdServicio == Servicio.DolarMEP || _entity.IdServicio == Servicio.DolarMEPGD30)
                        {
                            parametrosSMC.OperationType = "BUY_DOLLAR_MEP";
                            parametrosSMC.Currency = "ARS";
                            parametrosSMC.CreditAccountType = 10;
                        }

                        if (_entity.IdServicio == Servicio.DolarMEP)
                        {
                            parametrosSMC.SecurityId = "9572";
                        }

                        DependencyFactory.Resolve<ISmcDA>().InsertOrder(parametrosSMC);

                        #region Log
                        Mercados.LogTrace.LoggingHelper.Instance.Information("SE GRABO EN SMC");
                        #endregion
                    }

                    break;

                default:
                    ObtenerListaDeCuentasOperativas();
                    ObtenerListaDeCuentasTitulo();
                    //if (_entity.IdServicio == "AGDFH") ObtenerParametrosAGDFH();
                    if (_entity.Segmento.ToLower() != Segmento.BancaPrivada)
                        ObtenerListaDeCuentasPDC();
                    //CrearItems();
                    if (_entity.IdServicio != "CTR") AsignarPadreAHijo();
                    if (_entity.IdServicio != "CTR") PerfilDeInversor();
                    /*if (_entity.IdServicio != null)
                    {
                        ObtenerListaDeCuentasOperativas();
                        ObtenerListaDeCuentasTitulo();
                    }*/
                    break;

            }

            _entity.Items = _entity.Items?.OrderBy(x => (x as ControlSimple).Posicion).ToList();
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

        private void ObtenerListaDeCuentasPDC()
        {
            var listaCtasPDC = BusinessHelper.ObtenerCuentasPDC(_entity, _firma).ToArray();
            if (listaCtasPDC.Length > 0)
            {
                _entity.ListaCtasPDC = string.Join(",", listaCtasPDC.Select(x => x.NroCta));
            }
        }

        private void ObtenerListaDeCuentasOperativas()
        {
            var ListaCtasOper = BusinessHelper.ObtenerCuentasPorTipo(_entity, "OP", _firma, false);

            if (ListaCtasOper.Length > 0)
            {
                var CtasOperatSinBloq = BusinessHelper.ValidarCuentasBloquedas(ListaCtasOper);
                if (CtasOperatSinBloq.Length > 0)
                    _entity.ListaCtasOperativas = string.Join(",", CtasOperatSinBloq.Select(x => x.SucursalCta + "|" + x.NroCta + "|" + x.CodigoMoneda));
            }
        }

        private void ObtenerParametrosAGDFH()
        {

            var fondo = BusinessHelper.ObtenerInfoFondo(_entity);

            var monedaRequest = _entity.MonedaFondo;
            var controlMoneda = _entity.Items.FirstOrDefault(a => a.Config == NombreComponente.MonedaAGDFH) as InputText<string>;

            if (controlMoneda != null)
            {
                controlMoneda.Valor = fondo.Moneda;
            }
      
            var fondoRequest = _entity.CodigoDeFondo;
            var controlFondo = _entity.Items.FirstOrDefault(a => a.Config == NombreComponente.FondoAGDFH) as InputText<string>;


            if (fondoRequest != null && controlFondo != null)
            {
                controlFondo.Valor = fondo.DescFondo;
            }
        
            var operacionRequest = _entity.Operacion;
            var controlOperacion = _entity.Items.FirstOrDefault(a => a.Config == NombreComponente.OperacionAGDFH) as InputText<string>;


            if (operacionRequest != null && controlOperacion != null)
            {
                controlOperacion.Valor = operacionRequest;
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

        private void CrearFormulario()
        {
            estadoFormulario = FormularioHelper.CrearFormulario(_entity, _firma);
        }       
    }
}
