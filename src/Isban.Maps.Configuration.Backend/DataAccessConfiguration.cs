
namespace Isban.Maps.Configuration.Backend
{
    using Mercados.Configuration;
    using IDataAccess;
    using Mercados.UnityInject;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using Interception;
    using DataAccess;

    public class DataAccessConfiguration : ISetUp
    {
        public void Init()
        {
            DependencyFactory.RegisterType<IMapsControlesDA, MapsServiciosDataAccess>(new PerThreadLifetimeManager(),
              new InjectionMember[]
              {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<DataAccessInterceptor>()
              }
              );
            DependencyFactory.RegisterType<IOpicsDA, OpicsDataAccess>(new PerThreadLifetimeManager(),
              new InjectionMember[]
              {
                     new Interceptor<VirtualMethodInterceptor>(),
                     new InterceptionBehavior<DataAccessInterceptor>()
              }
              );
            DependencyFactory.RegisterType<ICanalesIatxDA, CanalesIatxDataAccess>(new PerThreadLifetimeManager(),
              new InjectionMember[]
              {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<DataAccessIatxInterceptor>()
              }
              );
            DependencyFactory.RegisterType<ISmcDA, SmcDataAccess>(new PerThreadLifetimeManager(),
           new InjectionMember[]
           {
                     new Interceptor<VirtualMethodInterceptor>(),
                     new InterceptionBehavior<DataAccessInterceptor>()
           }
           );
           DependencyFactory.RegisterType<IDatosClienteDA, DatosClienteDA>(new PerThreadLifetimeManager(),
           new InjectionMember[]
           {
                     new Interceptor<VirtualMethodInterceptor>(),
                     new InterceptionBehavior<DataAccessInterceptor>()
           }
           );
        }

        public void Dispose()
        {
            DependencyFactory.ClearContainer();

        }
    }
}
