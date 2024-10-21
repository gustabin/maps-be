using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Compuestos;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Isban.Maps.Business.Tests
{
    [TestFixture]
    public class BusinessHelperTest
    {
        [SetUp]
        public void Init()
        {
            DependencyFactory.RegisterType<IMapsControlesDA, DataAccessMock>(
                new InjectionMember[]
                {
                    new Interceptor<VirtualMethodInterceptor>()
                }
                );

            DependencyFactory.RegisterType<IServiceWebApiClient, ServiceWebApiClientMock>(
                new InjectionMember[]
                {
                    new Interceptor<VirtualMethodInterceptor>()
                }
                );

            DependencyFactory.RegisterType<ICanalesIatxDA, CanalesIatxDAMock>(
                new InjectionMember[]
                {
                new Interceptor<VirtualMethodInterceptor>()
                }
                );

            DependencyFactory.RegisterType<IOpicsDA, OpicsDAMock>(
                new InjectionMember[]
                {
                new Interceptor<VirtualMethodInterceptor>()
                }
                );

            DependencyFactory.RegisterType<ISmcDA, SmcDAMock>(
                new InjectionMember[]
                {
                new Interceptor<VirtualMethodInterceptor>()
                }
                );
        }

        [TestCase(true, "cuenta uno|1|ARS,cuenta dos|2|ARS,cuenta tres|3|ARS")]
        [TestCase(false, "1,2,3")]
        public void ConcatenarCuentas_SAF_ReturnNoVacio(bool conInformacionAdicional, string resultadoEsperado)
        {
            ClienteCuentaDDC[] cuentas = new ClienteCuentaDDC[] {
            new ClienteCuentaDDC { SucursalCta="cuenta uno", NroCta="1", CodigoMoneda="ARS" },
            new ClienteCuentaDDC { SucursalCta="cuenta dos", NroCta="2", CodigoMoneda="ARS" },
            new ClienteCuentaDDC { SucursalCta="cuenta tres", NroCta="3", CodigoMoneda="ARS" }
            };

            var resultado = BusinessHelper.ConcatenarCuentas(cuentas, conInformacionAdicional);

            Assert.IsNotNull(resultado);
            Assert.IsNotEmpty(resultado);
            Assert.AreEqual(resultado, resultadoEsperado);
        }

        [Test]
        public void ValidarCuentasBloquedas_SAF_ReturnNotNull()
        {
            ClienteCuentaDDC[] cuentas = new ClienteCuentaDDC[] {
            new ClienteCuentaDDC { SucursalCta="cuenta uno", NroCta="1", CodigoMoneda="ARS", CuentaBloqueada=TipoEstado.CuentaBloqueada },
            new ClienteCuentaDDC { SucursalCta="cuenta dos", NroCta="2", CodigoMoneda="ARS" ,CuentaBloqueada= TipoEstado.CuentaBloqueada},
            new ClienteCuentaDDC { SucursalCta="cuenta tres", NroCta="3", CodigoMoneda="ARS", CuentaBloqueada = TipoEstado.CuentaBloqueada }
            };

            var resultado = BusinessHelper.ValidarCuentasBloquedas(cuentas);

            Assert.IsNotNull(resultado);
        }

        [Test]
        public void ValidarCuentasBloquedas_SAF_TodasBloqueadas()
        {
            ClienteCuentaDDC[] cuentas = new ClienteCuentaDDC[] {
            new ClienteCuentaDDC { SucursalCta="cuenta uno", NroCta="1", CodigoMoneda="ARS", CuentaBloqueada=TipoEstado.CuentaBloqueada },
            new ClienteCuentaDDC { SucursalCta="cuenta dos", NroCta="2", CodigoMoneda="ARS" ,CuentaBloqueada= TipoEstado.CuentaBloqueada},
            new ClienteCuentaDDC { SucursalCta="cuenta tres", NroCta="3", CodigoMoneda="ARS", CuentaBloqueada = TipoEstado.CuentaBloqueada }
            };

            var resultado = BusinessHelper.ValidarCuentasBloquedas(cuentas);

            Assert.Zero(resultado.Length);
        }

        [Test]
        public void ValidarCuentasBloquedas_ReturnNoBloqueadas()
        {
            ClienteCuentaDDC[] cuentas = new ClienteCuentaDDC[] {
            new ClienteCuentaDDC { SucursalCta="cuenta uno", NroCta="1", CodigoMoneda="ARS", CuentaBloqueada="N" },
            new ClienteCuentaDDC { SucursalCta="cuenta dos", NroCta="2", CodigoMoneda="ARS" ,CuentaBloqueada= TipoEstado.CuentaBloqueada},
            new ClienteCuentaDDC { SucursalCta="cuenta tres", NroCta="3", CodigoMoneda="ARS", CuentaBloqueada = TipoEstado.CuentaBloqueada }
            };

            var resultado = BusinessHelper.ValidarCuentasBloquedas(cuentas);

            Assert.IsTrue(resultado.Length > 0);

        }

        [Test]
        [TestCase("SAF")]
        [TestCase("PDC")]
        public void ValidarCuentas_ReturnCuentasNoValidas(string servicio)
        {
            ClienteCuentaDDC[] cuentas = new ClienteCuentaDDC[] {
            new ClienteCuentaDDC { SucursalCta="1", NroCta="1", CodigoMoneda="ARS", CuentaBloqueada="N" },
            new ClienteCuentaDDC { SucursalCta="2", NroCta="1", CodigoMoneda="ARS" ,CuentaBloqueada= TipoEstado.CuentaBloqueada},
            new ClienteCuentaDDC { SucursalCta="3", NroCta="1", CodigoMoneda="ARS", CuentaBloqueada = TipoEstado.CuentaBloqueada }
            };

            FormularioResponse formulario = new FormularioResponse
            {
                IdServicio = servicio
            };

            //validar cuentas de DA tiene algunas condiciones para decir si es válida o no
            var resultado = BusinessHelper.ValidarCuentas(cuentas, formulario);

            Assert.IsNotNull(resultado);
            Assert.Zero(resultado.Length);
        }

        [Test]
        [TestCase("SAF", null, "0")]
        [TestCase("SAF", "0", null)]
        public void ValidarCuentas_SAF_ReturnException(string servicio, string nroCta, string sucursal)
        {
            ClienteCuentaDDC[] cuentas = new ClienteCuentaDDC[] {
            new ClienteCuentaDDC { SucursalCta=sucursal, NroCta=nroCta, CodigoMoneda="ARS", CuentaBloqueada="N" }
            };

            FormularioResponse formulario = new FormularioResponse
            {
                IdServicio = servicio
            };

            Assert.Throws(typeof(Exception), () =>
                            {
                                BusinessHelper.ValidarCuentas(cuentas, formulario);
                            });

        }

        [Test]
        [TestCase("PDC", "0", null)]
        [TestCase("AGD", "0", null)]
        public void ValidarCuentas_ReturnValidasOtrosServicios(string servicio, string nroCta, string sucursal)
        {
            ClienteCuentaDDC[] cuentas = new ClienteCuentaDDC[] {
            new ClienteCuentaDDC { SucursalCta=sucursal, NroCta=nroCta, CodigoMoneda="ARS", CuentaBloqueada="N" },
            new ClienteCuentaDDC { SucursalCta=sucursal, NroCta=nroCta, CodigoMoneda="ARS" ,CuentaBloqueada= TipoEstado.CuentaBloqueada},
            new ClienteCuentaDDC { SucursalCta=sucursal, NroCta=nroCta, CodigoMoneda="ARS", CuentaBloqueada = TipoEstado.CuentaBloqueada }
            };

            FormularioResponse formulario = new FormularioResponse
            {
                IdServicio = servicio
            };

            var resultado = BusinessHelper.ValidarCuentas(cuentas, formulario);

            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Length > 0);
        }

        [Test]
        [TestCase("Vigencia", true)]
        [TestCase("Fecha", false)]
        public void ValidarExistencia_Helper_ReturnTrueOrFalse(string componente, bool resultEsperado)
        {
            var formulario = new FormularioResponse();
            formulario.Items.Add(new FechaCompuesta
            {
                Nombre = NombreComponente.Vigencia
            });

            Assert.AreEqual(BusinessHelper.ValidarExistencia(componente, formulario), resultEsperado);
        }

        [Test]
        [TestCase(456789012, 789456)]
        [TestCase(456789000, 789457)]
        [TestCase(456789001, 789455)]
        public void ValidarCuentas_BP_ReturnValida(long ctaOper, long expectedResult)
        {
            var cuentas = new List<ConsultaLoadAtisResponse> {
             new ConsultaLoadAtisResponse { CuentaBp=123456789012, CuentaAtit=expectedResult},
             new ConsultaLoadAtisResponse { CuentaBp=123456789000, CuentaAtit=expectedResult},
             new ConsultaLoadAtisResponse { CuentaBp=123456789001, CuentaAtit=expectedResult}
            };

            var result = BusinessHelper.ValidarCuentas(cuentas, ctaOper);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, expectedResult);

        }

        [Test]
        [TestCase(456789011)]
        [TestCase(456789009)]
        public void ValidarCuentas_BP_ReturnException(long ctaOper)
        {
            var cuentas = new List<ConsultaLoadAtisResponse> {
             new ConsultaLoadAtisResponse { CuentaBp=123456789012, CuentaAtit=789456},
             new ConsultaLoadAtisResponse { CuentaBp=123456789000, CuentaAtit=789457},
             new ConsultaLoadAtisResponse { CuentaBp=123456789001, CuentaAtit=852693}
            };

            Assert.Throws(typeof(BusinessException), () => { BusinessHelper.ValidarCuentas(cuentas, ctaOper); });

        }

        [Test]
        [TestCase("Metilli", null, "Fernanda", "Metilli, Fernanda")]
        [TestCase(null, "Carulias", "Natalia", "Carulias, Natalia")]
        [TestCase("Ginzburg", null, "Malena", "Ginzburg, Malena")]
        [TestCase(null, null, "Dalia", "Dalia")]
        public void ObtenerTitulares_Helper_ReturnTitulares(string segundoApellido, string primerApellido, string nombre, string expectedResult)
        {
            TitularDDC[] titulares = new TitularDDC[] {
                                                    new TitularDDC {
                                                        SegundoApellido = segundoApellido ,
                                                        PrimerApellido =primerApellido,
                                                        Nombre =nombre}
                                                    };

            var resultado = BusinessHelper.ObtenerTitulares(titulares);

            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado[0], expectedResult);
        }

        [Test]

        public void AgregarComponente_Helper_ReturnOk()
        {
            var entity = new FormularioResponse();

            BusinessHelper.AgregarComponente(NombreComponente.FechaDesde, entity, NombreComponente.FechaDesde);

            Assert.That(entity.Items, Has.One.InstanceOf<Fecha>());
        }

        [Test]

        public void AgregarComponente_Helper_ItemsNull()
        {
            var entity = new FormularioResponse();
            entity.Items = null;

            BusinessHelper.AgregarComponente(NombreComponente.Fecha, entity, NombreComponente.Fecha);

            Assert.IsNotNull(entity);
            Assert.IsNull(entity.Items);
        }

        [Test]

        public void AgregarComponenteDescripcion_Helper_ReturnOk()
        {
            var entity = new FormularioResponse();

            Assert.IsInstanceOf(typeof(DescripcionDinamica<string>), BusinessHelper.AgregarComponenteDescripcion(entity, 1, "saf"));
        }

        [Test]
        public void AgregarComponenteEstado_Helper_Agregado()
        {
            FormularioResponse entity = new FormularioResponse();
            string estado = "Activo";
            long idEstadoAdhesion = 1;

            entity.Items.Add(BusinessHelper.AgregarComponenteEstado(entity, estado, idEstadoAdhesion));

            Assert.IsNotNull(entity.Items);
            Assert.That(entity.Items, Has.Some.InstanceOf<EstadoAdhesion<string>>());

        }

        [Test]
        [TestCase("00", "0000")]
        [TestCase("04", "0099")]
        [TestCase("03", "1")]
        public void GenerarCabecera_Helper_OK(string canal, string subcanal)
        {
            var result = BusinessHelper.GenerarCabecera(canal, subcanal);

            Assert.NotNull(result);
            Assert.IsInstanceOf<Cabecera>(result);
        }

        [Test]
        public void GetAdhesiones_PDC_OK()
        {
            var reqSec = new RequestSecurity<RequestMaps>
            {
                Canal = "00",
                SubCanal = "0000",
                Datos = new RequestMaps
                {
                    Nup = "03007878",
                    IdServicio = "SAF",
                    Ip = "1.1.1.1",
                    Segmento = "RTL",
                    Usuario = "B11111"
                }
            };

            var result = BusinessHelper.GetAdhesiones(reqSec);

            Assert.NotNull(result);
            Assert.That(result, Has.Count.EqualTo(6));
        }

        [Test]
        [TestCase(CuentaPDC.Simular, "A", null)]
        [TestCase(CuentaPDC.Alta, "A", 123)]
        [TestCase(CuentaPDC.Simular, CuentaPDC.Baja, null)]
        [TestCase(CuentaPDC.Alta, CuentaPDC.Baja, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, null)]
        public void SimularAltaBajaAdhesionPDC_Helper_OK(string operacion, string accion, long? idPDC)
        {
            var entity = new FormularioResponse();
            var ctaOper = new Lista<ItemCuentaOperativa<string>>() { Nombre = NombreComponente.CuentaOperativa };
            var itemCtaOper = new ItemCuentaOperativa<string>
            {
                Seleccionado = true,
                CodigoMoneda = "ARS",
                Desc = "test",
                NumeroCtaOperativa = "1231456",
                TipoCtaOperativa = "8",
                SucursalCtaOperativa = "0"
            };
            ctaOper.Items.Add(itemCtaOper);

            var ctaTit = new Lista<ItemCuentaTitulos<string>>() { Nombre = NombreComponente.CuentaTitulo };
            var itemCtaTit = new ItemCuentaTitulos<string>
            {
                NumeroCtaTitulo = "12346",
                Seleccionado = true,
                Desc = "test",
            };

            ctaTit.Items.Add(itemCtaTit);

            var moneda = new Lista<ItemMoneda<string>>() { Nombre = NombreComponente.Moneda };
            var itemMoneda = new ItemMoneda<string>
            {
                CodigoIso = "ARS",
                Seleccionado = true,
                Desc = "Peso Argentino"
            };

            moneda.Items.Add(itemMoneda);

            entity.Items.Add(ctaOper);
            entity.Items.Add(ctaTit);
            entity.Items.Add(moneda);

            var firma = new DatoFirmaMaps();

            var result = BusinessHelper.SimularAltaBajaAdhesionPDC(entity, firma, operacion, accion, idPDC);

            Assert.IsInstanceOf<SimulaPdcResponse>(result);
        }

        [Test]
        [TestCase(CuentaPDC.Simular, "A", null)]
        [TestCase(CuentaPDC.Alta, "A", 123)]
        [TestCase(CuentaPDC.Simular, CuentaPDC.Baja, null)]
        [TestCase(CuentaPDC.Alta, CuentaPDC.Baja, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, null)]
        public void SimularAltaBajaAdhesionPDC_Helper_SinCuentaOperativa(string operacion, string accion, long? idPDC)
        {
            var entity = new FormularioResponse();


            var ctaTit = new Lista<ItemCuentaTitulos<string>>() { Nombre = NombreComponente.CuentaTitulo };
            var itemCtaTit = new ItemCuentaTitulos<string>
            {
                NumeroCtaTitulo = "12346",
                Seleccionado = true,
                Desc = "test",
            };

            ctaTit.Items.Add(itemCtaTit);

            var moneda = new Lista<ItemMoneda<string>>() { Nombre = NombreComponente.Moneda };
            var itemMoneda = new ItemMoneda<string>
            {
                CodigoIso = "ARS",
                Seleccionado = true,
                Desc = "Peso Argentino"
            };

            moneda.Items.Add(itemMoneda);

            entity.Items.Add(ctaTit);
            entity.Items.Add(moneda);

            var firma = new DatoFirmaMaps();

            var result = BusinessHelper.SimularAltaBajaAdhesionPDC(entity, firma, operacion, accion, idPDC);

            Assert.IsInstanceOf<SimulaPdcResponse>(result);
        }

        [Test]
        [TestCase(CuentaPDC.Simular, "A", null)]
        [TestCase(CuentaPDC.Alta, "A", 123)]
        [TestCase(CuentaPDC.Simular, CuentaPDC.Baja, null)]
        [TestCase(CuentaPDC.Alta, CuentaPDC.Baja, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, null)]
        public void SimularAltaBajaAdhesionPDC_Helper_SinCuentaTitulo(string operacion, string accion, long? idPDC)
        {
            var entity = new FormularioResponse();
            var ctaOper = new Lista<ItemCuentaOperativa<string>>() { Nombre = NombreComponente.CuentaOperativa };
            var itemCtaOper = new ItemCuentaOperativa<string>
            {
                Seleccionado = true,
                CodigoMoneda = "ARS",
                Desc = "test",
                NumeroCtaOperativa = "1231456",
                TipoCtaOperativa = "8",
                SucursalCtaOperativa = "0"
            };
            ctaOper.Items.Add(itemCtaOper);

            var moneda = new Lista<ItemMoneda<string>>() { Nombre = NombreComponente.Moneda };
            var itemMoneda = new ItemMoneda<string>
            {
                CodigoIso = "ARS",
                Seleccionado = true,
                Desc = "Peso Argentino"
            };

            moneda.Items.Add(itemMoneda);

            entity.Items.Add(ctaOper);

            entity.Items.Add(moneda);

            var firma = new DatoFirmaMaps();

            var result = BusinessHelper.SimularAltaBajaAdhesionPDC(entity, firma, operacion, accion, idPDC);

            Assert.IsInstanceOf<SimulaPdcResponse>(result);
        }

        [Test]
        [TestCase(CuentaPDC.Simular, "A", null)]
        [TestCase(CuentaPDC.Alta, "A", 123)]
        [TestCase(CuentaPDC.Simular, CuentaPDC.Baja, null)]
        [TestCase(CuentaPDC.Alta, CuentaPDC.Baja, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, null)]
        public void SimularAltaBajaAdhesionPDC_Helper_SinMoneda(string operacion, string accion, long? idPDC)
        {
            var entity = new FormularioResponse();
            var ctaOper = new Lista<ItemCuentaOperativa<string>>() { Nombre = NombreComponente.CuentaOperativa };
            var itemCtaOper = new ItemCuentaOperativa<string>
            {
                Seleccionado = true,
                CodigoMoneda = "ARS",
                Desc = "test",
                NumeroCtaOperativa = "1231456",
                TipoCtaOperativa = "8",
                SucursalCtaOperativa = "0"
            };
            ctaOper.Items.Add(itemCtaOper);

            var ctaTit = new Lista<ItemCuentaTitulos<string>>() { Nombre = NombreComponente.CuentaTitulo };
            var itemCtaTit = new ItemCuentaTitulos<string>
            {
                NumeroCtaTitulo = "12346",
                Seleccionado = true,
                Desc = "test",
            };

            ctaTit.Items.Add(itemCtaTit);

            entity.Items.Add(ctaOper);
            entity.Items.Add(ctaTit);


            var firma = new DatoFirmaMaps();

            var result = BusinessHelper.SimularAltaBajaAdhesionPDC(entity, firma, operacion, accion, idPDC);

            Assert.IsInstanceOf<SimulaPdcResponse>(result);
        }

        [Test]
        [TestCase(CuentaPDC.Simular, "A", null)]
        [TestCase(CuentaPDC.Alta, "A", 123)]
        [TestCase(CuentaPDC.Simular, CuentaPDC.Baja, null)]
        [TestCase(CuentaPDC.Alta, CuentaPDC.Baja, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, 123)]
        [TestCase(CuentaPDC.Procesar, CuentaPDC.Alta, null)]
        public void SimularAltaBajaAdhesionPDC_Helper_CompItemsNull(string operacion, string accion, long? idPDC)
        {
            var entity = new FormularioResponse();
            var ctaOper = new Lista<ItemCuentaOperativa<string>>() { Nombre = NombreComponente.CuentaOperativa };
            var itemCtaOper = new ItemCuentaOperativa<string>
            {
                Seleccionado = true,
                CodigoMoneda = "ARS",
                Desc = "test",
                NumeroCtaOperativa = "1231456",
                TipoCtaOperativa = "8",
                SucursalCtaOperativa = "0"
            };
            ctaOper.Items = null;

            var ctaTit = new Lista<ItemCuentaTitulos<string>>() { Nombre = NombreComponente.CuentaTitulo };
            var itemCtaTit = new ItemCuentaTitulos<string>
            {
                NumeroCtaTitulo = "12346",
                Seleccionado = true,
                Desc = "test",
            };

            ctaTit.Items = null;

            var moneda = new Lista<ItemMoneda<string>>() { Nombre = NombreComponente.Moneda };
            moneda.Items = null;

            entity.Items.Add(ctaOper);
            entity.Items.Add(ctaTit);
            entity.Items.Add(moneda);

            var firma = new DatoFirmaMaps();

            var result = BusinessHelper.SimularAltaBajaAdhesionPDC(entity, firma, operacion, accion, idPDC);

            Assert.IsInstanceOf<SimulaPdcResponse>(result);
        }

        [Test]
        [TestCase("USD", "USB")]
        [TestCase("ARS", "ARS")]
        public void TraducirUsdAUsb(string moneda, string esperado)
        {
            var result = BusinessHelper.TraducirUsdAUsb(moneda);

            Assert.AreEqual(result, esperado);
        }


        [Test]
        [TestCase("RTL", "03007878", "USD", "0")]
        [TestCase("RTL", "03007878", "USD", "1")]
        [TestCase("RTL", "03007878", "ARS", "0")]
        [TestCase("RTL", "03007878", "ARS", "1")]
        [TestCase("BP", "03007878", "ARS", "0")]
        [TestCase("BP", "03007878", "ARS", "1")]
        [TestCase("BP", "03007878", "USD", "0")]
        [TestCase("BP", "03007878", "USD", "1")]
        [TestCase("BP", "03007878", "USD", "1")]
        public void ObtenerSaldoCuenta_Helper_ReturnSaldo(string segmento, string nup, string moneda, string sucursal)
        {
            var cuenta = new ClienteCuentaDDC
            {
                CalidadParticipacion = "Participacion test",
                CodigoBloqueo = "1",
                CodigoMoneda = moneda,
                CodProducto = "111",
                CodSubproducto = "222",
                CuentaBloqueada = "N",
                DescripcionTipoCta = "Cuenta Test",
                NroCta = "9911223366",
                TipoCta = 0,
                SucursalCta = sucursal == "0" ? "0" : "1",
                OrdenParticipacion = 1,
                SegmentoCuenta = segmento
            };

            var result = BusinessHelper.ObtenerSaldoCuenta(cuenta, segmento, nup, "00", "0000", "1.1.1.1", "B999999");

            Assert.NotNull(result);
        }

        [Test]
        public void ObtenerSaldoCuenta_Helper_ReturnSaldoNull()
        {
            var cuenta = new ClienteCuentaDDC
            {
                CalidadParticipacion = "Participacion test",
                CodigoBloqueo = "1",
                CodigoMoneda = "ARS",
                CodProducto = "111",
                CodSubproducto = "222",
                CuentaBloqueada = "N",
                DescripcionTipoCta = "Cuenta Test",
                NroCta = null,
                TipoCta = 0,
                SucursalCta = "0",
                OrdenParticipacion = 1,
                SegmentoCuenta = "BP"
            };

            var result = BusinessHelper.ObtenerSaldoCuenta(cuenta, cuenta.SegmentoCuenta, "03007878", "00", "0000", "1.1.1.1", "B999999");

            Assert.IsNull(result);
        }

        [Test]
        [TestCase("frm1", null, "SAF", true)]
        [TestCase("frm2", 1, "SAF", true)]
        [TestCase("frm3", 2, "SAF", true)]
        [TestCase("frm1", null, null, false)]
        public void ObtenerSiguienteFormulario(string frmOrigen, long? esperado, string servicio, bool items)
        {
            var entity = new FormularioResponse
            {
                IdServicio = servicio
            };

            var item = new InputText<string> { Config = "jdjdj", Posicion = 1 };
            if (items)
                entity.Items.Add(item);

            var result = BusinessHelper.ObtenerSiguienteFormulario(entity, frmOrigen);

            Assert.AreEqual(esperado, result);
        }

        [Test]
        public void ObtenerSiguienteFormulario_Alta_ReturnExceptionNoHayFormulario()
        {
            var entity = new FormularioResponse
            {
                IdServicio = "SAF"
            };
            var item = new InputText<string> { Config = "jdjdj", Posicion = 1 };
            entity.Items.Add(item);

            Assert.Throws<BusinessException>(() => BusinessHelper.ObtenerSiguienteFormulario(entity, "frm4"));


        }
        [Test]
        [TestCase(true, "03007878", 1)]
        [TestCase(true, "11223344", 0)]
        [TestCase(false, "03007878", 1)]
        [TestCase(false, "0307878", 1)]
        [TestCase(null, "03007878", 1)]
        public void ObtenerCuentasPDC(bool? porConsultaAB, string nup, int? cantExpected)
        {
            var entity = new FormularioResponse
            {
                Nup = nup,
                Segmento = "RTL",
                IdServicio = "PDC",
                Cabecera = new Cabecera()
            };

            var firma = new DatoFirmaMaps();

            var result = BusinessHelper.ObtenerCuentasPDC(entity, firma, porConsultaAB);

            Assert.NotNull(result);
            Assert.True(result.Count >= cantExpected);
        }

        [Test]
        [TestCase("03007878", true)]
        [TestCase("03007878", false)]
        public void ObtenerCuentasPorTipo(string nup, bool incluirTitulares)
        {
            var entity = new FormularioResponse
            {
                Nup = nup,
                Segmento = "RTL",
                IdServicio = "PDC",
                Cabecera = new Cabecera()
            };

            var firma = new DatoFirmaMaps();

            var result = BusinessHelper.ObtenerCuentasPorTipo(entity, "9", firma, incluirTitulares);

            Assert.NotNull(result);
        }
    }
}
