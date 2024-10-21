
namespace Isban.Maps.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using Isban.Maps.Entity.Response;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetClientePDC : EntityBase
    {
        [DataMember]
        public List<ConsultaPdcResponse> Clientes { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }
    }
}
