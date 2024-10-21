namespace Isban.Maps.Entity.Response
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ConsultaPdcResponse
    {
        public ConsultaPdcResponse()
        {

        }
        
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Apellido { get; set; }
                
        [DataMember]
        public DateTime FechaNacimiento { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }

        [DataMember]
        public string CodTipoDocumento { get; set; }

        [DataMember]
        public string TipoDocumento { get; set; }

        [DataMember]
        public string SegmentoCliente { get; set; }

        [DataMember]
        public string TipoBanca { get; set; }

        [DataMember]
        public string Empleado { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public List<CuentaCliente> Cuentas { get; set; }
    }
}