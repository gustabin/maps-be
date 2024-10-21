using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    [DataContract]
    public class DetalleDeFondoResp
    {
        [DataMember]
        public string HoraApertura { get; set; }

        [DataMember]
        public string HoraCierre { get; set; }

        [DataMember]
        public int MinValor { get; set; }

        [DataMember]
        public int MaxValor { get; set; }

        [DataMember]
        public string Informacion { get; set; }

        [DataMember]
        public string Operacion { get; set; }

        [DataMember]
        public string Moneda { get; set; }

        [DataMember]
        public string DescFondo { get; set; }
    }
}

