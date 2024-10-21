
namespace Isban.Maps.Configuration.Backend
{
    using Bussiness;
    using Bussiness.Wizard;
    using IBussiness;
    using Interception;
    using Isban.Mercados.Configuration;
    using Isban.Mercados.UnityInject;
    using Isban.Mercados.WebApiClient;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class BussinessConfiguration : ISetUp
    {
        public void Init()
        {
            DependencyFactory.RegisterType<ICallWebApi, CallWebApi>(
               new InjectionMember[]
               {
                    new Interceptor<VirtualMethodInterceptor>()
                    //TODO: hacer un interceptor para las web api.
                    //new InterceptionBehavior<BusinessInterceptor>()
               }
               );
            DependencyFactory.RegisterType<IMapsPDCServiciosBusiness, MapsServiciosPDCBusiness>(
             new InjectionMember[]
             {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
             }
             );
            DependencyFactory.RegisterType<IMapsServiciosBusiness, MapsServiciosBusiness>(
             new InjectionMember[]
             {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
             }
             );
            DependencyFactory.RegisterType<IServicesClient, ServicesClient>(
             new InjectionMember[]
             {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
             }
             );
            DependencyFactory.RegisterType<IServiceWebApiClient, ServiceWebApiClient>(
             new InjectionMember[]
             {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
             });
            DependencyFactory.RegisterType<IChequeoBusiness, ChequeoBusiness>(
               new InjectionMember[]
               {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
               }
               );
            DependencyFactory.RegisterType<IWizardBusiness, WizardBusiness>(
             new InjectionMember[]
             {
                    new Interceptor<VirtualMethodInterceptor>(),
                    new InterceptionBehavior<BusinessInterceptor>()
             }
             );
        }

        public void Dispose()
        {
            DependencyFactory.ClearContainer();
        }
    }
}
