using Isban.Maps.Entity.Base;
using System.Runtime.Serialization;

namespace Isban.Maps.Entity.Request
{
    [DataContract]
    public class DetalleDeFondoReq : EntityBase
    {
        [DataMember]
        public string Operacion { get; set; }

        [DataMember]
        public string CodigoDeFondo { get; set; }

    }
}
