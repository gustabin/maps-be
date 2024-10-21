namespace Isban.Maps.Entity.Response
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ConsultaRestriccionAdhesion
    {
        [DataMember]
        public string Mensaje { get; set; }

        [DataMember]
        public string Estado { get; set; }
    }
}
