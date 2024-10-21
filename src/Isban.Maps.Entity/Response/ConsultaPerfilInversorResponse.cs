using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    [DataContract]
    public class ConsultaPerfilInversorResponse
    {
        [DataMember]
        public int IdPerfil { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public DateTime? FechaDesde { get; set; }

        [DataMember]
        public DateTime? FechaHasta { get; set; }

        [DataMember]
        public int? DiasVencimiento { get; set; }

        [DataMember]
        public string TextoPerfil { get; set; }
    }
}
