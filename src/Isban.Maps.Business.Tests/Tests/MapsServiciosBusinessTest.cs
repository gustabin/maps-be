using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;

namespace Isban.Maps.Business.Tests.Tests
{
    [TestFixture]
    public class MapsServiciosBusinessTest
    {
        [SetUp]
        public void Init()
        {

            DependencyFactory.RegisterType<IMapsServiciosBusiness, MapsServiciosBusiness>(
            new InjectionMember[]
            {
                new Interceptor<VirtualMethodInterceptor>()
            }
            );

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

            DependencyFactory.RegisterType<IServicesClient, ServicesClientMock>(
                new InjectionMember[]
                {
                new Interceptor<VirtualMethodInterceptor>()
                }
                );
        }

        [Test]
        [TestCase("SAF", "RTL")]
        [TestCase("PDC", "RTL")]
        [TestCase("SAF", "BP")]
        public void AltaAdhesion_Business_ReturnCarga(string idServicio, string segmento)
        {
            var entity = new FormularioResponse()
            {
                Canal = "00",
                SubCanal = "0000",
                Cabecera = new Cabecera(),
                Estado = "carga",
                IdServicio = idServicio,
                Nombre = "frm-test",
                Segmento = segmento,
                OrdenOrigen = 0
            };

            var firma = new DatoFirmaMaps();

            var result = DependencyFactory.Resolve<IMapsServiciosBusiness>().AltaAdhesion(entity, firma);

            Assert.IsNotNull(result);
            Assert.That(result.Items.Count, Is.GreaterThan(3));
        }


        [Test]        
        [TestCase("SAF", "RTL")]
        public void AltaAdhesion_Simular_ReturnOk(string idServicio, string segmento)
        {
            var entity = new FormularioResponse()
            {
                Canal = "00",
                SubCanal = "0000",
                Cabecera = new Cabecera(),
                Estado = "carga",
                IdServicio = idServicio,
                Nombre = "frm-6",
                Segmento = segmento,
                OrdenOrigen = 0,
                FormularioId = 6

            };
            BusinessHelper.AgregarComponente(NombreComponente.Fecha, entity, NombreComponente.Fecha);

            var firma = new DatoFirmaMaps();

            var result = DependencyFactory.Resolve<IMapsServiciosBusiness>().AltaAdhesion(entity, firma);


        }
    }
}
