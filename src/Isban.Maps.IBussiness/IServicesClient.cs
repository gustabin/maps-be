using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Independientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.IBussiness
{
    public interface IServicesClient
    {
        string GetPerfil(string nup, DatoFirmaMaps firma);
        IList<ItemDisclaimer<string>> EvaluacionRiesgo(List<ControlSimple> list, string nup, DatoFirmaMaps firma);
        string GetMailXNup(string canal, string subcanal, string nup);
    }
}
