
namespace Isban.Maps.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class InsertOrderRequest
    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string OperationType { get; set; }

        [DataMember]
        public decimal? CustodyAccount { get; set; }

        [DataMember]
        public decimal? Amount { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string SecurityId { get; set; }

        [DataMember]
        public decimal? DebitBranch { get; set; }

        [DataMember]
        public string DebitProduct { get; set; }

        [DataMember]
        public string DebitSubProduct { get; set; }

        [DataMember]
        public decimal? DebitAccountType { get; set; }

        [DataMember]
        public decimal? DebitOperativeAccount { get; set; }

        [DataMember]
        public decimal? CreditBranch { get; set; }

        [DataMember]
        public string CreditProduct { get; set; }

        [DataMember]
        public string CreditSubProduct { get; set; }

        [DataMember]
        public decimal CreditAccountType { get; set; }

        [DataMember]
        public decimal? CreditOperativeAccount { get; set; }
        
        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public string Segment { get; set; }

        [DataMember]
        public string Channel { get; set; }

        [DataMember]
        public string SubChannel { get; set; }

        [DataMember]
        public long MapsId { get; set; }
    }
}
