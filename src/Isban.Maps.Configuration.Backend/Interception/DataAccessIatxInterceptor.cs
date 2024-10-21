
namespace Isban.Maps.Configuration.Backend.Interception
{
    using System;
    using Isban.Maps.Entity.Extensiones;
    using Isban.Mercados;
    using Isban.Mercados.LogTrace;
    using Isban.Mercados.UnityInject;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class DataAccessIatxInterceptor : InterceptorBase
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn methodReturn = getNext()(input, getNext);
            InterceptExceptions(methodReturn, input);
            return methodReturn;
        }

        private void InterceptExceptions(IMethodReturn result, IMethodInvocation input)
        {
            if (result.Exception != null && !(result.Exception is IsbanException))
            {
                if (result.Exception.InnerException is Isban.Common.Data.IATX.IATXException)
                {
                    var iatxExc = (Isban.Common.Data.IATX.IATXException)result.Exception.InnerException;
                    result.Exception = new IatxCodeException(this.FillCode(iatxExc), this.FillMessage(iatxExc),
                        result.Exception);

                }
                else
                {
                    var message = string.Format("Error {0} | Día y Hora: {1}. | Exception: {2}", this.GetType().Name,
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), result.Exception);
                    LoggingHelper.Instance.Error(result.Exception, message);
                    result.Exception = new IatxException("Error no controlado. Verificar InnerException.",
                        result.Exception);
                }
            }
        }

        private string FillMessage(Isban.Common.Data.IATX.IATXException iatxExc)
        {
            string[] list = iatxExc.Message.Split('õ');
            if (list.Length > 5)
            {
                return list[4].Trim();
            }
            return string.Empty;
        }

        private long FillCode(Isban.Common.Data.IATX.IATXException iatxExc)
        {

            string[] list = iatxExc.Message.Split('õ');
            if (list.Length > 2)
            {
                var co = list[1].Trim();
                return long.Parse(co.Substring(co.Length - 8));
            }
            return -99;
        }
    }
}