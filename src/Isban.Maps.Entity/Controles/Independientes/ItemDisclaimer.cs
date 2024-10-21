namespace Isban.Maps.Entity.Controles.Independientes
{
    using Base;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class ItemDisclaimer<T> : ItemBase<T>
    {
        [DataMember]
        public int Tipodisclaimer { get; set; }
    }
}
