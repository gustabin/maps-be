using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Request
{
    [DataContract]
    public class ConsultaParametrizacionReq
    {
        [DataMember]
        public string NomParametro { get; set; }

        [DataMember]
        public string CodigoSistema { get; set; }
    }
}
