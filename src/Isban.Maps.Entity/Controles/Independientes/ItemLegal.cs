
namespace Isban.Maps.Entity.Controles.Independientes
{
    using Isban.Maps.Entity.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class ItemLegal<T> : ItemBase<T>
    {
        [DataMember]
        public List<ItemGeneric> Items { get; set; }

    }

    [DataContract]
    public class ItemGeneric
    {
        [DataMember]
        public bool Checked { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TextoLink { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public string Posicion { get; set; }

        [DataMember]
        public bool Requerido { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UrlLink { get; set; }
    }
}
