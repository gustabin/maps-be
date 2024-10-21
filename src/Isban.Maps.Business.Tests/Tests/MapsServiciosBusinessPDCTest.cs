using Isban.Maps.Business.Tests.Tests;
using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;

namespace Isban.Maps.Business.Tests
{
    public class MapsServiciosBusinessPDCTest
    {
        [SetUp]
        public void Init()
        {

            DependencyFactory.RegisterType<IMapsPDCServiciosBusiness, MapsServiciosPDCBusiness>(
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
        [TestCase("PDC", "RTL")]
        
        public void AltaAdhesion_PDC_ReturnCarga(string idServicio, string segmento)
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

            var result = DependencyFactory.Resolve<IMapsPDCServiciosBusiness>().PDCAltaAdhesion(entity, firma);

            Assert.IsNotNull(result);
            Assert.That(result.Items.Count, Is.GreaterThan(3));
        }     
    }
}
