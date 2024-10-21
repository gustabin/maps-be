using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    [DataContract]
    public class ConsultaFondosAGDResponse
    {
        [DataMember]
        public string CodigoFondo { get; set; }

        [DataMember]
        public string DescripcionFondo { get; set; }

        [DataMember]
        public string MonedaFondo { get; set; }

        [DataMember]
        public bool PuedeRescatar { get; set; }

        [DataMember]
        public bool PuedeSuscribir { get; set; }

        public string Rescate { get; set; }

        public string Suscripcion { get; set; }

    }
}
