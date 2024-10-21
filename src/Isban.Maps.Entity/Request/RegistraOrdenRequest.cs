
namespace Isban.Maps.Entity.Request
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class RegistraOrdenRequest 
    {
        [DataMember]
        public long IdSimulacion { get; set; }

        [DataMember]
        public string CodEstadoProceso { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string IdServicio { get; set; }
    }
}
