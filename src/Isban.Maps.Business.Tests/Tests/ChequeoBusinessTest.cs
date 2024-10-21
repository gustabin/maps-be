using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Business.Tests.Tests
{
    [TestFixture]
    public class ChequeoBusinessTest
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

            DependencyFactory.RegisterType<IPlDA, PlDAMock>(
                new InjectionMember[]
                {
                    new Interceptor<VirtualMethodInterceptor>()
                }
                );
        }


        [Test]
        public void Cheque_Web_ReturnOk()
        {
            var chequeo = new ChequeoBusiness();
            var entity = new EntityBase();

            var resultado = chequeo.Chequeo(entity);

            Assert.That(resultado, !Has.Count.Zero);
        }
    }
}
