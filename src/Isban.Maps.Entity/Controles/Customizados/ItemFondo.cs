namespace Isban.Maps.Entity.Controles.Customizados
{
    using Base;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase para items del tipo SAF
    /// </summary>
    /// <typeparam name="T">Tipo de la propiedad valor</typeparam>
    /// 
    [DataContract]
    public class ItemFondo<T> : ItemBase<T>
    {
        public ItemFondo()
        {
            ToolTip = new ItemTooltip();   
        }

        [DataMember]
        public string CodMonedaEmision { get; set; }

        [DataMember]
        public string FactSheet { get; set; }

        [DataMember]
        public string ReglamentoFondo { get; set; }

        [DataMember]
        public ItemTooltip ToolTip { get; set; }
          
    }

}
