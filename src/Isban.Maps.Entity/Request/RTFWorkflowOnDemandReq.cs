using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Request
{
    public class RTFWorkflowOnDemandReq
    {

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string CuentaTitulo { get; set; }
    }
}
