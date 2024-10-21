namespace Isban.Maps.Entity.Response
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Cliente
    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Apellido { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }

        [DataMember]
        public string CodDocumento { get; set; }

        [DataMember]
        public string TipoDocumento { get; set; }

        [DataMember]
        public long? CuentaTitulos { get; set; }

        [DataMember]
        public string NroCtaOperativa { get; set; }

        [DataMember]
        public string CodProducto { get; set; }

        [DataMember]
        public string CodSubproducto { get; set; }

        [DataMember]
        public string SucursalCtaOperativa { get; set; }

        [DataMember]
        public long TipoCtaOperativa { get; set; }

        [DataMember]
        public string TipoCtaOperativaDescripcion { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        public string SegmentoCuenta { get; set; }

        [DataMember]
        public string SegmentoCliente { get; set; }

        [DataMember]
        public string TipoBanca { get; set; }

        [DataMember]
        public string Empleado { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public long CuentaPdc { get; set; }

        [DataMember]
        public decimal SaldoPdc { get; set; }

        [DataMember]
        public string AceptaPdc { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public string CuentaApta { get; set; }

        [DataMember]
        public string BloqueadoCT { get; set; }

        [DataMember]
        public string BloqueadoCO { get; set; }

        [DataMember]
        public string IndicadorMw { get; set; }

        [DataMember]
        public DateTime FechaNacimiento { get; set; }

        [DataMember]
        public string NombreYApellido { get { return Apellido + ", " + Nombre; } }

        [DataMember]
        public string TipoCtaOperativaDesc
        {
            get { return string.Format("{0:D2}", TipoCtaOperativa); }
        }
    }
}