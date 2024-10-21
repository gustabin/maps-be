using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Interfaces;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Isban.Maps.Entity.Base
{
    [DataContract]
    public abstract class ItemBase<T> :  IValor<T>, IItem
    {
        [DataMember]
        public T Valor { get; set; }

        [DataMember]
        public string Desc { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ValorPadre { get; set; }

        [DataMember]
        public bool Seleccionado { get; set; }
    }
}
