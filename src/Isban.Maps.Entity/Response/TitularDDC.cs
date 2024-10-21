using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    public class TitularDDC
    {
        [DataMember]
        public string CuentaTitulos { get; set; }

        [DataMember]
        public string SucursalCtaOperativa { get; set; }

        [DataMember]
        public int? TipoCtaOperativa { get; set; }

        [DataMember]
        public string NroCtaOperativa { get; set; }

        [DataMember]
        public string CodMoneda { get; set; }

        [DataMember]
        public string CalidadParticipacion { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string NombreFantasia { get; set; }

        [DataMember]
        public string PrimerApellido { get; set; }

        [DataMember]
        public string SegundoApellido { get; set; }

        [DataMember]
        public string TipoDocumento { get; set; }

        [DataMember]
        public long NroDocumento { get; set; }

        [DataMember]
        public string CodProducto { get; set; }

        [DataMember]
        public string CodSubproducto { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
    }
}
