using Isban.Maps.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Request
{
    [DataContract]
    public class ObtenerPasoReq : EntityBase
    {
        // TODO: IDFormulario - IdServicio - Segmento pueden pasarse a un request base.
        [DataMember]
        long PasoPuntual { get; set; }

        [DataMember]
        public long IdFormulario { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Segmento { get; set; }
    }
}
