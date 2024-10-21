using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Independientes;
using Isban.Maps.IBussiness;
using System;
using System.Collections.Generic;

namespace Isban.Maps.Business.Tests.Tests
{
    public class ServicesClientMock : IServicesClient
    {
        public IList<ItemDisclaimer<string>> EvaluacionRiesgo(List<ControlSimple> list, string nup, DatoFirmaMaps firma)
        {
            throw new NotImplementedException();
        }

        public string GetMailXNup(string canal, string subcanal, string nup)
        {
            return "mailTest@test.com.ar";
        }

        public string GetPerfil(string nup, DatoFirmaMaps firma)
        {
            return "Perfil TEST";
        }
    }
}