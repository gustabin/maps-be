using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    [DataContract]
    public class ObtenerFormAdhesionesResp
    {
        [DataMember]
        public string TextoJson { get; set; }
    }
}
