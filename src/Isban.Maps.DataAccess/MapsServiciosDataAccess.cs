
namespace Isban.Maps.DataAccess
{
    using DBRequest;
    using DBResponse;
    using Entity;
    using Entity.Base;
    using Entity.Constantes.Estructuras;
    using Entity.Controles;
    using Entity.Controles.Compuestos;
    using Entity.Controles.Customizados;
    using Entity.Extensiones;
    using Entity.Interfaces;
    using Entity.Response;
    using IDataAccess;
    using Isban.Maps.Entity.Controles.Independientes;
    using Isban.Maps.Entity.Helpers;
    using Isban.Maps.Entity.Request;
    using Isban.Mercados.LogTrace;
    using Mercados;
    using Mercados.DataAccess;
    using Mercados.DataAccess.OracleClient;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [ExcludeFromCodeCoverage]
    [ProxyProvider("DBMAPS", Owner = "MAPS")]
    public class MapsServiciosDataAccess : BaseProxy, IMapsControlesDA
    {
        public virtual void LogFormulario(FormularioResponse entity, long? CodSimuAdhe = null, long? CodAltaAdhe = null, long? CodBajaAdhe = null)
        {
            var request = entity.MapperClass<LogFormularioDbReq>(TypeMapper.IgnoreCaseSensitive);
            request.TextoJson = entity.SerializarToJson();
            request.CodSimuAdhe = CodSimuAdhe;
            request.CodAltaAdhe = CodAltaAdhe;
            request.CodBajaAdhe = CodBajaAdhe;

            this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();
        }

        public virtual long? ObtenerSiguienteFormulario(IFormulario entity, string componentes, string frmOrigen)
        {
            var request = entity.MapperClass<ObtenerSiguienteFormularioDbReq>(TypeMapper.IgnoreCaseSensitive);
            request.ListaComponentes = componentes;
            request.Origen = frmOrigen;

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.FormularioId;
        }

        public virtual decimal? ValidarCuenta(decimal? cuentaTitulo, decimal? cuentaOperativa, int? tipoCuentaOperativa, int? sucursalCuentaOperativa, FormularioResponse entity)
        {
            ValidarCuentaDbReq request = new ValidarCuentaDbReq
            {
                CuentaTitulos = cuentaTitulo,
                NroCuentaOperativa = cuentaOperativa,
                TipoCuentaOperativa = tipoCuentaOperativa,
                SucursalCuentaOperativa = sucursalCuentaOperativa,
                IdServicio = entity.IdServicio,
                Segmento = entity.Segmento,
                Ip = entity.Ip,
                Usuario = entity.Usuario
            };

            this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
            return request.TieneAdhesion;
        }

        /// <summary>
        ///   Obtiene de la base de datos la configuración del formulario.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ValorCtrlResponse[] ObtenerConfigDeFormulario(FormularioResponse entity, bool soloServicios = false)
        {
            var request = entity.MapperClass<ValorCtrlFormularioDbReq>(TypeMapper.IgnoreCaseSensitive);

            if (soloServicios)
            {
                request.IdServicio = null;
            }

            var list = this.Provider.GetCollection<ValoresFormularioDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<ValorCtrlResponse>(TypeMapper.IgnoreCaseSensitive).ToArray();

            return result;
        }

        /// <summary>
        ///  Obtiene de la base de datos la configuración del formulario.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ValorCtrlResponse[] ObtenerConfigDeFormulario(IFormulario entity)
        {
            var request = entity.MapperClass<ValorCtrlFormularioDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = this.Provider.GetCollection<ValoresFormularioDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<ValorCtrlResponse>(TypeMapper.IgnoreCaseSensitive).ToArray();

            return result;
        }

        /// <summary>
        /// Método genérico para llamar a la Base de Datos.
        /// </summary>
        /// <typeparam name="Req"></typeparam>
        /// <typeparam name="ReqDb"></typeparam>
        /// <typeparam name="RespDb"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal virtual RespDb[] ObtenerDatosDeControl<Req, ReqDb, RespDb>(Req entity)
            where ReqDb : IRequestBase, new()
            where RespDb : class, Common.Data.IContract, new()

        {
            var request = entity.MapperClass<ReqDb>(TypeMapper.IgnoreCaseSensitive);

            var list = this.Provider.GetCollection<RespDb>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.ToArray();

            return result;
        }

        /// <summary>
        /// Método genérico para obtener los datos que construyen controles y componentes.
        /// </summary>
        /// <param name="idComponente"></param>
        /// <param name="entityRequest"></param>
        /// <returns></returns>
        public virtual ValorCtrlResponse[] ObtenerDatosPorComponente(ControlSimple idComponente, FormularioResponse entity)
        {
            var componente = idComponente.Nombre;
            var componenteReq = entity.MapperClass<FormularioResponse>(TypeMapper.IgnoreCaseSensitive);
            componenteReq.IdComponente = idComponente.IdComponente;

            var entityBase = entity.MapperClass<EntityBase>(TypeMapper.IgnoreCaseSensitive);
            componenteReq.ServidorWin = Chequeo(entityBase).ServidorWin;//Obtengo el servidor desde donde se esta ejecutando.

            IEnumerable<ValorCtrlResponse> result = null;
            var obj = new object();

            lock (obj)
            {
                switch (componente)
                {
                    case NombreComponente.CuentaOperativa: //"cuenta-operativa"
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlCuentaOperativaDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.CuentasVinculadas: //"cuentas-viculadas"
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompCtasVinculadas, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.CuentaTitulo: //"cuenta-titulo"
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlCuentaTitulosDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Disclaimer:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompDisclaimerDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.ListaFondos:
                        var edbFondo = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlFondoDbReq, ValoresFormularioDbResp>(componenteReq), 8);
                        result = edbFondo.MapperEnumerable<ValorCtrlResponse>();
                        break;
                    case NombreComponente.Moneda: //"moneda"
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlMonedaDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Operacion: //"operacion"
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlOperacionDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Periodos: //"periodos"
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompPeriodosDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Producto:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompProductoDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.ListadoFondos:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompListadoFondo, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.ListadoGenerico:
                        componenteReq.Config = "estado_reproceso";
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlListadoGenericoDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.ListadoAsesoramiento:
                        componenteReq.Config = "ASESOR_INVERSIONES";
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlListadoGenericoDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.ListaPep:
                        componenteReq.Config = "PEP";
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlListadoGenericoDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Servicio:
                        var rdb = ObtenerDatosDeControl<FormularioResponse, ValorCtrlServicioDbReq, ValoresFormularioDbResp>(componenteReq);
                        result = rdb.MapperEnumerable<ValorCtrlResponse>();
                        break;
                    case NombreComponente.Alias:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlControlDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.SaldoMinimo:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlSaldoMinimoDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.FechaEjecucion:
                    case NombreComponente.Fecha:
                        result = ObtenerDatosDeControl<FormularioResponse, CompFechaDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>();
                        break;
                    case NombreComponente.Email:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlEmailDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.FechaDesde:
                        result = ObtenerDatosDeControl<FormularioResponse, ValorCompFechaDesdeDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>();
                        break;
                    case NombreComponente.FechaHasta:
                        result = ObtenerDatosDeControl<FormularioResponse, ValorCompFechaHastaDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>();
                        break;
                    case NombreComponente.DescripcionDinamica:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompDescripcionDinamicaDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.EstadoAdhesion:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompEstadoAdhesionDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.MontoSuscripcionMinimo:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlMontoMinimoDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.MontoSuscripcionMaximo:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlMontoMaximoDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Comprobante:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompComprobanteDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Legal:
                    case NombreComponente.LegalBajaPDC:
                        var edbLegal = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlLegalDbReq, ValoresFormularioDbResp>(componenteReq), 8);
                        result = edbLegal.MapperEnumerable<ValorCtrlResponse>();
                        break;
                    case NombreComponente.Vigencia:
                    case NombreComponente.FechaCompuesta:
                        var fecComp = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, CompFechaCompuestaDbReq, ValoresFormularioDbResp>(componenteReq), 8);
                        result = fecComp.MapperEnumerable<ValorCtrlResponse>();
                        break;
                    case NombreComponente.ImporteCompuesto:
                    case NombreComponente.MontoSuscripcion:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlMontoSuscripcionDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Frecuencia:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompListaFrecuencia, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.FondoCompuesto:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCtrlFondoCompuestoDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.LegalAgendamiento:

                        result = ObtenerDatosDeControl<FormularioResponse, ObtenerLegalesFondosAgdDbReq, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>();
                        break;
                    case NombreComponente.ListaFrecuencia:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompListaFrecuencia, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.ListaDias:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompListaDias, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.Numeros:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompNumeros, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    case NombreComponente.FechaFrecuencia:
                        result = Cache.Get($"ObtenerDatosDeControl{componenteReq.IdComponente}{componenteReq.IdServicio}", () => ObtenerDatosDeControl<FormularioResponse, ValorCompFechaFrecuencia, ValoresFormularioDbResp>(componenteReq).MapperEnumerable<ValorCtrlResponse>(), 8);
                        break;
                    default:
                        break;
                }
            }
            return result?.ToArray();
        }

        /// <summary>
        /// Método para la consulta de Adhesiones
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual ValorConsDeAdhesionesResp[] ObtenerConsultaDeAdhesiones(FormularioResponse entity, string cuentasOperativas = null, string cuentasTitulos = null)
        {
            var request = entity.MapperClass<ServConsAdhesionesDbReq>(TypeMapper.IgnoreCaseSensitive);

            if (!string.IsNullOrEmpty(cuentasOperativas))
                request.CuentasOperativas = cuentasOperativas;

            if (!string.IsNullOrEmpty(cuentasTitulos))
                request.CuentasTitulos = cuentasTitulos;

            var list = this.Provider.GetCollection<ValorConsDeAdhesionesDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<ValorConsDeAdhesionesResp>(TypeMapper.IgnoreCaseSensitive).ToArray();

            return result;
        }

        public virtual long? BajaAdhesion(FormularioResponse entity)
        {
            var request = entity.MapperClass<ServBajaAdhesionDbReq>(TypeMapper.IgnoreCaseSensitive);

            this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.IdAdhesionBaja;
        }

        public virtual long? SimulacionBajaAdhesion(FormularioResponse entity)
        {
            var request = entity.MapperClass<ServBajaAdhesionSimulacionDbReq>(TypeMapper.IgnoreCaseSensitive);

            this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.IdSimulacion;
        }

        public virtual long? RegistrarOrden(RegistraOrdenRequest entity)
        {
            var request = entity.MapperClass<RegistraOrdenDbReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.IdSolicitudOrdenes;
        }

        /// <summary>
        /// Actualiza el comprobante en el json
        /// </summary>
        /// <param name="entity"></param>
        public virtual void ActualizarComprobanteAJson(FormularioResponse entity)
        {
            var request = entity.MapperClass<ServConsAdhesionActualizarJsonReq>(TypeMapper.IgnoreCaseSensitive);
            request.TextoJson = entity.SerializarToJson();

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();
        }

        /// <summary>
        /// Graba la confirmación de la adhesión en la BD
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual decimal ConfirmarAdhesion(FormularioResponse entity, string TextoDisclaimer = null, string CuentaTitulos = null)
        {
            var request = entity.MapperClass<ServConsAdhesionConfimacionReq>(TypeMapper.IgnoreCaseSensitive);
            request.TextoJson = entity.SerializarToJson();
            request.TextoDisclaimer = TextoDisclaimer;

            if (!string.IsNullOrEmpty(CuentaTitulos))
                request.CuentaTitulos = long.Parse(CuentaTitulos);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.Comprobante;
        }

        /// <summary>
        /// Genera el regitro de simulación y su ID.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cuentaTitulo"></param>
        /// <returns>Devuelve el ID de Simulación de la Adhesión.</returns>
        public virtual long? SimularAdhesion(FormularioResponse entity, long? cuentaTitulo = null)
        {
            ServConsAdhesionSimulacionReq request = null;

            if (entity.IdServicio == Servicio.Agendamiento || entity.IdServicio == Servicio.AgendamientoFH)
            {
                request = GenerarRequestAGD(entity, cuentaTitulo);
            }
            else
            {
                request = GenerarRequest(entity, cuentaTitulo);
            }


            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.IdSimulacion;
        }

        private ServConsAdhesionSimulacionReq GenerarRequest(FormularioResponse entity, long? cuentaTitulo)
        {
            var request = entity.MapperClass<ServConsAdhesionSimulacionReq>(TypeMapper.IgnoreCaseSensitive);
            request.TextoJson = entity.SerializarToJson();

            var ctaOperativa = entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
            var ctaTitulo = entity.Items.GetControlMaps<Lista<ItemCuentaTitulos<string>>>(NombreComponente.CuentaTitulo);
            var moneda = entity.Items.GetControlMaps<Lista<ItemMoneda<string>>>(NombreComponente.Moneda);

            var ctaOperativaSeleccionada = ctaOperativa?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
            var ctaTituloSeleccionada = ctaTitulo?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
            var monedaSeleccionada = moneda?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
            request.SucCtaOper = ctaOperativaSeleccionada?.SucursalCtaOperativa.ParseGenericVal<decimal?>();
            request.NroCtaOper = ctaOperativaSeleccionada?.NumeroCtaOperativa.ParseGenericVal<decimal?>();
            request.TipoCuentaOperativa = ctaOperativaSeleccionada?.TipoCtaOperativa.ParseGenericVal<int?>();
            request.SaldoCuentaAntes = ctaOperativaSeleccionada?.SaldoCta;
            request.CodigoMoneda = monedaSeleccionada?.Valor;
            request.CuentaTitulo = ctaTituloSeleccionada?.NumeroCtaTitulo == null ? ctaTituloSeleccionada?.Valor.ParseGenericVal<decimal?>() : ctaTituloSeleccionada?.NumeroCtaTitulo.ParseGenericVal<decimal?>();
            request.CodProducto = ctaOperativaSeleccionada?.Producto;

            if (entity.Segmento.ToLower() == Segmento.BancaPrivada && entity.Id != Servicio.Repatriacion)
                request.CuentaTitulo = cuentaTitulo?.ParseGenericVal<decimal?>();

            var vigencia = entity.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia);
            var saldo = entity.Items.GetControlMaps<ImporteCompuesto>(NombreComponente.MontoSuscripcion);
            var fondo = entity.Items.GetControlMaps<Lista<Item<string>>>(NombreComponente.ListadoFondos);

            var ItemsaldoMinimo = entity.Items.Where(x => (x as ControlSimple).Nombre == NombreComponente.SaldoMinimo).FirstOrDefault();
            if (ItemsaldoMinimo != null)
                request.SaldoMinDejarCta = (ItemsaldoMinimo as InputNumber<decimal?>)?.Valor;

            request.CodigoFondo = fondo?.Items.Where(x => x.Seleccionado == true).FirstOrDefault()?.Valor;

            request.CodEspecie = null;
            request.FecVigenciaDesde = vigencia?.Items.GetControlMaps<Fecha>(NombreComponente.FechaDesde).Valor;

            request.FecVigenciaHasta = vigencia?.Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta).Valor;
            request.SaldoMinOperacion = saldo?.Items.Where(x => x.Nombre.ToLower().Equals(NombreComponente.MontoSuscripcionMinimo)).FirstOrDefault()?.Valor;
            request.SaldoMaxOperacion = saldo?.Items.Where(x => x.Nombre.ToLower().Equals(NombreComponente.MontoSuscripcionMaximo)).FirstOrDefault()?.Valor;

            if (entity.IdServicio == Servicio.Repatriacion)
            {
                request.CodigoMoneda = ctaOperativaSeleccionada?.CodigoMoneda;
            }

            if (entity.IdServicio == Servicio.AltaCuenta)
            {
                request.CodigoMoneda = "ARS";
                request.CodigoMonedaDolares = "USD";

                var ctaOperativaPesos = entity.Items?.FirstOrDefault(i => i.Id == "cuenta-operativa-1") as Lista<ItemCuentaOperativa<string>>;
                var ctaOperativaDolares = entity.Items?.FirstOrDefault(i => i.Id == "cuenta-operativa-2") as Lista<ItemCuentaOperativa<string>>;

                var ctaOperativaPesosSeleccionada = ctaOperativaPesos?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
                var ctaOperativaDolaresSeleccionada = ctaOperativaDolares?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();

                request.SucCtaOper = ctaOperativaPesosSeleccionada?.SucursalCtaOperativa.ParseGenericVal<decimal?>();
                request.NroCtaOper = ctaOperativaPesosSeleccionada?.NumeroCtaOperativa.ParseGenericVal<decimal?>();
                request.TipoCuentaOperativa = ctaOperativaPesosSeleccionada?.TipoCtaOperativa.ParseGenericVal<int?>();

                request.SucCtaOperDolares = ctaOperativaDolaresSeleccionada?.SucursalCtaOperativa.ParseGenericVal<decimal?>();
                request.NroCtaOperDolares = ctaOperativaDolaresSeleccionada?.NumeroCtaOperativa.ParseGenericVal<decimal?>();
                request.TipoCuentaOperativaDolares = ctaOperativaDolaresSeleccionada?.TipoCtaOperativa.ParseGenericVal<int?>();
            }


            return request;
        }

        private ServConsAdhesionSimulacionReq GenerarRequestAGD(FormularioResponse entity, long? cuentaTitulo)
        {
            var request = entity.MapperClass<ServConsAdhesionSimulacionReq>(TypeMapper.IgnoreCaseSensitive);
            request.TextoJson = entity.SerializarToJson();

            var ctaOperativa = entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
            var ctaTitulo = entity.Items.GetControlMaps<Lista<ItemCuentaTitulos<string>>>(NombreComponente.CuentaTitulo);
            var moneda = entity.Items.GetControlMaps<Lista<ItemMoneda<string>>>(NombreComponente.Moneda);
            var operacion = entity.Items.GetControlMaps<Lista<Item<string>>>(NombreComponente.Operacion);

            var ctaOperativaSeleccionada = ctaOperativa?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
            var ctaTituloSeleccionada = ctaTitulo?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
            var monedaSeleccionada = moneda?.Items.Where(x => x.Seleccionado == true).FirstOrDefault();
            request.SucCtaOper = ctaOperativaSeleccionada?.SucursalCtaOperativa.ParseGenericVal<decimal?>();
            request.NroCtaOper = ctaOperativaSeleccionada?.NumeroCtaOperativa.ParseGenericVal<decimal?>();
            request.TipoCuentaOperativa = ctaOperativaSeleccionada?.TipoCtaOperativa.ParseGenericVal<int?>();
            request.SaldoEnviado = entity.Items.GetControlMaps<InputNumber<decimal?>>(NombreComponente.SaldoMinimo)?.Valor;
            request.CodigoMoneda = monedaSeleccionada?.Valor;
            request.CuentaTitulo = ctaTituloSeleccionada?.NumeroCtaTitulo.ParseGenericVal<decimal?>();
            request.CodProducto = ctaOperativaSeleccionada?.Producto;
            request.Operacion = operacion?.Items.Where(x => x.Seleccionado == true).FirstOrDefault().Valor;

            if (entity.Segmento.ToLower() == Segmento.BancaPrivada)
                request.CuentaTitulo = cuentaTitulo?.ParseGenericVal<decimal?>();

            var vigencia = entity.Items.GetControlMaps<FechaCompuesta>(NombreComponente.Vigencia);
            var fondoCompuesto = entity.Items.GetControlMaps<Lista<ItemGrupoAgd>>(NombreComponente.ListaFondos);

            var ItemsaldoMinimo = entity.Items.Where(x => (x as ControlSimple).Nombre == NombreComponente.SaldoMinimo).FirstOrDefault();
            if (ItemsaldoMinimo != null)
                request.SaldoMinDejarCta = (ItemsaldoMinimo as InputNumber<decimal?>)?.Valor;

            request.CodigoFondo = fondoCompuesto?.Items.SelectMany(x => x.Items.Where(y => y.Seleccionado)).FirstOrDefault()?.Valor;

            request.CodEspecie = null;

            var a = entity.Items.Where(x => x.Nombre == NombreComponente.Fecha)?.Where(y => string.Compare(y.Id, "fecha-2", true) == 0).FirstOrDefault();

            if (vigencia != null)
            {
                request.FecVigenciaDesde = vigencia?.Items.GetControlMaps<Fecha>(NombreComponente.FechaDesde).Valor;
                request.FecVigenciaHasta = vigencia?.Items.GetControlMaps<Fecha>(NombreComponente.FechaHasta).Valor;
            }
            else
            {
                request.FecVigenciaDesde = request.FecVigenciaHasta = (a as Fecha)?.Valor;
            }

            if (a != null)
            {
                request.FechaDeEjecucion = (a as Fecha)?.Valor;
            }
            else
            {
                request.FechaDeEjecucion = DateTime.Now.AddDays(1);
            }

            if (entity.IdServicio == Servicio.AgendamientoFH)
            {
                request.CodigoFondo = entity.Config?.Split('|')?.Where(x => x.Contains("Fondo")).FirstOrDefault()?.Split(':')[1]; ;
                request.CodigoMoneda = entity.Config?.Split('|')?.Where(x => x.Contains("MonedaFondo")).FirstOrDefault()?.Split(':')[1];
                request.Operacion = entity.Config?.Split('|')?.Where(x => x.Contains("Operacion")).FirstOrDefault()?.Split(':')[1];
                request.FechaDeEjecucion = request.FechaDeEjecucion.Value.Date;
                request.FecVigenciaDesde = request.FecVigenciaDesde.Value.Date;
                request.FecVigenciaHasta = request.FecVigenciaHasta.Value.Date;
            }

            return request;
        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            var request = entity.MapperClass<ChequeoAccesoMAPSReq>(TypeMapper.IgnoreCaseSensitive);
            var li = Provider.GetCollection<ChequeoAccesoResp>(CommandType.StoredProcedure, request);
            if (li.Any())
                return li.First().MapperClass<ChequeoAcceso>(TypeMapper.IgnoreCaseSensitive);
            throw new DBCodeException(-1, "Error MAPS");
        }

        public string GetInfoDB(long id)
        {
            BaseProxySeguridad seg = new BaseProxySeguridad();
            UsuarioPasswordBaseClaves usuario = seg.ObtenerUsuarioBaseDeClaves(id);
            return Encrypt.EncryptToBase64String(string.Format("{0}||{1}", usuario.Usuario, usuario.Password));
        }

        public virtual long ObtenerIdComponente(string componente, string usuario, string ip)
        {
            ObtenerIdComponenteDbReq request = new ObtenerIdComponenteDbReq
            {
                Nombre = componente,
                Ip = ip,
                Usuario = usuario
            };

            this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
            return request.IdComponente.Value;
        }

        public virtual string GetTextoDisclaimer(FormularioResponse frm)
        {
            string textoDiscl = "";
            var itemDisclaimer = frm.Items.GetControlMaps<Lista<ItemDisclaimer<string>>>(NombreComponente.Disclaimer);
            if (itemDisclaimer != null)
            {
                foreach (var itemD in itemDisclaimer.Items)
                {
                    if (!String.IsNullOrEmpty(itemD.Valor))
                    {
                        textoDiscl = itemD.Valor;
                    }

                }
            }
            else
                textoDiscl = "NO POSEE DISCLAIMER";

            return textoDiscl;
        }

        public virtual string ObtenerOrigen(FormularioResponse entity)
        {
            var request = entity.MapperClass<ObtenerOrigenDbReq>(TypeMapper.IgnoreCaseSensitive);

            this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.FrmOrigen;
        }

        /// <summary>
        /// Obtiene el Id del primer formulario para comenzar el flujo de carga.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Id del formulario de inicio del flujo</returns>
        public virtual long? ObtenerFormularioIdOrigenFlujo(FormularioResponse entity)
        {
            var request = entity.MapperClass<ObtenerFormularioIdOrigenFlujo>(TypeMapper.IgnoreCaseSensitive);

            this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.FormularioId;
        }

        /// <summary>
        /// Obtiene el Id del primer formulario para comenzar el flujo de carga.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Id del formulario de inicio del flujo</returns>
        public virtual long? ObtenerFormularioIdOrigenFlujoBaja(FormularioResponse entity)
        {
            var request = entity.MapperClass<ObtenerFormularioIdOrigenFlujoBaja>(TypeMapper.IgnoreCaseSensitive);

            this.Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.FormularioId;
        }

        /// <summary>
        /// Obtiene valores parametrizados en la BD.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string ObtenerValorParametrizado(ConsultaParametrizacionReq entity)
        {
            var request = entity.MapperClass<ConsultaParametrizacionDbReq>(TypeMapper.IgnoreCaseSensitive);

            var result = this.Provider.GetCollection<ConsultaParametrizacionDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            //Devuelve un cursor y solo interesa el campo valor
            return result?.ToArray().FirstOrDefault()?.Valor;
        }

        public virtual ConsultaOrigenResponse ConsultaOrigen(FormularioResponse entity)
        {
            var request = entity.MapperClass<ConsultaOrigenDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = this.Provider.GetCollection<ConsultaOrigenDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<ConsultaOrigenResponse>(TypeMapper.IgnoreCaseSensitive);

            return result.ToList().FirstOrDefault();
        }

        /// <summary>
        /// Obtiene el formulario o paso que le sigue.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Clase </returns>
        public virtual PasoWizardRes ConsultaPasoSiguiente(PasoWizardReq entity)
        {
            if (entity.IdServicio == null)
            {
                entity.FormularioId = 0;//Pido el inicial, la lista de servicios. 
            }

            var request = entity.MapperClass<ConsultarPasoSiguiente>(TypeMapper.IgnoreCaseSensitive);

            var response = this.Provider.GetFirst<ConsultaPasoAntSigDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            return response.MapperClass<PasoWizardRes>(TypeMapper.IgnoreCaseSensitive);
        }

        public virtual PasoWizardRes ConsultaPasoSiguienteBaja(PasoWizardReq entity)
        {

            var request = entity.MapperClass<ConsultarPasoSiguienteBaja>(TypeMapper.IgnoreCaseSensitive);
            request.CodAltaAdhesion = entity.IdAdhesion;

            var response = this.Provider.GetFirst<ConsultaPasoAntSigDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            return response.MapperClass<PasoWizardRes>(TypeMapper.IgnoreCaseSensitive);
        }

        public PasoWizardRes ConsultaPasoAnterior(PasoWizardReq entity)
        {
            var request = entity.MapperClass<ConsultaPasoAnteriorAdhDbReq>(TypeMapper.IgnoreCaseSensitive);

            var response = Provider.GetCollection<ConsultaPasoAntSigDbResp>(CommandType.StoredProcedure, request).FirstOrDefault();

            request.CheckError();

            return response.MapperClass<PasoWizardRes>(TypeMapper.IgnoreCaseSensitive);
        }

        public ObtenerPasoResponse ObtenerPaso(ObtenerPasoReq entity)
        {
            var request = entity.MapperClass<ObtenerPasoAdhDbReq>(TypeMapper.IgnoreCaseSensitive);

            var response = Provider.GetCollection<ObtenerPasoDbResp>(CommandType.StoredProcedure, request).FirstOrDefault();

            request.CheckError();

            return response.MapperClass<ObtenerPasoResponse>(TypeMapper.IgnoreCaseSensitive);
        }

        public long? RegistrarPasoWizard(RegistrarPasoWizard entity)
        {
            var request = entity.MapperClass<RegistrarPasoWizardDbReq>(TypeMapper.IgnoreCaseSensitive);

            var response = Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.Id;
        }

        public void GuardarConfirmacionJson(GuardarConfirmacionJsonReq entity)
        {
            var request = entity.MapperClass<GuardarConfirmacionJsonDbReq>(TypeMapper.IgnoreCaseSensitive);

            var response = Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();
        }

        /// <summary>
        /// Obteners the usuario racf.
        /// </summary>
        /// <returns></returns>
        public virtual UsuarioRacf ObtenerUsuarioRacf()
        {
            var request = new ConsultaParametrizacionDbReq { NomParametro = "ID_RACF_USER", CodigoSistema = "MAPS", Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault };

            var listResult = Provider.GetFirst<ConsultaParametrizacionDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var id = listResult.MapperClass<Parametro>();

            BaseProxySeguridad dbseg = new BaseProxySeguridad();

            var usuarioBaseDeClaves = dbseg.ObtenerUsuarioBaseDeClaves(Convert.ToInt64(id.Valor));

            return usuarioBaseDeClaves.MapperClass<UsuarioRacf>();
        }

        public ObtenerFormAdhesionesResp ObtenerFormAdhesiones(ObtenerFormAdhesionesReq entity)
        {
            var request = entity.MapperClass<ObtenerFormAdhesionesDbReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.MapperClass<ObtenerFormAdhesionesResp>(TypeMapper.IgnoreCaseSensitive);
        }

        public ObtenerIdAdhesionResp ObtenerIdAdhesion(ObtenerIdAdhesionReq entity)
        {
            var request = entity.MapperClass<ObtenerIdAdhesionDbReq>(TypeMapper.IgnoreCaseSensitive);
            var result = Provider.GetCollection<ObtenerIdAdhesionDbResp>(CommandType.StoredProcedure, request).FirstOrDefault();
            request.CheckError();

            var output = result.MapperClass<ObtenerIdAdhesionResp>(TypeMapper.IgnoreCaseSensitive);
            output.IdCuentaPdc = entity.IdCuentaPdc;
            return output;
        }

        public List<ConsultarDatosSimulacionConfirmacionResp> ObtenerDatosDeSimulacion(FormularioResponse _entity)
        {
            var request = _entity.MapperClass<ConsultarDatosSimulacionDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.GetCollection<ConsultarDatosSimulacionDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            return a.MapperEnumerable<ConsultarDatosSimulacionConfirmacionResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public List<ConsultarDatosSimulacionConfirmacionResp> ObtenerDatosDeConfirmacion(FormularioResponse _entity)
        {
            var request = _entity.MapperClass<ConsultarDatosConfirmacionDbReq>(TypeMapper.IgnoreCaseSensitive);
            var a = this.Provider.GetCollection<ConsultarDatosConfirmacionDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            return a.MapperEnumerable<ConsultarDatosSimulacionConfirmacionResp>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public ValorCtrlResponse[] ObtenerTodosLosPasos(MapsBase datos)
        {
            var request = datos.MapperClass<TotalDePasosDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = this.Provider.GetCollection<ValoresFormularioDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperEnumerable<ValorCtrlResponse>(TypeMapper.IgnoreCaseSensitive).ToArray();

            return result;
        }

        /// <summary>
        /// Obtiene los detalles deconfiguración de importe para el fondo seleccionado.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DetalleDeFondoResp ObtenerDetalleDelFondo(DetalleDeFondoReq entity)
        {

            var request = entity.MapperClass<DetalleDeFondoDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = this.Provider.GetFirst<DetalleDeFondoDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperClass<DetalleDeFondoResp>(TypeMapper.IgnoreCaseSensitive);

            return result;

        }

        public DetalleDeFondoResp ObtenerInfoFondo(InfoFondoReq entity)
        {

            var request = entity.MapperClass<InfoFondoDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = this.Provider.GetFirst<DetalleDeFondoDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var result = list.MapperClass<DetalleDeFondoResp>(TypeMapper.IgnoreCaseSensitive);

            return result;

        }

        public OperacionesDisponiblesFondosResp ObtenerOperacionesFondo(OperacionesDisponiblesFondosReq entity)
        {
            var request = entity.MapperClass<OperacionesDisponiblesFondosDbReq>(TypeMapper.IgnoreCaseSensitive);

            var list = this.Provider.GetCollection<OperacionesDisponiblesFondosDbResp>(CommandType.StoredProcedure, request).FirstOrDefault();

            request.CheckError();

            var result = list.MapperClass<OperacionesDisponiblesFondosResp>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }

        public virtual List<CuentaAdheridaRTF> ConsultaArchivosRTF(RTFWorkflowOnDemandReq entity)
        {
            var request = entity.MapperClass<ConsultaArchivoRTFDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<ConsultaArchivoRTFDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<CuentaAdheridaRTF>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual List<ArchivoRTF> ObtenerArchivoRTF(RTFWorkflowOnDemandReq entity)
        {
            var request = entity.MapperClass<ConsultaArchivoRTFDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<ConsultaArchivoRTFDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<ArchivoRTF>(TypeMapper.IgnoreCaseSensitive).ToList();
        }

        public virtual UltimoPeriodoRTF ConsultaUltimoPeriodoRTF(RTFWorkflowOnDemandReq entity)
        {
            var request = entity.MapperClass<ConsultaUltimoPeriodoRTFReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();

            return request.MapperClass<UltimoPeriodoRTF>(TypeMapper.IgnoreCaseSensitive);
        }

        public virtual ConsultaCuentasMEP ObtenerCuentasMEP(string nup)
        {
            var request = new ConsultaCuentasMEPDbReq() { Nup = nup, Usuario = "AUTO", Ip = "1.1.1.1" };

            var dbResult = Provider.GetCollection<ConsultaCuentasMEPDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();
            var result = dbResult.MapperEnumerable<ConsultaCuentasMEP>(TypeMapper.IgnoreCaseSensitive).FirstOrDefault();
            return result;
        }
        public virtual ConsultaRestriccionAdhesion ObtenerRestriccionAdhesion(ValidaRestriccionMEPRequest entity)
        {
            var request = entity.MapperClass<ConsultaRestriccionAdhesionDBReq>(TypeMapper.IgnoreCaseSensitive);
            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);
            request.CheckError();
            return new ConsultaRestriccionAdhesion() { 
                Mensaje= request.Mensaje,
                Estado = request.Estado
            };
        }

        public virtual List<ConsultaFondosAGDResponse> ConsultaFondosAGD(ConsultaFondosAGDRequest entity)
        {
            var request = entity.MapperClass<ConsultaFondosAGDDbReq>(TypeMapper.IgnoreCaseSensitive);
            var dbResul = Provider.GetCollection<ConsultaFondosAGDDbResp>(CommandType.StoredProcedure, request);
            request.CheckError();

            return dbResul.MapperEnumerable<ConsultaFondosAGDResponse>(TypeMapper.IgnoreCaseSensitive).ToList();
        }
    }
}
