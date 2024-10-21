
namespace Isban.Maps.Entity.Controles.Customizados
{
    using Base;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class ItemServicio<T> : ItemBase<T>
    {
        [DataMember]        
        public string Footer { get; set; }

        [DataMember]
        public int CantAdhesiones { get; set; }

        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public string TipoServicio { get; set; }

        [DataMember]        
        public string Icono { get; set; }

        [DataMember]
        public int Posicion { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Bloqueado { get; set; }
    }
}
