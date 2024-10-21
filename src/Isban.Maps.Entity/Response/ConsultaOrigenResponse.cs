namespace Isban.Maps.Entity.Response
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ConsultaOrigenResponse
    {
        [DataMember]
        public long? Origen { get; set; }
    }
}