using Isban.Maps.Entity.Responsabilidad;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Isban.Maps.Entity.Controles.Compuestos
{
    public class CuentaCompuesta : ControlSimple
    {
        public CuentaCompuesta()
            : base(new AsignarDatosCuentaCompuesta())
        {
            Items = new List<ControlSimple>();
        }

        [DataMember]
        public List<ControlSimple> Items { get; set; }
        
    }
}
