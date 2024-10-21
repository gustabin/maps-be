namespace Isban.Maps.Entity.Response
{
    using Base;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    
    [DataContract]
    public class ClienteDDC : EntityBase
    {
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

        /// <summary>
        /// Atributo: CalidadParticipacion 
        /// Tipo de Dato: string 
        /// </summary>
        /// <value>
        /// The calidad participacion.
        /// </value>
        [DataMember]
        public string TipoPersona { get; set; }

        [DataMember]
        public ClienteCuentaDDC[] Cuentas { get; set; }
    }

    [DataContract]
    public class CuentasCustodiaResponse
    {


        [DataMember]
        public List<CuentasCustodia> CuentasCustodia { get; set; }


    }

    [DataContract]
    public class CuentasCustodia
    {
        [DataMember]
        public List<CuentaOperativa> CuentasOperativas { get; set; }

        [DataMember]
        public string IdCuentaCustodia { get; set; }
        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public string Alias { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public List<TitularCuentaCustodia> Titulares { get; set; }


    }


    [DataContract]
    public class CuentaOperativa
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Moneda { get; set; }
        [DataMember]
        public string Sucursal { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public string Producto { get; set; }
        [DataMember]
        public string Subproducto { get; set; }

    }



    [DataContract]
    public class TitularCuentaCustodia
    {

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Apellido { get; set; }


        [DataMember]
        public string Participacion { get; set; }

    }
}