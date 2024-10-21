namespace Isban.Maps.Entity.Controles.Customizados
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase para items del tipo SAF
    /// </summary>
    /// <typeparam name="T">Tipo de la propiedad valor</typeparam>
    /// 

    [DataContract]
    public class ItemTooltip
    {
        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public string Contenido { get; set; }
    }
}
