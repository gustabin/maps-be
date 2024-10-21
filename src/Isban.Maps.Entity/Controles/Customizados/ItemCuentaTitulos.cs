
namespace Isban.Maps.Entity.Controles.Customizados
{
    using Base;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System;

    [DataContract]
    public class ItemCuentaTitulos<T> : ItemBase<T>
    {
        [DataMember]
        public string[] Titulares { get; set; }

        [DataMember]
        public string NumeroCtaTitulo { get; set; }   
    }
}
