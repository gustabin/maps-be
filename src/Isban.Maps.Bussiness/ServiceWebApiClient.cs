
namespace Isban.Maps.Bussiness
{
    using Entity.Constantes.Estructuras;
    using Entity.Request;
    using Entity.Response;
    using IBussiness;
    using Isban.Mercados.LogTrace;
    using Mercados.Service.InOut;
    using Mercados.UnityInject;
    using Mercados.WebApiClient;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class ServiceWebApiClient : IServiceWebApiClient
    {
        [WebApiService("GetClienteByText")]
        public ClienteDDC[] GetCuentas(RequestSecurity<GetCuentas> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ClienteDDC[]>, RequestSecurity<GetCuentas>>(entity);

            if (response.Codigo != 0)
            {
                throw new Exception($"Código: {response.Codigo} (Mensaje técnico: { response.MensajeTecnico}");
            }

            var result = new List<ClienteDDC>();
            if (response.Datos != null)
            {
                for (int i = 0; i < response.Datos.Length; i++)
                {
                    if (string.Compare(response.Datos[i].TipoPersona, TipoPersona.Fisica, true) == 0)
                    {
                        result.Add(response.Datos[i]);
                    }
                }
            }
            //return response.Datos?.Where(x => string.Compare(x.TipoPersona, TipoPersona.Fisica, true) == 0)?.ToArray();            
            return result.ToArray();
        }

        [WebApiService("GetClienteByText")]
        public ClienteDDC[] GetCuentasFisicasYjuridicas(RequestSecurity<GetCuentas> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ClienteDDC[]>, RequestSecurity<GetCuentas>>(entity);

            if (response.Codigo != 0)
            {
                throw new Exception($"Código: {response.Codigo} (Mensaje técnico: { response.MensajeTecnico}");
            }

            var result = new List<ClienteDDC>();
            if (response.Datos != null)
            {
                for (int i = 0; i < response.Datos.Length; i++)
                {
                    result.Add(response.Datos[i]);
                }
            }
            //return response.Datos?.Where(x => string.Compare(x.TipoPersona, TipoPersona.Fisica, true) == 0)?.ToArray();            
            return result.ToArray();
        }

        [WebApiService("ConsultaCuentasCustodia")]
        public List<ClienteCuentaDDC> ConsultaCuentasCustodia(RequestSecurity<GetCuentas> entity, bool filtarOperativas)
        {
            LoggingHelper.Instance.Information($"Inicio ConsultaCuentasCustodia - Request: {JsonConvert.SerializeObject(entity)} - filtrarOperativas: {filtarOperativas}");
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<CuentasCustodiaResponse>, RequestSecurity<GetCuentas>>(entity);
            LoggingHelper.Instance.Information($"ConsultaCuentasCustodia - Response: {JsonConvert.SerializeObject(response)}");
            if (response.Codigo != 0)
            {
                throw new Exception($"Código: {response.Codigo} (Mensaje técnico: { response.MensajeTecnico}");
            }
            var result = new List<ClienteCuentaDDC>();

            if (response.Datos != null)
            {
                var cuentasRepra = response.Datos.CuentasCustodia.Where(a => a.Estado == "REPATRIACION");

                if (cuentasRepra != null)
                {
                    foreach (var cuenta in cuentasRepra)
                    {
                        var clienteCuenta = new ClienteCuentaDDC();

                        if (filtarOperativas)
                        {
                            if (!string.IsNullOrEmpty(cuenta.IdCuentaCustodia)) continue;
                            clienteCuenta.NroCta = cuenta.CuentasOperativas.FirstOrDefault()?.Id;
                        }
                        else
                        {
                            clienteCuenta.NroCta = cuenta.IdCuentaCustodia?.PadLeft(12, '0');
                        }


                        clienteCuenta.SegmentoCuenta = cuenta.Segmento;
                        clienteCuenta.CodProducto = cuenta.CuentasOperativas.FirstOrDefault()?.Producto;
                        clienteCuenta.CodSubproducto = cuenta.CuentasOperativas.FirstOrDefault()?.Subproducto;
                        clienteCuenta.CodigoMoneda = cuenta.CuentasOperativas.FirstOrDefault()?.Moneda;
                        clienteCuenta.SucursalCta = cuenta.CuentasOperativas.FirstOrDefault()?.Sucursal.PadLeft(4, '0');
                        clienteCuenta.DescripcionTipoCta = "REPATRIACION";
                        if (cuenta.CuentasOperativas.FirstOrDefault()?.Tipo != null)
                            clienteCuenta.TipoCta = long.Parse(cuenta.CuentasOperativas.FirstOrDefault()?.Tipo);

                        var listTitulares = new List<TitularDDC>();
                        foreach (var titular in cuenta.Titulares)
                        {
                            var titularDDC = new TitularDDC();
                            titularDDC.Nombre = titular.Nombre;
                            titularDDC.PrimerApellido = titular.Apellido;
                            titularDDC.CalidadParticipacion = titular.Participacion;
                            titularDDC.NroCtaOperativa = cuenta.CuentasOperativas.FirstOrDefault()?.Id;
                            listTitulares.Add(titularDDC);
                        }
                        clienteCuenta.Titulares = listTitulares.ToArray();
                        result.Add(clienteCuenta);

                    }
                }
         
            }
            LoggingHelper.Instance.Information($"Fin ConsultaCuentasCustodia - Result: {JsonConvert.SerializeObject(result)}");
            return result;
        }

        [WebApiService("PDCCUENTASAPTAS")]
        public List<Cliente> GetCuentasAptas(RequestSecurity<RequestCuentasAptas> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<List<ConsultaPdcResponse>>, RequestSecurity<RequestCuentasAptas>>(entity);
            var clientes = new List<Cliente>();
            if (response.Datos != null && response.Datos.Count() > 0)
            {
                var req = new GetClientePDC
                {
                    Canal = entity.Canal,
                    SubCanal = entity.SubCanal,
                    Ip = entity.Datos.Ip,
                    Usuario = entity.Datos.Usuario,
                    Clientes = response.Datos
                };

                clientes = GetClientePDC(req);
            }

            clientes = clientes.Where(r => 
              (!(r.CuentaApta.Equals(CuentaPDC.CuentaNoApta) && r.Estado.Equals(CuentaPDC.EstadoCuentaBaja))
            && !(r.CuentaApta.Equals(CuentaPDC.CuentaNoApta) && r.Estado.Equals(CuentaPDC.EstadoCuentaNoAdherido))
            && !string.IsNullOrEmpty(r.CuentaApta))).ToList();

            foreach (var item in clientes.Where(o => o.CuentaPdc < 0))
            {
                item.CuentaPdc = 0;
            }

            if (response.Codigo != 0)
            {
                //Manejo de Excepciones
            }

            return clientes;
        }

        [WebApiService("GetClientePDC")]
        public List<Cliente> GetClientePDC(GetClientePDC req)
        {
            var clientePDC = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<List<Cliente>>, Request<GetClientePDC>>(new Request<GetClientePDC> { Canal = req.Canal, SubCanal = req.SubCanal, Datos = req }).Datos;

            return clientePDC;
        }

        [WebApiService("SIMULACIONALTASBAJAS")]
        public SimulaPdcResponse PDCSimulacionAltasBajas(RequestSecurity<SimulaPdcRequest> entity)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<SimulaPdcResponse>, RequestSecurity<SimulaPdcRequest>>(entity);

            if (response.Codigo != 0)
            {
                throw new ExceptionWebApiClient(response.MensajeTecnico);
            }

            return response.Datos;
        }

        [WebApiService("ConsultaPerfilInversor")]
        public ConsultaPerfilInversorResponse ConsultaPerfilInversor(RequestSecurity<ConsultaPerfilInversorRequest> request)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ConsultaPerfilInversorResponse>, RequestSecurity<ConsultaPerfilInversorRequest>>(request);
            if (response.Codigo != 0)
            {
                throw new ExceptionWebApiClient(response.MensajeTecnico);
            }
            return response.Datos;
        }

        [WebApiService("ValidarRestriccionMEP")]
        public ValidaRestriccionMEPResponse ValidarRestriccionMEP(RequestSecurity<ValidaRestriccionMEPRequest> request)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ValidaRestriccionMEPResponse>, RequestSecurity<ValidaRestriccionMEPRequest>>(request);
            if (response.Codigo != 0)
            {
                throw new ExceptionWebApiClient(response.MensajeTecnico);
            }
            return response.Datos;
        }

        [WebApiService("ValidarRestriccionCMEP")]
        public ValidaRestriccionMEPResponse ValidarRestriccionCMEP(RequestSecurity<ValidaRestriccionCMEPRequest> request)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ValidaRestriccionMEPResponse>, RequestSecurity<ValidaRestriccionCMEPRequest>>(request);
            return ValidarResponse(response, response.Datos);
        }

        [WebApiService("ValidarCNV907MEP")]
        public ValidarCNV907Response ValidarCNV907(RequestSecurity<ValidarCNV907Request> request)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ValidarCNV907Response>, RequestSecurity<ValidarCNV907Request>>(request);
            return ValidarResponse(response, response.Datos);
        }

        [WebApiService("ObtenerSaldoBP")]
        public LoadSaldosResponse ObtenerSaldoBP(RequestSecurity<LoadSaldosRequest> request)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<LoadSaldosResponse>, RequestSecurity<LoadSaldosRequest>>(request);
            if (response.Codigo != 0)
            {
                throw new ExceptionWebApiClient(response.MensajeTecnico);
            }
            return response.Datos;
        }

        [WebApiService("VincularCuentasActivas")]
        public void VincularCuentasActivas(RequestSecurity<VincularCuentasActivasReq> request)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ObtenerPasoResponse>, RequestSecurity<VincularCuentasActivasReq>>(request);
            if (response.Codigo != 0)
            {
                throw new ExceptionWebApiClient(response.MensajeTecnico);
            }
            //return response.Datos;
        }


        [WebApiService("InsertarCuentasVinculadas")]
        public void InsertarCuentasVinculadas(RequestSecurity<InsertarCuentasVinculadasReq> request)
        {
            var response = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<ObtenerPasoResponse>, RequestSecurity<InsertarCuentasVinculadasReq>>(request);
            if (response.Codigo != 0)
            {
                throw new ExceptionWebApiClient(response.MensajeTecnico);
            }
            //return response.Datos;
        }


        [WebApiService("ConsultaDiasNoHabiles")]
        public DiasNoHabilesResponse ConsultaDiasNoHabiles(RequestSecurity<DiasNoHabilesRequest> req)
        {
            var clientePDC = DependencyFactory.Resolve<ICallWebApi>().CallWebApiMethod<Response<DiasNoHabilesResponse>, Request<DiasNoHabilesRequest>>(req)?.Datos;

            return clientePDC;
        }

        public T ValidarResponse<Resp, T>(Resp response, T datos) where Resp : Response<T>
        {
            if (response.Codigo != 0)
            {
                throw new ExceptionWebApiClient(response.MensajeTecnico);
            }
            return response.Datos;
        }
    }
}
