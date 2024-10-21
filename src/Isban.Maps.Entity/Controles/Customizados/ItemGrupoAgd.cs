
namespace Isban.Maps.Entity.Controles.Customizados
{
    using Base;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ItemGrupoAgd 
    {
       
        public ItemGrupoAgd()
        {
            Items = new List<ItemFondoAgd>();
        }
        public ItemGrupoAgd(ItemFondoAgd item)
        {
            Items = new List<ItemFondoAgd>();
            Items.Add(item);
        }
        [DataMember]
        public string Grupo { get; set; }

        [DataMember]
        public List<ItemFondoAgd> Items { get; set; }
    }
    /*
    [DataContract]
    public class ItemSubGrupoAgd
    {
        public ItemSubGrupoAgd()
        {
            Items = new List<ItemFondoAgd>();
        }

        [DataMember]
        public string SubGrupo { get; set; }

        [DataMember]
        public List<ItemFondoAgd> Items { get; set; }  
    }*/

    [DataContract]
    public class ItemFondoAgd : ItemBase<string>
    {  
        [DataMember]
        public string CodMonedaEmision { get; set; }

        [DataMember]
        public string FactSheet { get; set; }

        [DataMember]
        public string ReglamentoFondo { get; set; }

        [DataMember]
        public TooltipAgd ToolTip { get; set; }

    }

    [DataContract]
    public class TooltipAgd : ItemTooltip
    {   /*
        [DataMember]
        public ItemGeneric CheckBox { get; set; }*/
    }
}
