
namespace Isban.Maps.Entity.Controles.Customizados
{
    using Base;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    public class ItemMoneda <T>: ItemBase<T>
    {        
        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal TipoCambio { get; set; }

        [DataMember]
        public string CodigoIso { get; set; }
    }
}
