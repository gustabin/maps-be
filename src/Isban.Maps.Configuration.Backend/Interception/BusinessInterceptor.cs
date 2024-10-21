
namespace Isban.Maps.Configuration.Backend.Interception
{
    using Entity.Controles;
    using Entity.Interfaces;
    using Mercados;
    using Mercados.LogTrace;
    using Mercados.UnityInject;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using System;
    using System.Transactions;

    public class BusinessInterceptor : InterceptorBase
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
            {
                foreach (var item in input.Arguments)
                {
                    var validacion = item as ControlSimple;
                    if (validacion != null && !validacion.Validar())
                    {
                        VirtualMethodReturn resultRequest;
                        if (item as IFormulario != null)
                        {
                            resultRequest = new VirtualMethodReturn(input, new MapsException("Error Controlado en Bussiness. Verificar el Formulario.", item as IFormulario));

                        }
                        else
                        {
                            resultRequest = new VirtualMethodReturn(input, new Exception("Error Controlado en Bussiness. Verificar InnerException.", item as Exception));
                        }
                        input.CreateMethodReturn(null);
                        return resultRequest;
                    }
                }

                IMethodReturn methodReturn = getNext()(input, getNext);

                if (methodReturn.Exception == null)
                    scope.Complete();

                InterceptExceptions(methodReturn, input);

                return methodReturn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="input"></param>
        private void InterceptExceptions(IMethodReturn result, IMethodInvocation input)
        {
            if (result.Exception != null)
            {
                if ((result.Exception as IsbanException) == null)
                {
                    var message = string.Format("Error {0} | Día y Hora: {1}. | Exception: {2}", this.GetType().Name,
                        DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), result.Exception);
                    LoggingHelper.Instance.Error(result.Exception, message);
                    result.Exception = new BusinessException("Error no controlado. Verificar InnerException.", result.Exception);
                }
            }
        }
    }
}
