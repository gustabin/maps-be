using System;
using Isban.Mercados.WebApiClient;
using System.Collections.Generic;
using NUnit.Framework;
using Isban.Maps.Entity.Response;

namespace Isban.Maps.Business.Test
{
    internal class CallWebApiMock : ICallWebApi
    {
        public TRs CallWebApiMethod<TRs>(string uriCall = null, TipoJson tipoJson = TipoJson.Newtonsoft)
        {
            throw new NotImplementedException();
        }

        public void CallWebApiMethod<TRq>(TRq request, string uriCall = null, TipoJson tipoJson = TipoJson.Newtonsoft)
        {
            
        }

        public TRs CallWebApiMethod<TRs, TRq>(TRq request, string uriCall = null, TipoJson tipoJson = TipoJson.Newtonsoft)
        {
           var a = Activator.CreateInstance(typeof(TRs));
            return (TRs)a;
        }

        public void Clear()
        {
            
        }

        public Uri GetUriService(string serviceName)
        {
            throw new NotImplementedException();
        }
    }
}