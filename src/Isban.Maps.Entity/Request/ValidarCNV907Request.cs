namespace Isban.Maps.Entity.Request
{
    using System;
    using Isban.Maps.Entity.Base;
    using System.Runtime.Serialization;
    [DataContract]
    public class ValidarCNV907Request : EntityBase
    {
        [DataMember]
        public string Nup { get; set; }
    }
}
