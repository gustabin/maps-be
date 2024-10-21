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
    public class ConfigFormularioCTR : ICrearComponente
    {
        private static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        private static List<string> propExcluidas = new List<string> { "items", "nup", "segmento", "canal", "subcanal", "idsimulacion", "cabecera" };

        private FormularioResponse _entity;
        private DatoFirmaMaps _firma;
        private string estadoFormulario;
        ValorCtrlResponse[] _datosForm;

        public ConfigFormularioCTR(FormularioResponse formulario)
        {
            _entity = formulario;
        }

        public ConfigFormularioCTR(FormularioResponse formulario, DatoFirmaMaps firma)
        {
            _entity = formulario;
            _firma = firma;
        }

        public ConfigFormularioCTR(FormularioResponse formulario, DatoFirmaMaps firma, ValorCtrlResponse[] datos)
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

                    //if (_entity.Segmento.ToLower() == Segmento.BancaPrivada)
                    //{
                    //    var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
                    //    {
                    //        Nup = _entity.Nup.ParseGenericVal<long?>(),
                    //        CuentaBp = 0
                    //    });

                    //    //ver si se puede hacer una busqueda recursiva dentro del json para evitar linq
                    //    var ctaOperativa = _entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
                    //    var ctaOperativaSeleccionada = ctaOperativa.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
                    //    cuentaBp = BusinessHelper.ValidarCuentas(atisResp, ctaOperativaSeleccionada.NumeroCtaOperativa.ParseGenericVal<long>());
                    //}

                    //obtener los items de los formulario y agregarlos al de simulacion

                    _entity.IdSimulacion = daSim.SimularAdhesion(_entity, cuentaBp).ParseGenericVal<long?>();
                    _entity.Bloqueado = true;
                    _entity.Validado = true;
                    _entity.Titulo = FormularioHelper.ObtenerTituloSimulación(_entity.IdServicio);

                    break;
                case TipoEstadoFormulario.Confirmacion:

                    var daConf = DependencyFactory.Resolve<IMapsControlesDA>();
                    var itemsConf = daConf.ObtenerDatosDeConfirmacion(_entity);

                    _entity.Items.AddRange(ObtenerItemsDeFormulario(itemsConf));

                    //var cuentaTitulos = CreacionCuentaTitulos();

                    _entity.Comprobante = daConf.ConfirmarAdhesion(_entity, string.Empty,null).ToString();
                    _entity.IdAdhesion = _entity.Comprobante.ParseGenericVal<long?>();
                    _entity.Titulo = FormularioHelper.ObtenerTituloSimulación(_entity.IdServicio);

                    CrearRelacionCuentas();

                    daConf.ActualizarComprobanteAJson(_entity);

                    break;

                default:
                    ObtenerListaDeCuentasOperativas();
                    ObtenerListaDeCuentasTitulo();
 
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
                {
                    _entity.ListaCtasOperativas = string.Join(",", CtasOperatSinBloq.Where(a => a.DescripcionTipoCta != "REPATRIACION").Select(x => x.SucursalCta + "|" + x.NroCta + "|" + x.CodigoMoneda));
                    _entity.ListaCtasRepatriacion = string.Join(",", CtasOperatSinBloq.Where(a => a.DescripcionTipoCta == "REPATRIACION").Select(x => x.SucursalCta + "|" + x.NroCta + "|" + x.CodigoMoneda));
                }
            }
        }
        private void CrearFormulario()
        {
            estadoFormulario = FormularioHelper.CrearFormulario(_entity, _firma);

            if (_entity.IdServicio == Servicio.AltaCuenta)
            {
                var cuentaOperativaPesos = _entity.Items?.FirstOrDefault(i => i.Id == "cuenta-operativa-1") as Lista<ItemCuentaOperativa<string>>;

                if(cuentaOperativaPesos != null)
                    cuentaOperativaPesos.Items = cuentaOperativaPesos.Items?.Where(c => c.CodigoMoneda == "ARS")?.ToList();

                var cuentaOperativaUSD = _entity.Items?.FirstOrDefault(i => i.Id == "cuenta-operativa-2") as Lista<ItemCuentaOperativa<string>>;

                if (cuentaOperativaUSD != null)
                    cuentaOperativaUSD.Items = cuentaOperativaUSD.Items?.Where(c => c.CodigoMoneda == "USD")?.ToList();
            }
        }

        private string CreacionCuentaTitulos()
        {
            var ctaOperativa = _entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
            var ctaOperativaSeleccionada = ctaOperativa?.Items.Where(c=>c.Seleccionado == true).FirstOrDefault();

           return BusinessHelper.CreacionCuentaTitulosRepatriacion(_entity, ctaOperativaSeleccionada);
        }

        private void CrearRelacionCuentas()
        {
            IOpicsDA opic = DependencyFactory.Resolve<IOpicsDA>();
           
            var alias = _entity.Items.GetControlMaps<InputText<string>>(NombreComponente.Alias);
            var descripcion = _entity.Items.GetControlMaps<DescripcionDinamica<string>>(NombreComponente.DescripcionDinamica);

            if (_entity.IdServicio == Servicio.Repatriacion)
            {
                var ctaOperativa = _entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
                var ctaOperativaSeleccionada = ctaOperativa?.Items.Where(c => c.Seleccionado == true).FirstOrDefault();

                var vinculacion = new VincularCuentasActivasReq
                {
                    Nup = _entity.Nup,
                    CuentaTitulos = 1,
                    Producto = "60",
                    SubProducto = "0000",
                    Alias = alias?.Valor,
                    Segmento = _entity.Segmento,
                    Descripcion = descripcion?.Valor,
                    CuentaOperativa = long.Parse(ctaOperativaSeleccionada?.NumeroCtaOperativa),
                    Sucursal = long.Parse(ctaOperativaSeleccionada?.SucursalCtaOperativa),
                    TipoCuenta = long.Parse(ctaOperativaSeleccionada?.TipoCtaOperativa),
                    CodMoneda = ctaOperativaSeleccionada?.CodigoMoneda,
                    Usuario = _entity.Usuario,
                    Ip = _entity.Ip
                };

                var reqSecurity = _firma.MapperClass<RequestSecurity<VincularCuentasActivasReq>>(TypeMapper.IgnoreCaseSensitive);

                reqSecurity.Datos = vinculacion;
                reqSecurity.Canal = _entity.Canal;
                reqSecurity.SubCanal = _entity.SubCanal;

                DependencyFactory.Resolve<IServiceWebApiClient>().VincularCuentasActivas(reqSecurity);
            }


            if (_entity.IdServicio == Servicio.AltaCuenta)
            {
                var ctaOperativaPesos = _entity.Items?.FirstOrDefault(i => i.Id == "cuenta-operativa-1") as Lista<ItemCuentaOperativa<string>>;
                var ctaOperativaPesosSeleccionada = ctaOperativaPesos?.Items.Where(c => c.Seleccionado == true).FirstOrDefault();
                var ctaOperativaDolares = _entity.Items?.FirstOrDefault(i => i.Id == "cuenta-operativa-2") as Lista<ItemCuentaOperativa<string>>;
                var ctaOperativaDolaresSeleccionada = ctaOperativaDolares?.Items.Where(c => c.Seleccionado == true).FirstOrDefault();

                var reqCtas = _entity.MapperClass<GetCuentas>(TypeMapper.IgnoreCaseSensitive);
                reqCtas.TipoBusqueda = "N";
                reqCtas.Segmento = _entity.Segmento;
                reqCtas.CuentasRespuesta = "OP";
                reqCtas.Cabecera.H_Nup = _entity.Nup;
                reqCtas.DatoConsulta = _entity.Nup;

                var reqSecurityGetCuenta = _firma.MapperClass<RequestSecurity<GetCuentas>>(TypeMapper.IgnoreCaseSensitive);

                reqSecurityGetCuenta.Datos = reqCtas;

                var cliente = DependencyFactory.Resolve<IServiceWebApiClient>().GetCuentas(reqSecurityGetCuenta)?.FirstOrDefault();
                var cuentaBloqueada = cliente?.Cuentas?.FirstOrDefault(c => c.NroCta.PadLeft(12,'0') == ctaOperativaPesosSeleccionada?.NumeroCtaOperativa.PadLeft(12,'0'))?.CuentaBloqueada;

                var vinculacion = new InsertarCuentasVinculadasReq
                {
                    Nup = _entity.Nup,
                    CuentaTitulos = 1,
                    Producto = "60",
                    SubProducto = "0000",
                    Alias = alias?.Valor,
                    Segmento = _entity.Segmento,
                    Descripcion = descripcion?.Valor,
                    CuentaOperativa = long.Parse(ctaOperativaPesosSeleccionada?.NumeroCtaOperativa),
                    Sucursal = long.Parse(ctaOperativaPesosSeleccionada?.SucursalCtaOperativa),
                    TipoCuenta = long.Parse(ctaOperativaPesosSeleccionada?.TipoCtaOperativa),
                    CodMoneda = ctaOperativaPesosSeleccionada?.CodigoMoneda,
                    Moneda = ctaOperativaPesosSeleccionada?.CodigoMoneda,
                    Usuario = _entity.Usuario,
                    Ip = _entity.Ip,
                    Nombre = cliente?.Nombre,
                    Apellido = cliente?.Apellido,
                    TipoDocumento = cliente?.CodTipoDocumento,
                    NumeroDocumento = cliente?.NumeroDocumento,
                    DescripcionDocumento = cliente?.TipoDocumento,
                    TipoPersona = cliente?.TipoPersona,
                    ProductoOperativa = ctaOperativaPesosSeleccionada?.Producto,
                    SubProductoOperativa = ctaOperativaPesosSeleccionada?.SubProducto,
                    CtaBloqueada = cuentaBloqueada,
                    Estado = cuentaBloqueada == "N" ? "NORMAL" : "BLOQUEADA",
                    CuentaOperativaDolares = long.TryParse(ctaOperativaDolaresSeleccionada?.NumeroCtaOperativa, out long resNumeroCuentaOperativaUSD) ? resNumeroCuentaOperativaUSD : (long?)null,
                    SucursalCtaDolares = long.TryParse(ctaOperativaDolaresSeleccionada?.SucursalCtaOperativa, out long resSucursalCtaDolares) ? resSucursalCtaDolares : (long?)null,
                    TipoCuentaDolares = long.TryParse(ctaOperativaDolaresSeleccionada?.TipoCtaOperativa, out long resTipoCuentaDolares) ? resTipoCuentaDolares : (long?)null,
                    CodMonedaDolares = ctaOperativaDolaresSeleccionada?.CodigoMoneda,
                    CodAltaAdhesion = _entity.IdAdhesion
                };

                var reqSecurity = _firma.MapperClass<RequestSecurity<InsertarCuentasVinculadasReq>>(TypeMapper.IgnoreCaseSensitive);

                reqSecurity.Datos = vinculacion;
                reqSecurity.Canal = _entity.Canal;
                reqSecurity.SubCanal = _entity.SubCanal;

                DependencyFactory.Resolve<IServiceWebApiClient>().InsertarCuentasVinculadas(reqSecurity);
            }


        }
    }
}
