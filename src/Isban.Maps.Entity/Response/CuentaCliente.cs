namespace Isban.Maps.Entity.Response
{
    using Base;
    using System.Runtime.Serialization;

    [DataContract]
    public class CuentaCliente : EntityBase
    {
        [DataMember]
        public string NroCta { get; set; }

        [DataMember]
        public long? TipoCta { get; set; }

        [DataMember]
        public string DescripcionTipoCta { get; set; }

        [DataMember]
        public string CodProducto { get; set; }

        [DataMember]
        public string CodSubproducto { get; set; }

        [DataMember]
        public string SucursalCta { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        public string SegmentoCuenta { get; set; }

        [DataMember]
        public string CuentaBloqueada { get; set; }

        [DataMember]
        public string CodigoBloqueo { get; set; }

        [DataMember]
        public string MotivoBloqueo { get; set; }

        [DataMember]
        public string DetalleBloqueo { get; set; }

        [DataMember]
        public string CalidadParticipacion { get; set; }

        [DataMember]
        public long OrdenParticipacion { get; set; }
    }
}