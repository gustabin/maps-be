using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Request
{
    [DataContract]
    public class ValidaRestriccionCMEPRequest
    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string TipoBono { get; set; }

        [DataMember]
        public string TipoControlPrestamosCheques { get; set; }

        [DataMember]
        public Cabecera Encabezado { get; set; }

    }
}
