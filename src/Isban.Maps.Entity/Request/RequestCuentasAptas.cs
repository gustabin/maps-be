
namespace Isban.Maps.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using System.Runtime.Serialization;

    [DataContract]
    public class RequestCuentasAptas : EntityBase
    {
        [DataMember]
        public string DatoConsulta { get; set; }
        
        [DataMember]
        public string TipoBusqueda { get; set; }
        
        [DataMember]
        public string CuentasRespuesta { get; set; }
        
        [DataMember]
        public string Segmento { get; set; }
        
        [DataMember]
        public string IdServicio { get; set; }
        
        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }
    }
}
