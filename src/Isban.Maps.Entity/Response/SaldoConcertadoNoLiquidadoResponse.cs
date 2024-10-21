namespace Isban.Maps.Entity.Response
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SaldoConcertadoNoLiquidadoResponse
    {
        [DataMember]
        public decimal? Saldo { get; set; }
    }
}