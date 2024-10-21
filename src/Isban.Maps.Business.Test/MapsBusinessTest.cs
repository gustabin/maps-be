
namespace Isban.Maps.Business.Test
{
    using Isban.Maps.Bussiness;
    using Isban.Maps.Configuration.Backend.Interception;
    using Isban.Maps.IBussiness;
    using Isban.Mercados.UnityInject;
    using IDataAccess;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using NUnit.Framework;
    using Entity.Response;
    using Mercados;
    using System;
    using Entity.Base;
    using System.Collections.Generic;
    using Entity.Controles.Compuestos;
    using Entity.Controles;
    using Mercados.WebApiClient;

    [TestFixture]
    public class MapsBusinessTest
    {

        [SetUp]
        public void SetUpTest()
        {
            DependencyFactory.RegisterType<IMapsServiciosBusiness, MapsServiciosBusiness>(
             new InjectionMember[]
             {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
             }
             );
            DependencyFactory.RegisterType<IMapsControlesDA, MapsServiciosMocksDA>(
            new InjectionMember[]
            {
                    new Interceptor<VirtualMethodInterceptor>()
            }
            );
            DependencyFactory.RegisterType<ICallWebApi, CallWebApiMock>(          //mockear el servicio.
               new InjectionMember[]
               {
                    new Interceptor<VirtualMethodInterceptor>()
               }
               );
        }

        [Test]
        public void ConsultaAdhesionOK()
        {
            var bss = DependencyFactory.Resolve<IMapsServiciosBusiness>();
            var frm = new FormularioResponse()
            {
                Nup = "00111111",
                IdServicio = "SAF",
                IdAdhesion = 2,
                Segmento = "RTL",
                FormularioId = 7

            };

            Assert.IsAssignableFrom(typeof(FormularioResponse), bss.ConsultaAdhesion(frm,new DatoFirmaMaps()));
        }

        [Test]
        public void BajaAdhesionOK()
        {
            var bss = DependencyFactory.Resolve<IMapsServiciosBusiness>();
            var frm = new FormularioResponse()
            {
                Nup = "00111111",
                IdServicio = "SAF",
                IdAdhesion = null,
                Segmento = "RTL"
            };

            Assert.IsInstanceOf<FormularioResponse>( bss.BajaAdhesion(frm, new DatoFirmaMaps()));

            //Assert.IsInstanceOf<FormularioResponse>(bss.BajaAdhesion(frm));
        }

        [Test]
        public void BajaAdhesionSimulacionOK()
        {
            var bss = DependencyFactory.Resolve<IMapsServiciosBusiness>();
            var frm = new FormularioResponse()
            {
                Nup = "00111111",
                IdServicio = "SAF",
                Segmento = "RTL"
            };

            Assert.IsInstanceOf<FormularioResponse>( bss.BajaAdhesion(frm, new DatoFirmaMaps()));
            //Assert.IsInstanceOf<FormularioResponse>(bss.BajaAdhesion(frm));
        }

        [Test]
        public void BajaAdhesionSimulacionPorIfOK()
        {
            var bss = DependencyFactory.Resolve<IMapsServiciosBusiness>();
            var frm = new FormularioResponse()
            {
                Nup = "00111111",
                IdServicio = "SAF",
                IdAdhesion = 2,
                Segmento = "RTL"
            };

            Assert.IsInstanceOf<FormularioResponse>( bss.BajaAdhesion(frm, new DatoFirmaMaps()));
            //Assert.IsInstanceOf<FormularioResponse>(bss.BajaAdhesion(frm));
        }

        [Test]
        public void AltaAdhesionOK()
        {
            var bss = DependencyFactory.Resolve<IMapsServiciosBusiness>();

            Assert.IsNotNull(bss.AltaAdhesion(new FormularioResponse()
            {
                IdServicio = "SAF",
                Canal = "00",
                SubCanal = "0000",
                Nup = "00112233",
                Segmento = "RTL",
                Items = GenerarControlesMocks()

            }, new DatoFirmaMaps {}));


        }

        private List<ControlSimple> GenerarControlesMocks()
        {
            var inputText = new InputText<string>();
            var inputNumber = new InputNumber<decimal>();
            var lista = new Lista<Item<string>>();
            var controles = new List<ControlSimple>() { inputNumber, inputText, lista };

            return controles;
        }
    }
}
