using System.Runtime.Serialization;

namespace Isban.Maps.Entity.Base
{
    public class MapsBase : EntityBase
    {
        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Segmento { get; set; }     

        [DataMember]
        public string Estado { get; set; }      

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public string Nup { get; set; }
    }
}
