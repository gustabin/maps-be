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
    public class ObtenerLegalesFondosAgd : MapsBase
    {
        [DataMember]
        public string CodigoDeFondo { get; set; }
        
        [DataMember]
        public long? FormularioId { get; set; }

        [DataMember]
        public long IdComponente { get; set; }
    }
}
