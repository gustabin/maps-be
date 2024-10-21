
namespace Isban.Maps.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class UpdateOrderRequest
    {
        [DataMember]
        public long OrderId { get; set; }

        [DataMember]
        public long MapsId { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Obs { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
