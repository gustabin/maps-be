
namespace Isban.Maps.Bussiness
{
    using Business.Componente.Factory;
    using Business.Factory;
    using Entity.Base;
    using Entity.Controles.Compuestos;
    using IBussiness;
    using Isban.Maps.Bussiness.Factory;
    using Isban.Maps.Entity.Constantes.Estructuras;
    using Isban.Maps.Entity.Controles;
    using Isban.Maps.Entity.Controles.Customizados;
    using Isban.Maps.Entity.Controles.Independientes;
    using Isban.Maps.Entity.Extensiones;
    using Isban.Maps.Entity.Request;
    using Isban.Maps.Entity.Response;
    using Isban.Maps.IDataAccess;
    using Isban.Mercados;
    using Isban.Mercados.Service.InOut;
    using Mercados.LogTrace;
    using Mercados.UnityInject;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class BusinessHelper
    {
        public static ClienteCuentaDDC[] ObtenerCuentasPorTipo(FormularioResponse entity, string tipoCta, DatoFirmaMaps firma, bool incluirTitulares)
        {
            entity.Cabecera = ObtenerCabeceraMock();
            ClienteDDC[] cliente;

            var reqCtas = entity.MapperClass<GetCuentas>(TypeMapper.IgnoreCaseSensitive);
            reqCtas.TipoBusqueda = "N";
            reqCtas.Segmento = entity.Segmento;
            reqCtas.IdServicio = null;
            reqCtas.Titulares = incluirTitulares ? "S" : "N";
            reqCtas.CuentasRespuesta = tipoCta;
            reqCtas.Cabecera.H_Nup = entity.Nup;
            reqCtas.DatoConsulta = entity.Nup;

            var reqSecurity = firma.MapperClass<RequestSecurity<GetCuentas>>(TypeMapper.IgnoreCaseSensitive);

            reqSecurity.Datos = reqCtas;

            if (entity.IdServicio == Servicio.PoderDeCompra && (entity.Canal == "12" || entity.Canal == "09")) //Si es PDC SIVD traigo cuentas fisicas y juridicas
            {
                cliente = DependencyFactory.Resolve<IServiceWebApiClient>().GetCuentasFisicasYjuridicas(reqSecurity);
            }
            else
            {
                cliente = DependencyFactory.Resolve<IServiceWebApiClient>().GetCuentas(reqSecurity);
            }

            ClienteCuentaDDC[] cuenta = new ClienteCuentaDDC[] { };
            if (cliente != null && cliente.FirstOrDefault() != null)
                cuenta = cliente.FirstOrDefault().Cuentas;

            reqSecurity.Canal = entity.Canal;
            reqSecurity.SubCanal = entity.SubCanal;

            if (entity.IdServicio == Servicio.Repatriacion )
            {
                var cuentasCustodia = DependencyFactory.Resolve<IServiceWebApiClient>().ConsultaCuentasCustodia(reqSecurity,true);
                cuenta = cuentasCustodia.ToArray();
            }

            //Agrego cuentas repatriacion para saber si mostrar la tarjeta con lista de servicios
            if (string.IsNullOrEmpty(entity.IdServicio) && tipoCta == "OP")
            {
                var cuentasCustodia = DependencyFactory.Resolve<IServiceWebApiClient>().ConsultaCuentasCustodia(reqSecurity,true);

                if (cuentasCustodia != null && cuentasCustodia.Any() && cuenta != null && cuenta.Any())
                {
                    var listCuentas = cuenta.ToList();
                    listCuentas.AddRange(cuentasCustodia.ToList());

                    cuenta = listCuentas.ToArray();
                }

            }

            //Se remueven las cuentas repatriacion
            if (entity.IdServicio == Servicio.Agendamiento || entity.IdServicio == Servicio.AgendamientoFH || entity.IdServicio == Servicio.PoderDeCompra)
            {
                reqSecurity.Canal = entity.Canal;
                reqSecurity.SubCanal = entity.SubCanal;

                var cuentasCustodia = DependencyFactory.Resolve<IServiceWebApiClient>().ConsultaCuentasCustodia(reqSecurity,false);

                if (cuentasCustodia != null && cuentasCustodia.Any() && cuenta != null && cuenta.Any())
                {
                    var listCuentas = cuenta.ToList();

                    foreach (var cuentaCustodia in cuentasCustodia.ToList().Where(a => a.DescripcionTipoCta == "REPATRIACION"))
                    {
                        if (!string.IsNullOrEmpty(cuentaCustodia.NroCta))
                        { 
                        var cuentaRepa = listCuentas.FirstOrDefault(a => a.NroCta.Contains(cuentaCustodia.NroCta));
                        if (cuentaRepa != null) listCuentas.Remove(cuentaRepa);
                         }
                         
                        var cuentaRepaOp = listCuentas.FirstOrDefault(a => a.NroCta.Contains(cuentaCustodia.Titulares.FirstOrDefault()?.NroCtaOperativa?.PadLeft(12, '0')));
                        if (cuentaRepaOp != null) listCuentas.Remove(cuentaRepaOp);
                    }

                    cuenta = listCuentas.ToArray();
                }
            }

            return cuenta;
        }

        public static DetalleDeFondoResp ObtenerInfoFondo(FormularioResponse entity)
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

            var req = new InfoFondoReq
            {
                CodigoDeFondo = entity.CodigoDeFondo
            };

            return daMapsControles.ObtenerInfoFondo(req);
        }

        private static string ObtenerCodigoOperacion(string operacion)
        {
            string result = null;

            switch (operacion.ToLower())
            {
                case "suscripción":
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

        public static List<ClienteCuentaDDC> ObtenerCuentasPDC(FormularioResponse entity, DatoFirmaMaps firma, bool? porConsultaAB = null)
        {
            var reqAdhesiones = entity.MapperClass<RequestSecurity<RequestMaps>>();
            reqAdhesiones.Datos = new RequestMaps
            {
                Nup = entity.Nup,
                IdServicio = entity.IdServicio,
                FormCompleted = 0,
                Segmento = entity.Segmento,
                Usuario = entity.Usuario,
                Ip = entity.Ip
            };
            reqAdhesiones.Firma = firma.Firma;
            reqAdhesiones.Dato = firma.Dato;
            reqAdhesiones.DatosFirmado = firma.DatosFirmado;
            var responseAdhesionesPDC = GetAdhesiones(reqAdhesiones);

            /*
             * switch (item.Estado)
                {
                    case "A": estado = "A"; break;
                    case "SB": estado = "A"; break;
                    case "AC": estado = "A"; break;
                    case "SA": estado = "A"; break;
                    default: estado = "D"; break;
                }
                Estado PDC
                A   Cuenta Alta
                SA  Cuenta Solicitud de Ata
                SB  Solicitud de Baja
                B   Cuenta de Baja
                N  Cuenta No existente en PDC
                AC Adherido por Cotitular

                Estado Adhesion  (agrupamiento en función de los Estados PDC)
                A Adherido (A, SB, AC, SA)
                D Disponible (B, N)
             */

            if (porConsultaAB.HasValue && porConsultaAB == true) //Viene por aca cuando se esta consultando adhesiones Alta/Baja relacionadas fondoSeleccionado un nup
            {
                responseAdhesionesPDC = responseAdhesionesPDC.Where(x => !x.Estado.Equals("N") && !x.Estado.Equals("SB") && !x.Estado.Equals("SA")).GroupBy(x => x.CuentaTitulos).Select(x => x.First()).ToList();
            }
            else
            {
                responseAdhesionesPDC = responseAdhesionesPDC.Where(x => !x.Estado.Equals("A") && !x.Estado.Equals("SB") && !x.Estado.Equals("AC") && !x.Estado.Equals("SA") /*&& !x.Estado.Equals("N")*/).GroupBy(x => x.CuentaTitulos).Select(x => x.First()).ToList();
            }

            var cuentasTitulo = ObtenerCuentasPorTipo(entity, "TI", firma, true);

            var cuentasDisponibles = from cuentaPDC in responseAdhesionesPDC
                                     join cuentaTitulo in cuentasTitulo on cuentaPDC.CuentaTitulos equals cuentaTitulo.NroCta.ParseGenericVal<long>()
                                     select cuentaTitulo;

            if (porConsultaAB.HasValue && porConsultaAB == true) //Viene por aca cuando se esta consultando adhesiones Alta/Baja relacionadas fondoSeleccionado un nup
            {

                var cuentas = cuentasDisponibles != null && cuentasDisponibles.Count() > 0 ? cuentasTitulo.Union(cuentasDisponibles.ToList()) : cuentasTitulo;

                return cuentas?.ToList();
            }

            return cuentasDisponibles.ToList();
        }

        public static List<Cliente> GetAdhesiones(RequestSecurity<RequestMaps> entity)
        {
            var reqCtaAptas = entity.MapperClass<RequestSecurity<RequestCuentasAptas>>();
            reqCtaAptas.Datos = new RequestCuentasAptas
            {
                DatoConsulta = entity.Datos.Nup,
                Canal = entity.Canal,
                SubCanal = entity.SubCanal,
                IdServicio = entity.Datos.IdServicio,
                Segmento = entity.Datos.Segmento,
                CuentasRespuesta = "TO",
                TipoBusqueda = "N"
            };

            var respuesta = DependencyFactory.Resolve<IServiceWebApiClient>().GetCuentasAptas(reqCtaAptas)?.ToList();

            return respuesta;
        }

        public static void AgregarComponente(string nombreComponente, FormularioResponse entity, string tipo)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            long idComponente = daMapsControles.ObtenerIdComponente(nombreComponente, entity.Usuario, entity.Ip);
            var compFecha = new Fecha    //TODO: agregar el nombre nuevo segun ppt.
            {
                IdComponente = idComponente,
                Nombre = nombreComponente
            };

            compFecha.AsignarDatosBackend(daMapsControles.ObtenerDatosPorComponente(compFecha, entity), null, compFecha);
            if (tipo == NombreComponente.FechaSolBaja)
            {
                compFecha.Etiqueta = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                {
                    NomParametro = Keys.FechaDeSolicitud
                });

                compFecha.Ayuda = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
                {
                    NomParametro = Keys.AyudaFechaDeBaja
                });
                compFecha.Config = NombreComponente.FechaSolBaja;
            }

            entity.Items?.Add(compFecha);
        }

        public static Cabecera GenerarCabecera(string canal, string subCanal)
        {
            return new Cabecera()
            {
                H_CanalTipo = canal,
                H_SubCanalId = "HTML",
                H_CanalVer = "000",
                H_SubCanalTipo = "99",
                H_CanalId = subCanal,
                H_UsuarioTipo = "03",
                H_UsuarioID = "ONLINEBP",
                H_UsuarioAttr = " ",
                H_UsuarioPwd = "DV09SA10",
                H_IdusConc = "00000000",
                H_NumSec = "00000002",
                H_IndSincro = "0",
                H_TipoCliente = "I",
                H_TipoIDCliente = "N",
                H_NroIDCliente = "13488020",
                H_FechaNac = "19570812"
            };
        }

        public static decimal? ObtenerSaldoCuenta(ClienteCuentaDDC cuenta, string segmento, string nup, string canal, string subCanal, string ip, string usuario)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            DatosCuentaIATXResponse datosCuentaIATXResponse = null;
            var usuarioRacf = DependencyFactory.Resolve<IMapsControlesDA>().ObtenerUsuarioRacf();

            int tipoCta = 0;
            int sucCtaOper = 0;
            decimal? saldoCuenta = null;

            int.TryParse(cuenta.TipoCta.ToString(), out tipoCta);
            int.TryParse(cuenta.SucursalCta.ToString(), out sucCtaOper);

            try
            {
                #region BancaPrivada
                if (segmento.ToLower() == Segmento.BancaPrivada)
                {
                    decimal sumSaldo = 0;
                    var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
                    {
                        Nup = nup.ParseGenericVal<long?>(),
                        CuentaBp = 0
                    });

                    var cuentaBp = ValidarCuentasBP(atisResp, cuenta.NroCta.ParseGenericVal<long>());

                    var resLoadSaldos = DependencyFactory.Resolve<IOpicsDA>().EjecutarLoadSaldos(new LoadSaldosRequest
                    {
                        Canal = canal,
                        Cuenta = cuentaBp.Value.ParseGenericVal<string>(),
                        FechaDesde = DateTime.Now.Date,
                        FechaHasta = DateTime.Now.Date,
                        Usuario = usuarioRacf.Usuario,
                        Password = usuarioRacf.Password
                    });

                    var resSaldoConcertado = DependencyFactory.Resolve<ISmcDA>().EjecutarSaldoConcertadoNoLiquidado(new SaldoConcertadoNoLiquidadoRequest
                    {
                        Fecha = DateTime.Now.Date,
                        Ip = ip,
                        Moneda = TraducirUsdAUsb(cuenta.CodigoMoneda),
                        NroCtaOper = cuenta.NroCta.ParseGenericVal<string>(),
                        SucCtaOper = cuenta.SucursalCta.ParseGenericVal<string>(),
                        TipoCtaOper = cuenta.TipoCta.ParseGenericVal<decimal>(),
                        Usuario = usuario
                    });

                    switch (cuenta.CodigoMoneda.ToLower())
                    {
                        case "ars":
                            foreach (var saldo in resLoadSaldos.ListaSaldos)
                            {

                                sumSaldo += saldo.CAhorroPesos;
                            }
                            break;
                        case "usd":
                            foreach (var saldo in resLoadSaldos.ListaSaldos)
                            {
                                sumSaldo += saldo.CAhorroDolares;
                            }
                            break;
                    }

                    var totalSaldo = sumSaldo - (resSaldoConcertado.Saldo.HasValue ? resSaldoConcertado.Saldo.Value : 0m);
                    saldoCuenta = totalSaldo;
                }
                #endregion
                #region RTL
                else
                {
                    var cabecera = GenerarCabecera(canal, subCanal);
                    cabecera.H_UsuarioID = usuarioRacf.Usuario;
                    cabecera.H_UsuarioPwd = usuarioRacf.Password;

                    datosCuentaIATXResponse = daIATX.ConsultaDatosCuenta(cabecera, cuenta, nup);

                    if (cuenta.TipoCta == 10)
                    {
                        var entero = datosCuentaIATXResponse.Saldo_Cuenta_D.Substring(0, 12);
                        var dec = datosCuentaIATXResponse.Saldo_Cuenta_D.Substring(12, 2);
                        var signo = datosCuentaIATXResponse.Saldo_Cuenta_D.Substring(14, 1);
                        decimal numDec = 0;

                        if (decimal.TryParse($"{signo}{entero}.{dec}", out numDec))
                        {
                            saldoCuenta = numDec;
                        }
                    }
                    else
                    {
                        var entero = datosCuentaIATXResponse.Saldo_Cuenta.Substring(0, 12);
                        var dec = datosCuentaIATXResponse.Saldo_Cuenta.Substring(12, 2);
                        var signo = datosCuentaIATXResponse.Saldo_Cuenta.Substring(14, 1);
                        decimal numDec = 0;

                        if (decimal.TryParse($"{signo}{entero}.{dec}", out numDec))
                        {
                            saldoCuenta = numDec;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al obtener saldo de cuenta operativa:{ex.Message}", "BusinessHelper", "ObtenerSaldoCuenta");
                saldoCuenta = null;
            }
            return saldoCuenta;
        }

        public static decimal? ObtenerSaldoCuenta(ClienteCuentaDDC cuenta, string segmento, string nup, string canal, string subCanal, string ip, string usuario, DatoFirmaMaps firma)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            DatosCuentaIATXResponse datosCuentaIATXResponse = null;
            var usuarioRacf = DependencyFactory.Resolve<IMapsControlesDA>().ObtenerUsuarioRacf();

            int tipoCta = 0;
            int sucCtaOper = 0;
            decimal? saldoCuenta = null;

            int.TryParse(cuenta.TipoCta.ToString(), out tipoCta);
            int.TryParse(cuenta.SucursalCta.ToString(), out sucCtaOper);

            try
            {
                #region BancaPrivada
                if (segmento.ToLower() == Segmento.BancaPrivada)
                {
                    decimal sumSaldo = 0;
                    var atisResp = DependencyFactory.Resolve<IOpicsDA>().ObtenerAtis(new ConsultaLoadAtisRequest
                    {
                        Nup = nup.ParseGenericVal<long?>(),
                        CuentaBp = 0
                    });

                    var cuentaBp = ValidarCuentasBP(atisResp, cuenta.NroCta.ParseGenericVal<long>());
                    var req = firma.MapperClass<RequestSecurity<LoadSaldosRequest>>(TypeMapper.IgnoreCaseSensitive);
                    req.Canal = canal;
                    req.SubCanal = subCanal;
                    req.Datos = new LoadSaldosRequest
                    {
                        Canal = canal,
                        Cuenta = cuentaBp.Value.ParseGenericVal<string>(),
                        FechaDesde = DateTime.Now.Date,
                        FechaHasta = DateTime.Now.Date,
                        Usuario = usuarioRacf.Usuario,
                        Password = usuarioRacf.Password
                    };
                    LoggingHelper.Instance.Information($"Fecha-Hora: {DateTime.Now.ToString("dd/MM/yyyy")} | Request ObtenerSaldoBP: {JsonConvert.SerializeObject(req)}");
                    var resLoadSaldos = DependencyFactory.Resolve<IServiceWebApiClient>().ObtenerSaldoBP(req);

                    /*
                    var resSaldoConcertado = DependencyFactory.Resolve<ISmcDA>().EjecutarSaldoConcertadoNoLiquidado(new SaldoConcertadoNoLiquidadoRequest
                    {
                        Fecha = DateTime.Now.Date,
                        Ip = ip,
                        Moneda = TraducirUsdAUsb(cuenta.CodigoMoneda),
                        NroCtaOper = cuenta.NroCta.ParseGenericVal<string>(),
                        SucCtaOper = cuenta.SucursalCta.ParseGenericVal<string>(),
                        TipoCtaOper = cuenta.TipoCta.ParseGenericVal<decimal>(),
                        Usuario = usuario
                    });
                    LoggingHelper.Instance.Information($"Fecha-Hora: {DateTime.Now.ToString("dd/MM/yyyy")} | Response ObtenerSaldoBP: {JsonConvert.SerializeObject(resSaldoConcertado)}");
                    */
                    switch (cuenta.CodigoMoneda.ToLower())
                    {
                        case "ars":
                            foreach (var saldo in resLoadSaldos.ListaSaldos)
                            {

                                sumSaldo += saldo.CAhorroPesos;
                            }
                            break;
                        case "usd":
                            foreach (var saldo in resLoadSaldos.ListaSaldos)
                            {
                                sumSaldo += saldo.CAhorroDolares;
                            }
                            break;
                    }

                    var totalSaldo = sumSaldo; //- (resSaldoConcertado.Saldo.HasValue ? resSaldoConcertado.Saldo.Value : 0m);
                    saldoCuenta = totalSaldo;
                }
                #endregion
                #region RTL
                else
                {
                    var cabecera = GenerarCabecera(canal, subCanal);
                    cabecera.H_UsuarioID = usuarioRacf.Usuario;
                    cabecera.H_UsuarioPwd = usuarioRacf.Password;

                    datosCuentaIATXResponse = daIATX.ConsultaDatosCuenta(cabecera, cuenta, nup);

                    if (cuenta.TipoCta == 10)
                    {
                        var entero = datosCuentaIATXResponse.Saldo_Cuenta_D.Substring(0, 12);
                        var dec = datosCuentaIATXResponse.Saldo_Cuenta_D.Substring(12, 2);
                        var signo = datosCuentaIATXResponse.Saldo_Cuenta_D.Substring(14, 1);
                        decimal numDec = 0;

                        if (decimal.TryParse($"{signo}{entero}.{dec}", out numDec))
                        {
                            saldoCuenta = numDec;
                        }
                    }
                    else
                    {
                        var entero = datosCuentaIATXResponse.Saldo_Cuenta.Substring(0, 12);
                        var dec = datosCuentaIATXResponse.Saldo_Cuenta.Substring(12, 2);
                        var signo = datosCuentaIATXResponse.Saldo_Cuenta.Substring(14, 1);
                        decimal numDec = 0;

                        if (decimal.TryParse($"{signo}{entero}.{dec}", out numDec))
                        {
                            saldoCuenta = numDec;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al obtener saldo de cuenta operativa:{ex.Message}", "BusinessHelper", "ObtenerSaldoCuenta");
                saldoCuenta = null;
            }
            return saldoCuenta;
        }


        public static string CreacionCuentaTitulosRepatriacion(FormularioResponse entity, ItemCuentaOperativa<string> cuentaOperativa)
        {
            ICanalesIatxDA daIATX = DependencyFactory.Resolve<ICanalesIatxDA>();
            //GeneracionCuentaResponse datosCuentaIATXResponse = null;
            var usuarioRacf = DependencyFactory.Resolve<IMapsControlesDA>().ObtenerUsuarioRacf();

            try
            {
                var cabecera = GenerarCabecera(entity.Canal, entity.SubCanal);
                cabecera.H_UsuarioID = usuarioRacf.Usuario;
                cabecera.H_UsuarioPwd = usuarioRacf.Password;


                var SucCtaOper = cuentaOperativa?.SucursalCtaOperativa;
                var subProducto = cuentaOperativa?.SubProducto;
                var producto = cuentaOperativa?.Producto;

                //datosCuentaIATXResponse = daIATX.ConsultaCuentaIATX(cabecera);

                var numeroCuentaGenerado = GenerarNumeroCuenta(daIATX, cabecera, entity, SucCtaOper, producto, subProducto);

                if (!string.IsNullOrEmpty(numeroCuentaGenerado))
                    AltaCuentaTitulo(daIATX, cabecera, entity, SucCtaOper, producto, subProducto, numeroCuentaGenerado);

                return numeroCuentaGenerado;

            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al generar la cuenta titulos:{ex.Message}", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");
                throw new BusinessException("Error al crear la cuenta titulos");
                //return null;
            }
        }

        private static string GenerarNumeroCuenta(ICanalesIatxDA daIATX,Cabecera cabecera, FormularioResponse entity,string SucCtaOper, string CodProducto, string subProducto)
        {

            try
            {
                GeneracionCuentaResponse datosCuentaIATXResponse = null;
                datosCuentaIATXResponse = daIATX.GeneracionCuentaIATX(cabecera, entity, SucCtaOper, CodProducto, subProducto);

                return datosCuentaIATXResponse.Código_retorno_extendido;
            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al generar el numero de cuenta titulos:{ex.Message}", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");
                return null;
            }
        }

        private static string AltaCuentaTitulo(ICanalesIatxDA daIATX, Cabecera cabecera, FormularioResponse entity, string SucCtaOper, string CodProducto, string subProducto, string numeroCuenta)
        {

            try
            {
                GeneracionCuentaResponse datosCuentaIATXResponse = null;
                datosCuentaIATXResponse = daIATX.AltaCuentaIATX(cabecera, entity, SucCtaOper, CodProducto ,subProducto,numeroCuenta);

                return datosCuentaIATXResponse.Código_retorno_extendido;
            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error(ex, $"Error al dar de alta la cuenta titulo:{ex.Message}", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");
                return null;
            }
        }

        public static void ReemplazarControlEmail(FormularioResponse entity)
        {
            if (entity.IdServicio == "RTF")
            {
                var item = entity.Items
                            .Where(x => string.Compare((x as ControlSimple).Nombre, NombreComponente.Email, true) == 0)
                            .FirstOrDefault() as InputText<string>;
                if (item != null)
                {
                    item.Valor = "Se envía al email registrado en Mensajes y Avisos.";
                }
            }

        }

        public static ControlSimple AgregarComponenteLegal(FormularioResponse entity, long idLegal, string idServicio)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var compLegal = new Lista<ItemLegal<string>>();
            compLegal.Tipo = TipoControl.Legal;
            compLegal.Nombre = NombreComponente.LegalBajaPDC;
            compLegal.IdComponente = idLegal;
            entity.IdComponente = idLegal;
            entity.IdServicio = idServicio;

            var datosLegal = daMapsControles.ObtenerDatosPorComponente(compLegal, entity);
            compLegal.AsignarDatosBackend(datosLegal, null, compLegal);

            return compLegal;
        }

        public static string TraducirUsdAUsb(string monedaOperacion)
        {
            switch (monedaOperacion.ToUpper())
            {
                case "USD": return "USB";
                default: return monedaOperacion;
            }
        }

        /// <summary>
        /// valida que la cuenta esta disponible para el servicio
        /// </summary>
        /// <param name="cuentaTitulo"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool ValidarCuentaDisponible(string cuentaTitulo, FormularioResponse entity)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var r =  daMapsControles.ValidarCuenta(Convert.ToDecimal(cuentaTitulo), null, null, null, entity).Value == 1 ? false : true;

            return r;
        }

        public static ClienteCuentaDDC[] ValidarCuentas(ClienteCuentaDDC[] cuentas, FormularioResponse entity)
        {
            List<ClienteCuentaDDC> cuentasValidas = new List<ClienteCuentaDDC>();
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

            foreach (var cuenta in cuentas)
            {
                decimal? validado = -1;

                if (string.IsNullOrWhiteSpace(cuenta.NroCta))
                {
                    throw new Exception("La cuenta no puede estar vacía.");
                }

                if (string.Compare(entity.IdServicio, Servicio.SAF, true) == 0)
                {
                    if (string.IsNullOrWhiteSpace(cuenta.SucursalCta))
                    {
                        throw new Exception("La sucursal de la cuenta no puede estar vacío.");
                    }

                    validado = daMapsControles.ValidarCuenta(null, Convert.ToDecimal(cuenta.NroCta), Convert.ToInt32(cuenta.TipoCta), Convert.ToInt32(cuenta.SucursalCta), entity);
                }
                else if (string.Compare(entity.IdServicio, Servicio.PoderDeCompra, true) == 0)
                {
                    validado = daMapsControles.ValidarCuenta(Convert.ToDecimal(cuenta.NroCta), null, null, null, entity);
                }
                else
                {
                    validado = 0;
                }

                if (string.Compare(cuenta.CuentaBloqueada, TipoEstado.CuentaBloqueada, true) == 0)
                {
                    validado = 1; //Cuenta bloqueda no permite operar.
                }

                if (validado == 0) //No existe en adhesiones activas y se puede utilizar.
                {
                    cuentasValidas.Add(cuenta);
                }
            }

            return cuentasValidas.ToArray();
        }

        public static string[] ObtenerTitulares(TitularDDC[] titulares)
        {
            var titularesCuenta = new List<string>();

            if (titulares != null)
            {
                foreach (var item in titulares)
                {
                    var titular = string.Empty;
                    if (!string.IsNullOrWhiteSpace(item.PrimerApellido))
                        titular = item.PrimerApellido.Trim();

                    if (!string.IsNullOrWhiteSpace(item.SegundoApellido))
                        titular = titular + " " + item.SegundoApellido.Trim();

                    if (!string.IsNullOrWhiteSpace(titular))
                        titular = titular + ", " + item.Nombre.Trim();

                    else
                        titular = item.Nombre.Trim(); //------> Por defecto este campo, si o si debe venir informado.

                    titularesCuenta.Add(titular.Trim());
                }
            }
            return titularesCuenta.ToArray();
        }

        public static ControlSimple AgregarComponenteEstado(FormularioResponse entity, string estado, long idEstadoAdhesion)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var compEstadoAdhesion = new EstadoAdhesion<string>();
            compEstadoAdhesion.Tipo = TipoControl.InputText;
            compEstadoAdhesion.Nombre = NombreComponente.EstadoAdhesion;
            compEstadoAdhesion.IdComponente = idEstadoAdhesion;
            entity.IdComponente = idEstadoAdhesion;

            var datosEA = daMapsControles.ObtenerDatosPorComponente(compEstadoAdhesion, entity);
            compEstadoAdhesion.AsignarDatosBackend(datosEA, null, compEstadoAdhesion);
            compEstadoAdhesion.Valor = estado;
            compEstadoAdhesion.Bloqueado = true;
            compEstadoAdhesion.Validado = true;

            return compEstadoAdhesion;
        }

        public static EstadoAdhesion<string> AgregarComponenteEstado(FormularioResponse entity, long idEstadoAdhesion)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var compEstadoAdhesion = new EstadoAdhesion<string>();
            compEstadoAdhesion.Tipo = TipoControl.InputText;
            compEstadoAdhesion.Nombre = NombreComponente.EstadoAdhesion;
            compEstadoAdhesion.IdComponente = idEstadoAdhesion;
            entity.IdComponente = idEstadoAdhesion;

            var datosEA = daMapsControles.ObtenerDatosPorComponente(compEstadoAdhesion, entity);
            compEstadoAdhesion.AsignarDatosBackend(datosEA, null, compEstadoAdhesion);
            //compEstadoAdhesion.Valor = estado;
            compEstadoAdhesion.Bloqueado = true;
            compEstadoAdhesion.Validado = true;

            return compEstadoAdhesion;
        }

        public static ControlSimple AgregarComponenteVigencia(FormularioResponse entity, long idVigencia, long idPeriodos, long idFechaDesde, long idFechaHasta, FormularioResponse fa)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var compVigencia = new FechaCompuesta();
            compVigencia.Tipo = TipoControl.FechaCompuesta;
            compVigencia.Nombre = NombreComponente.Vigencia;
            compVigencia.IdComponente = idVigencia;
            entity.IdComponente = idVigencia;
            var datosEA = daMapsControles.ObtenerDatosPorComponente(compVigencia, entity);
            compVigencia.AsignarDatosBackend(datosEA, null, compVigencia);
            compVigencia.Bloqueado = true;
            compVigencia.Validado = true;
            compVigencia.Etiqueta = "Vigencia";

            var compPeriodos = new Lista<Item<decimal>>();
            compPeriodos.Tipo = TipoControl.Lista;
            compPeriodos.Nombre = NombreComponente.Periodos;
            compPeriodos.IdComponente = idPeriodos;
            var datosEAPeriodos = daMapsControles.ObtenerDatosPorComponente(compPeriodos, entity);
            compPeriodos.AsignarDatosBackend(datosEAPeriodos, null, compPeriodos);

            if (compPeriodos != null)
            {
                var seleccion = compPeriodos.Items.Where(x => x.Desc == "Otro Intervalo").FirstOrDefault();
                if(seleccion != null) seleccion.Seleccionado = true;
            }

            compVigencia.Items.Add(compPeriodos);

            var fechaEje = fa.Items.Where(x => x.Nombre == NombreComponente.Fecha)?.Where(y => string.Compare(y.Id, "fecha-1", true) == 0).FirstOrDefault();

            var compFechaDesde = new Fecha();
            compFechaDesde.Tipo = TipoControl.Fecha;
            compFechaDesde.Nombre = NombreComponente.FechaDesde;
            compFechaDesde.IdComponente = idFechaDesde;
            compFechaDesde.AsignarDatosBackend(daMapsControles.ObtenerDatosPorComponente(compFechaDesde, entity), null, compFechaDesde);
            compFechaDesde.FechaMin = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            compFechaDesde.FechaMax = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            compFechaDesde.Valor = (fechaEje as Fecha)?.Valor ?? DateTime.Now;

            compVigencia.Items.Add(compFechaDesde);

            var compFechaHasta = new Fecha();
            compFechaHasta.Tipo = TipoControl.Fecha;
            compFechaHasta.Nombre = NombreComponente.FechaHasta;
            compFechaHasta.IdComponente = idFechaHasta;
            compFechaHasta.AsignarDatosBackend(daMapsControles.ObtenerDatosPorComponente(compFechaHasta, entity), null, compFechaHasta);
            compFechaHasta.FechaMin = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            compFechaHasta.FechaMax = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            compFechaHasta.Valor = (fechaEje as Fecha)?.Valor ?? DateTime.Now;

            compVigencia.Items.Add(compFechaHasta);

            return compVigencia;
        }

        public static FechaCompuesta AgregarComponenteVigencia(FormularioResponse entity, long idVigencia, long idPeriodos, long idFechaDesde, long idFechaHasta)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var compVigencia = new FechaCompuesta();
            compVigencia.Tipo = TipoControl.FechaCompuesta;
            compVigencia.Nombre = NombreComponente.Vigencia;
            compVigencia.IdComponente = idVigencia;
            entity.IdComponente = idVigencia;
            var datosEA = daMapsControles.ObtenerDatosPorComponente(compVigencia, entity);
            compVigencia.AsignarDatosBackend(datosEA, null, compVigencia);
            compVigencia.Bloqueado = true;
            compVigencia.Validado = true;
            compVigencia.Etiqueta = "Vigencia";

            var compPeriodos = new Lista<Item<decimal>>();
            compPeriodos.Tipo = TipoControl.Lista;
            compPeriodos.Nombre = NombreComponente.Periodos;
            compPeriodos.IdComponente = idPeriodos;
            var datosEAPeriodos = daMapsControles.ObtenerDatosPorComponente(compPeriodos, entity);
            compPeriodos.AsignarDatosBackend(datosEAPeriodos, null, compPeriodos);

            if (compPeriodos != null)
            {
                var seleccion = compPeriodos.Items.Where(x => x.Desc == "Otro Intervalo").FirstOrDefault();
                if (seleccion != null) seleccion.Seleccionado = true;
            }

            compVigencia.Items.Add(compPeriodos);

            //var fechaEje = fa.Items.Where(x => x.Nombre == NombreComponente.Fecha)?.Where(y => string.Compare(y.Id, "fecha-1", true) == 0).FirstOrDefault();

            var compFechaDesde = new Fecha();
            compFechaDesde.Tipo = TipoControl.Fecha;
            compFechaDesde.Nombre = NombreComponente.FechaDesde;
            compFechaDesde.IdComponente = idFechaDesde;
            compFechaDesde.AsignarDatosBackend(daMapsControles.ObtenerDatosPorComponente(compFechaDesde, entity), null, compFechaDesde);
            //compFechaDesde.FechaMin = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            //compFechaDesde.FechaMax = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            //compFechaDesde.Valor = (fechaEje as Fecha)?.Valor ?? DateTime.Now;

            compVigencia.Items.Add(compFechaDesde);

            var compFechaHasta = new Fecha();
            compFechaHasta.Tipo = TipoControl.Fecha;
            compFechaHasta.Nombre = NombreComponente.FechaHasta;
            compFechaHasta.IdComponente = idFechaHasta;
            compFechaHasta.AsignarDatosBackend(daMapsControles.ObtenerDatosPorComponente(compFechaHasta, entity), null, compFechaHasta);
            //compFechaHasta.FechaMin = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            //compFechaHasta.FechaMax = (fechaEje as Fecha)?.Valor ?? DateTime.Now;
            //compFechaHasta.Valor = (fechaEje as Fecha)?.Valor ?? DateTime.Now;

            compVigencia.Items.Add(compFechaHasta);

            return compVigencia;
        }


        public static ControlSimple AgregarComponenteDescripcion(FormularioResponse entity, long idDescripcionDinamica, string idServicio)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var compDescripcionDinamica = new DescripcionDinamica<string>();
            compDescripcionDinamica.Tipo = TipoControl.InputText;
            compDescripcionDinamica.Nombre = NombreComponente.DescripcionDinamica;
            compDescripcionDinamica.IdComponente = idDescripcionDinamica;

            #region parche
            //TODO: esto es parche porque debe llegar el id de servicio cuando es consulta por todos sin modificar 
            //la entity original.

            var _entity = entity.MapperClass<FormularioResponse>(TypeMapper.IgnoreCaseSensitive);
            _entity.IdComponente = idDescripcionDinamica;
            _entity.IdServicio = idServicio;
            #endregion

            var datosDD = daMapsControles.ObtenerDatosPorComponente(compDescripcionDinamica, _entity);
            compDescripcionDinamica.AsignarDatosBackend(datosDD, null, compDescripcionDinamica);
            compDescripcionDinamica.Bloqueado = true;
            compDescripcionDinamica.Validado = true;

            return compDescripcionDinamica;
        }

        public static bool ValidarExistencia(string nombreComponente, FormularioResponse frm)
        {
            var result = false;
            var componente = frm.Items.Where(x =>
                        string.Compare((x as ControlSimple).Nombre, nombreComponente, true) == 0)
                        .ToList();

            if (componente.Count > 0) //Existe
            {
                result = true;
            }

            return result;
        }

        public static string ConcatenarCuentas(ClienteCuentaDDC[] cuentasLista, bool conInformacionAdicional)
        {
            string cuentas = string.Empty;
            if (conInformacionAdicional) // ----> Cuentas Operativas
                cuentas = string.Join(",", cuentasLista.Select(x => x.SucursalCta + "|" + x.NroCta + "|" + x.CodigoMoneda));
            else // ----> Cuentas Titulos
                cuentas = string.Join(",", cuentasLista.Select(x => x.NroCta));
            return cuentas;
        }

        public static SimulaPdcResponse SimularAltaBajaAdhesionPDC(FormularioResponse entity, DatoFirmaMaps firma, string operacion, string accion, long? idPDC = null)
        {
            var reqSimulacion = entity.MapperClass<RequestSecurity<SimulaPdcRequest>>();
            reqSimulacion.Datos = new SimulaPdcRequest
            {
                NUP = entity.Nup,
                Operacion = operacion == CuentaPDC.Simular ? accion == "A" ? CuentaPDC.SimularAlta : CuentaPDC.SimularBaja : accion == "A" ? CuentaPDC.ProcesarAlta : CuentaPDC.ProcesarBaja,
                Segmento = entity.Segmento,
                Canal = entity.Canal,
                Subcanal = entity.SubCanal,
                Usuario = entity.Usuario,
                Ip = entity.Ip,
            };

            if (string.Compare(accion, CuentaPDC.Baja, true) == 0) //Viene por una baja en PDC
            {
                reqSimulacion.Datos.IDSimCuentaPDC = idPDC.HasValue ? idPDC.Value : 0;
            }
            else if (string.Compare(operacion, CuentaPDC.Procesar, true) == 0 && string.Compare(accion, CuentaPDC.Alta, true) == 0) //Se debe informar el id de simulacion en PDC
            {
                reqSimulacion.Datos.IDSimCuentaPDC = idPDC ?? 0;
            }

            if (string.Compare(accion, TipoEstado.AltaConsulta, true) == 0 && string.Compare(operacion, CuentaPDC.Simular, true) == 0)
            {
                //ALTA
                var ctaOperativa = entity.Items.GetControlMaps<Lista<ItemCuentaOperativa<string>>>(NombreComponente.CuentaOperativa);
                var ctaTitulo = entity.Items.GetControlMaps<Lista<ItemCuentaTitulos<string>>>(NombreComponente.CuentaTitulo);
                var moneda = entity.Items.GetControlMaps<Lista<ItemMoneda<string>>>(NombreComponente.Moneda);

                var ctaOperativaSeleccionada = ctaOperativa?.Items?.Where(x => x.Seleccionado == true).FirstOrDefault();
                var ctaTituloSeleccionada = ctaTitulo?.Items?.Where(x => x.Seleccionado == true).FirstOrDefault();
                var monedaSeleccionada = moneda?.Items?.Where(x => x.Seleccionado == true).FirstOrDefault();

                reqSimulacion.Datos.CodigoMoneda = ctaOperativaSeleccionada?.CodigoMoneda;
                reqSimulacion.Datos.CuentaTitulos = ctaTituloSeleccionada?.NumeroCtaTitulo.ParseGenericVal<long?>();
                reqSimulacion.Datos.NroCtaOperativa = ctaOperativaSeleccionada?.NumeroCtaOperativa;
                reqSimulacion.Datos.NUP = entity.Nup;
                reqSimulacion.Datos.Producto = ctaOperativaSeleccionada?.Producto;
                reqSimulacion.Datos.Segmento = entity.Segmento;
                reqSimulacion.Datos.Subproducto = ctaOperativaSeleccionada?.SubProducto;
                reqSimulacion.Datos.SucursalCtaOperativa = ctaOperativaSeleccionada?.SucursalCtaOperativa;
                reqSimulacion.Datos.TipoCtaOperativa = ctaOperativaSeleccionada?.TipoCtaOperativa;
            }

            reqSimulacion.Firma = firma.Firma;
            reqSimulacion.Dato = firma.Dato;
            reqSimulacion.DatosFirmado = firma.DatosFirmado;

            var response = new SimulaPdcResponse();

            response = DependencyFactory.Resolve<IServiceWebApiClient>().PDCSimulacionAltasBajas(reqSimulacion);

            return response;
        }

        /// <summary>
        /// Método para obtener el ID del siguiente formulario.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static long? ObtenerSiguienteFormulario(FormularioResponse entity, string frmOrigen)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            long? val = null;
            string componentes = null;

            if (entity.Items != null)
            {
                componentes = string.Join(",", (from c in entity.Items
                                                select string.Format("{0},{1}", c.Config, c.Posicion)));

                if (!string.IsNullOrWhiteSpace(componentes) && !string.IsNullOrWhiteSpace(entity.IdServicio))
                {
                    try
                    {
                        long? sigForm = daMapsControles.ObtenerSiguienteFormulario(entity, componentes, frmOrigen);

                        val = (sigForm.HasValue ? sigForm : entity.FormularioId);
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessException($"ERROR AL OBTENER EL SIGUIENTE FORMULARIO. Cantidad de controles enviados {entity.Items.Count}\r\n", ex);
                    }
                }
            }

            return val;
        }

        public static ClienteCuentaDDC[] ValidarCuentasBloquedas(ClienteCuentaDDC[] cuentas)
        {
            List<ClienteCuentaDDC> cuentasSinBloquear = new List<ClienteCuentaDDC>();

            for (int i = 0; i < cuentas.Length; i++)
            {
                if (string.Compare(cuentas[i].CuentaBloqueada, TipoEstado.CuentaBloqueada, true) != 0)
                {
                    cuentasSinBloquear.Add(cuentas[i]);
                }
            }

            return cuentasSinBloquear.ToArray();
        }

        public static long? ValidarCuentas(List<ConsultaLoadAtisResponse> atisResp, long nroCtaOperativa)
        {
            bool cuentasValidas = false;

            foreach (var item in atisResp)
            {
                cuentasValidas = item.Validar(item.CuentaBp.Value.ToString().Substring(3, item.CuentaBp.Value.ToString().Length - 3), nroCtaOperativa.ToString());//sacar los 3 primeros.

                if (cuentasValidas)
                {
                    return item.CuentaAtit;
                }
            }

            throw new BusinessException("La cuenta seleccionada no le permite realizar esta operacion."); //Esto va suceder cuando no hay relacion de cuenta operativa con titulo en BCAINV.
        }

        public static long? ValidarCuentasBP(List<ConsultaLoadAtisResponse> atisResp, long nroCtaOperativa)
        {
            bool cuentasValidas = false;

            foreach (var item in atisResp)
            {
                cuentasValidas = item.Validar(item.CuentaBp.Value.ToString().Substring(3, item.CuentaBp.Value.ToString().Length - 3), nroCtaOperativa.ToString());//sacar los 3 primeros.

                if (cuentasValidas)
                {
                    return item.CuentaBp;
                }
            }

            throw new BusinessException("La cuenta seleccionada no le permite realizar esta operacion."); //Esto va suceder cuando no hay relacion de cuenta operativa con titulo en BCAINV.
        }

        private static Cabecera ObtenerCabeceraMock()
        {
            return new Cabecera()
            {
                H_CanalTipo = "04",
                H_SubCanalId = "HTML",
                H_CanalVer = "000",
                H_SubCanalTipo = "99",
                H_CanalId = "0001",
                H_UsuarioTipo = "03",
                H_UsuarioID = "01FRQF31",
                H_UsuarioAttr = "  ",
                H_UsuarioPwd = "@DP33YTO",
                H_IdusConc = "788646",
                H_NumSec = "14",
                //H_Nup = entity.Nup,
                H_IndSincro = "1",
                H_TipoCliente = "I",
                H_TipoIDCliente = "N",
                H_NroIDCliente = "00020956698",
                H_FechaNac = "19690922"
            };

        }

        public static void DepurarControles(FormularioResponse form)
        {
            form.Items.ForEach(componente =>
            {//por componente depurar las listas para cuando viajen con un elemento seleccionado.
                try
                {
                    switch (componente.Nombre)
                    {
                        case NombreComponente.FondoCompuesto:
                            var comp = componente as FondoCompuesto;
                            var fondoAGD = (comp.Items.Where(x => x.Nombre == NombreComponente.ListaFondos)).FirstOrDefault() as Lista<ItemGrupoAgd>;
                            var fondoSeleccionado = fondoAGD.Items.SelectMany(x => x.Items.Where(y => y.Seleccionado == true));

                            if (fondoSeleccionado.Count() > 1)
                            {
                                throw new BusinessException("Fue seleccionado más de un fondo para uno o varios grupos");
                            }
                            else if (fondoSeleccionado.Count() == 1)
                            {
                                var itemGrupo = fondoAGD.Items.Where(x => x.Items.Contains(fondoSeleccionado.FirstOrDefault())).FirstOrDefault();
                                itemGrupo.Items.RemoveAll(x => x.Seleccionado != true);
                                //var itemGrupo = new ItemGrupoAgd(fondoSeleccionado.FirstOrDefault());
                                fondoAGD.Items.Clear();
                                fondoAGD.Items.Add(itemGrupo);
                                #region revisar si va acá
                                fondoAGD.Bloqueado = true;
                                fondoAGD.Validado = true;
                                #endregion


                                form.CodigoDeFondo = fondoSeleccionado.FirstOrDefault()?.Valor;

                                var legalAgd = comp.Items.Where(c => c.Nombre == NombreComponente.LegalAgendamiento).FirstOrDefault() as Lista<ItemLegal<string>>;
                                var fac = new EstrategiaComp(new ConfigLegal(form, legalAgd));
                                fac.Crear();


                            }
                            else if (fondoSeleccionado.Count() == 0)
                            {
                                var legalesAGD = (comp.Items.Where(x => x.Nombre == NombreComponente.LegalAgendamiento)).FirstOrDefault() as Lista<ItemLegal<string>>;

                                legalesAGD.Items.Clear();
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
    }
}
