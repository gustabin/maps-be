namespace Isban.Maps.Entity.Controles.Customizados
{
    using Base;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase para items del tipo Fondo
    /// </summary>
    /// <typeparam name="T">Tipo de la propiedad valor</typeparam>
    /// 
    [DataContract]
    public class ItemGrupoFondo<T> : ItemBase<T>
    {
        [DataMember]
        public string Grupo { get; set; }  

        public ItemGrupoFondo()
        {
            Items = new List<ItemFondo<T>>();
        }

        [DataMember]
        public List<ItemFondo<T>> Items { get; set; }
    }
}
