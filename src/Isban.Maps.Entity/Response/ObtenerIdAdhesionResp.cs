using System.Runtime.Serialization;

namespace Isban.Maps.Entity.Response
{
    [DataContract]
    public class ObtenerIdAdhesionResp
    {
        [DataMember]
        public string IdAltaAdhesion { get; set; }

        [DataMember]
        public long IdCuentaPdc { get; set; }
    }
}
