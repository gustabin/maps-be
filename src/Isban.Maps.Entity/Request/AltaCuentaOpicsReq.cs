using Isban.Maps.Entity.Base;
using System.Runtime.Serialization;

namespace Isban.Maps.Entity.Request
{
    [DataContract]
    public class AltaCuentaOpicsReq : EntityBase
    {
        [DataMember]
        public long? CuentaTitulo { get; set; }
    }
}
