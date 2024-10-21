
namespace Isban.Maps.Entity.Request
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ConsultaLoadAtisRequest
    {
        [DataMember]
        public long? Nup { get; set; }

        [DataMember]
        public long? CuentaBp { get; set; }

    }
}
