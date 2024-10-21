using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    [DataContract]
    public class OperacionesDisponiblesFondosResp
    {
        [DataMember]
        public string Suscripcion { get; set; }

        [DataMember]
        public string Rescate { get; set; }
    }

    [DataContract]
    public class OperacionesDisponiblesFondos
    {
        [DataMember]
        public List<string> Operaciones { get; set; }
    }
}
