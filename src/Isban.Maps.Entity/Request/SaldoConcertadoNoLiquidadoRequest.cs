
namespace Isban.Maps.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SaldoConcertadoNoLiquidadoRequest
    {
        [DataMember]
        public string NroCtaOper { get; set; }

        [DataMember]
        public string SucCtaOper { get; set; }

        [DataMember]
        public decimal TipoCtaOper { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string Moneda { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
