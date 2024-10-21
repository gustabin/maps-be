namespace Isban.Maps.Entity.Controles.Customizados
{
    using Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class ItemCuentaOperativa<T> : ItemBase<T>
    { 
        [DataMember]
        public string TipoCtaOperativa { get; set; }

        [DataMember]
        public string NumeroCtaOperativa { get; set; }

        [DataMember]
        public string Producto { get; set; }

        [DataMember]
        public string SubProducto { get; set; }

        [DataMember]
        public string SucursalCtaOperativa { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? SaldoCta { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Titulares { get; set; }
              
    }
}
