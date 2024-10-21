using Isban.Maps.Business.Componente.Factory;
using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Bussiness.Factory.Formularios;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados.Extensions;
using Isban.Mercados.UnityInject;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using System.Collections.Generic;

namespace Isban.Maps.Business.Tests.Tests
{
    [TestFixture]
    public class FactoryCompTest
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
        public void Crear_FactoryComp_ReturnConfigAlias()
        {
            FormularioResponse formulario = new FormularioResponse();
            var comp = new InputText<string>() { Nombre = NombreComponente.Alias };

            var fac = new EstrategiaComp(new ConfigAlias(null, comp));
            fac.Crear();

            Assert.IsNotNull(comp.Nombre);
            Assert.IsNotNull(comp.Id);
            Assert.IsNotNull(comp.Tipo);
            Assert.IsNotNull(comp.Etiqueta);
            Assert.AreEqual(comp.Nombre, NombreComponente.Alias);

        }

        [Test]
        public void Crear_FactoryComp_ReturnConfigCtaOperativa()
        {
            FormularioResponse formulario = new FormularioResponse();
            formulario.Nup = "03007878";
            formulario.Segmento = "RTL";
            formulario.Cabecera = new Cabecera();

            var comp = new Lista<ItemCuentaOperativa<string>>()
            {
                Nombre = "TestcuentaOperativa",
                Id = "id-test",
                Tipo = "lista",
                Etiqueta = "Lista de cuentas operativas"
            };

            var fac = new EstrategiaComp(new ConfigCuentaOperativa(formulario, comp, null));
            fac.Crear();

            Assert.IsNotNull(comp.Nombre);
            Assert.IsNotNull(comp.Id);
            Assert.IsNotNull(comp.Tipo);
            Assert.IsNotNull(comp.Etiqueta);
            Assert.True(comp.Items.Count > 0);
            Assert.That(comp.Items, Has.Some.InstanceOf<ItemCuentaOperativa<string>>());

        }

        [Test]
        public void Crear_factoryComp_ReturnCtaTitulo()
        {
            FormularioResponse formulario = new FormularioResponse();
            formulario.Nup = "03007878";
            formulario.Segmento = "RTL";
            formulario.Cabecera = new Cabecera();

            var comp = new Lista<ItemCuentaTitulos<string>>()
            {
                Nombre = "TestCtaTitulo",
                Id = "id-test",
                Tipo = "lista",
                Etiqueta = "Lista de cuentas título"
            };

            var fac = new EstrategiaComp(new ConfigCuentaTitulo(formulario, comp, null));
            fac.Crear();

            Assert.IsNotNull(comp.Nombre);
            Assert.IsNotNull(comp.Id);
            Assert.IsNotNull(comp.Tipo);
            Assert.IsNotNull(comp.Etiqueta);
            Assert.True(comp.Items.Count > 0);
            Assert.That(comp.Items, Has.Some.InstanceOf<ItemCuentaTitulos<string>>());
        }

        [Test]
        public void Crear_factoryComp_ReturnCtaTituloPDC()
        {
            FormularioResponse formulario = new FormularioResponse();
            formulario.Nup = "03007878";
            formulario.Segmento = "RTL";
            formulario.Cabecera = new Cabecera();

            var comp = new Lista<ItemCuentaTitulos<string>>()
            {
                Nombre = "TestCtaTitulo",
                Id = "id-test",
                Tipo = "lista",
                Etiqueta = "Lista de cuentas título"
            };

            var fac = new EstrategiaComp(new ConfigCuentaTituloPDC(formulario, comp, new DatoFirmaMaps()));
            fac.Crear();

            Assert.IsNotNull(comp.Nombre);
            Assert.IsNotNull(comp.Id);
            Assert.IsNotNull(comp.Tipo);
            Assert.IsNotNull(comp.Etiqueta);
            Assert.True(comp.Items.Count > 0);
            Assert.That(comp.Items, Has.Some.InstanceOf<ItemCuentaTitulos<string>>());
        }

        [Test]
        public void Crear_FactoryComp_ReturnConfigCtaOperativaPDC()
        {
            FormularioResponse formulario = new FormularioResponse();
            formulario.Nup = "03007878";
            formulario.Segmento = "RTL";
            formulario.Cabecera = new Cabecera();

            var comp = new Lista<ItemCuentaOperativa<string>>()
            {
                Nombre = "TestcuentaOperativa",
                Id = "id-test",
                Tipo = "lista",
                Etiqueta = "Lista de cuentas operativas"
            };

            var fac = new EstrategiaComp(new ConfigCuentataOperPDC(formulario, comp, new DatoFirmaMaps()));
            fac.Crear();

            Assert.IsNotNull(comp.Nombre);
            Assert.IsNotNull(comp.Id);
            Assert.IsNotNull(comp.Tipo);
            Assert.IsNotNull(comp.Etiqueta);
            Assert.True(comp.Items.Count > 0);
            Assert.That(comp.Items, Has.Some.InstanceOf<ItemCuentaOperativa<string>>());

        }

        [Test]
        public void CrearItems_Factory_ReturnOk()
        {
            Lista<ItemCuentaOperativa<string>> a = new Lista<ItemCuentaOperativa<string>>();
            var formulario = new FormularioResponse();

            formulario.Items.Add(new Lista<ItemCuentaOperativa<string>>());

            FormularioHelper.CrearItems(formulario, null);

        }
    }
}
