
namespace Isban.Maps.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class GetCuentas : EntityBase
    {        
        [DataMember]
        public string DatoConsulta { get; set; }
        
        [DataMember]
        public string TipoBusqueda { get; set; }
        
        [DataMember]
        public string CuentasRespuesta { get; set; }
        
        [DataMember]
        public string Segmento { get; set; }
        
        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Titulares { get; set; }


        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Canal { get; set; }
        
        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public Cabecera Cabecera { get; set; }
    }

    [DataContract]
    public class DiasNoHabilesRequest : EntityBase
    {
        [DataMember]
        public string FiltroPais { get; set; }

    }

    [DataContract]
    public class DiasNoHabilesResponse 
    {
        [DataMember]
        public List<Feriado> ListaFeriados { get; set; }

    }

    [DataContract]
    public class Feriado
    {
        [DataMember]
        public string Pais { get; set; }

        [DataMember]
        public string Fecha { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

    }



}
