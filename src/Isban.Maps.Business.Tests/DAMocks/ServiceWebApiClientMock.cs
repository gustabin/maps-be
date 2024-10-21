using System;
using System.Collections.Generic;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Mercados.Service.InOut;

namespace Isban.Maps.Business.Tests
{
    public class ServiceWebApiClientMock : IServiceWebApiClient
    {

        public List<ClienteCuentaDDC> ConsultaCuentasCustodia(RequestSecurity<GetCuentas> entity, bool filtarOperativas)
        {
            return new List<ClienteCuentaDDC>();
        }
        public void VincularCuentasActivas(RequestSecurity<VincularCuentasActivasReq> request)
        {
            throw new NotImplementedException();
        }


        public void InsertarCuentasVinculadas(RequestSecurity<InsertarCuentasVinculadasReq> request)
        {
            throw new NotImplementedException();
        }


        public ConsultaPerfilInversorResponse ConsultaPerfilInversor(RequestSecurity<ConsultaPerfilInversorRequest> request)
        {
            throw new NotImplementedException();
        }

        public List<Cliente> GetClientePDC(GetClientePDC req)
        {
            var cliente = new List<Cliente> {
             new Cliente { Nombre="Test 1", Apellido ="apellido 1" },
             new Cliente { Nombre="Test 2", Apellido ="apellido 2" },
             new Cliente { Nombre="Test 3", Apellido ="apellido 3" },
            };

            return cliente;
        }

        public ClienteDDC[] GetCuentas(RequestSecurity<GetCuentas> entity)
        {
            switch (entity.Datos.DatoConsulta)
            {
                case "11223344":
                    return null;


                default:

                    var result = new List<ClienteDDC>() {
             new ClienteDDC {
              Apellido = "Apellido1",
              Nombre ="Nombre1",
               SegmentoCliente="RTL",
                FechaNacimiento=DateTime.Now.AddYears(-40),
                 Cuentas=new ClienteCuentaDDC[] { new ClienteCuentaDDC {
                      NroCta="111456325898",
                       SucursalCta="0",
                       CodigoBloqueo=null,
                       CuentaBloqueada="N",
                        CodigoMoneda="ARS"
                 } }
             }
            };

                    return result.ToArray();
            }
        }

        public ClienteDDC[] GetCuentasFisicasYjuridicas(RequestSecurity<GetCuentas> entity)
        {
            switch (entity.Datos.DatoConsulta)
            {
                case "11223344":
                    return null;


                default:

                    var result = new List<ClienteDDC>() {
             new ClienteDDC {
              Apellido = "Apellido1",
              Nombre ="Nombre1",
               SegmentoCliente="RTL",
                FechaNacimiento=DateTime.Now.AddYears(-40),
                 Cuentas=new ClienteCuentaDDC[] { new ClienteCuentaDDC {
                      NroCta="111456325898",
                       SucursalCta="0",
                       CodigoBloqueo=null,
                       CuentaBloqueada="N",
                        CodigoMoneda="ARS"
                 } }
             }
            };

                    return result.ToArray();
            }
        }

        public List<Cliente> GetCuentasAptas(RequestSecurity<RequestCuentasAptas> entity)
        {
            var cliente = new List<Cliente>();

            switch (entity.Datos.DatoConsulta)
            {
                case "11223344":
                    cliente = new List<Cliente> {
             new Cliente { Nombre="Test 1", Apellido ="apellido 1" , Estado="Z", CuentaTitulos=11199999999},
             new Cliente { Nombre="Test 2", Apellido ="apellido 2" , Estado="A", CuentaTitulos=11199999999},
             new Cliente { Nombre="Test 3", Apellido ="apellido 3", Estado="SB" , CuentaTitulos=11199999999},
             new Cliente { Nombre="Test 1", Apellido ="apellido 1" , Estado="AC", CuentaTitulos=11199999999},
             new Cliente { Nombre="Test 2", Apellido ="apellido 2" , Estado="SA", CuentaTitulos=11199999999},
             new Cliente { Nombre="Test 3", Apellido ="apellido 3" , Estado="N", CuentaTitulos=11199999999}
            };
                    break;
                default:
                    cliente = new List<Cliente> {
             new Cliente { Nombre="Test 1", Apellido ="apellido 1" , Estado="Z", CuentaTitulos=111456325898},
             new Cliente { Nombre="Test 2", Apellido ="apellido 2" , Estado="A", CuentaTitulos=111456325898},
             new Cliente { Nombre="Test 3", Apellido ="apellido 3", Estado="SB" , CuentaTitulos=111456325898},
             new Cliente { Nombre="Test 1", Apellido ="apellido 1" , Estado="AC", CuentaTitulos=111456325898},
             new Cliente { Nombre="Test 2", Apellido ="apellido 2" , Estado="SA", CuentaTitulos=111456325898},
             new Cliente { Nombre="Test 3", Apellido ="apellido 3" , Estado="N", CuentaTitulos=111456325898}
            };
                    break;
            }


            return cliente;
        }

        public SimulaPdcResponse PDCSimulacionAltasBajas(RequestSecurity<SimulaPdcRequest> entity)
        {
            return new SimulaPdcResponse();
        }

        public DiasNoHabilesResponse ConsultaDiasNoHabiles(RequestSecurity<DiasNoHabilesRequest> req)
        {
            throw new NotImplementedException();
        }

        public ValidaRestriccionMEPResponse ValidarRestriccionMEP(RequestSecurity<ValidaRestriccionMEPRequest> request)
        {
            throw new NotImplementedException();
        }

        public LoadSaldosResponse ObtenerSaldoBP(RequestSecurity<LoadSaldosRequest> request)
        {
            throw new NotImplementedException();
        }

        public ValidarCNV907Response ValidarCNV907(RequestSecurity<ValidarCNV907Request> request)
        {
            throw new NotImplementedException();
        }

        public ValidaRestriccionMEPResponse ValidarRestriccionCMEP(RequestSecurity<ValidaRestriccionCMEPRequest> request)
        {
            throw new NotImplementedException();
        }
    }
}
