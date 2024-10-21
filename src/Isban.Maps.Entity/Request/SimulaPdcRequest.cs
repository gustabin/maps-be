
namespace Isban.Maps.Entity.Request
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SimulaPdcRequest
    {
        [DataMember]
        public long IDSimCuentaPDC { get; set; }

        [DataMember]
        public string NUP { get; set; }

        [DataMember]
        public long? CuentaTitulos { get; set; }

        [DataMember]
        public string NroCtaOperativa { get; set; }

        [DataMember]
        public string TipoCtaOperativa { get; set; }

        [DataMember]
        public string SucursalCtaOperativa { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        public DateTime FechaAlta { get; set; }

        [DataMember]
        public string Canal { get; set; }
        
        [DataMember]
        public string Subcanal { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Operacion { get; set; }

        [DataMember]
        public string Producto { get; set; }

        [DataMember]
        public string Subproducto { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public DateTime SiguienteDiaHabil { get; set; }
        
        [DataMember]
        public string Motivo { get; set; }
    }
}
