namespace Isban.Maps.Entity.Response
{
    using Base;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    [DataContract]
    public class ClienteCuentaDDC : EntityBase
    {
        //     "CodigoBloqueo": Codigo del Bloqueo de la cuenta(si hubiere; sino null) FALTA
        //     "CalidadParticipacion": Indica si es Titular o Cotitular FALTA
        //     "OrdenParticipacion": Indica el Orden de la Participacion(1: Titular, luego orden decreciente para Cotitulares) FALTA

        [DataMember]
        public string NroCta { get; set; }

        [DataMember]
        public long TipoCta { get; set; }

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
        public string CalidadParticipacion { get; set; }

        [DataMember]
        public long OrdenParticipacion { get; set; }

        [DataMember]
        public TitularDDC[] Titulares { get; set; }
    }
}
