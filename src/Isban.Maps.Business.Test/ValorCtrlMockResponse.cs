using Isban.Maps.Entity.Interfaces;
using System.Xml;
using System.Xml.Serialization;

namespace Isban.Maps.Business.Test
{
    [XmlRoot(IsNullable = false)]
    public class ValorCtrlMockResponse : IResponseEntity
    {
        [XmlElement(ElementName = "ITEM_GROUP_ID")]
        public string ItemGroupId { get; set; }

        [XmlElement(ElementName = "ITEM_SUB_GROUP_ID")]
        public string ItemSubGrupId { get; set; }

        [XmlElement(ElementName = "ID_SERVICIO")]
        public string ServicioCodigo { get; set; }

        [XmlElement(ElementName = "NOMBRE_SERVICIO")]
        public string ServicioNombre { get; set; }

        [XmlElement(ElementName = "ID_FORMULARIO")]
        public long? FormularioId { get; set; }

        [XmlElement(ElementName = "NOMBRE_FORMULARIO")]
        public string FormularioNombre { get; set; }

        [XmlElement(ElementName = "DESC_FORMULARIO")]
        public string FormularioDescripcion { get; set; }

        [XmlElement(ElementName = "ID_INSTANCIA_FORMULARIO")]
        public decimal BranchId { get; set; }

        [XmlElement(ElementName = "DESC_INSTANCIA_FORMULARIO")]
        public string BranchDesc { get; set; }

        [XmlElement(ElementName = "ID_FORMULARIO_INSTANCIA")]
        public decimal? BranchFrmId { get; set; }

        [XmlElement(ElementName = "ID_COMPONENTE")]
        public decimal IdComponente { get; set; }

        [XmlElement(ElementName = "NOMBRE_COMPONENTE")]
        public string NombreControl { get; set; }

        [XmlElement(ElementName = "DESC_COMPONENTE")]
        public string DefinicionDescripcion { get; set; }

        [XmlElement(ElementName = "ID_COMPONENTE_PADRE")]
        public decimal? ControlPadreId { get; set; }

        [XmlElement(ElementName = "ID_CONTROL_ATRIBUTO_COMPONENTE")]
        public decimal? ControlAtributoPadreId { get; set; }

        [XmlElement(ElementName = "ID_CONTROL_ATRIBUTO")]
        public decimal ControlId { get; set; }

        [XmlElement(ElementName = "ID_ATRIBUTO")]
        public decimal AtributoId { get; set; }

        [XmlElement(ElementName = "DESC_ATRIBUTO")]
        public string AtributoDesc { get; set; }

        [XmlElement(ElementName = "IN_TIENE_HIJO")]
        public decimal? HasChildren { get; set; }

        [XmlElement(ElementName = "ID_CONTROL_ATRIBUTO_PADRE")]
        public decimal? AtributoPadreId { get; set; }

        [XmlElement(ElementName = "VALOR_ATRIBUTO_COMPONENTE")]
        public string AtributoValor { get; set; }

        [XmlElement(ElementName = "VALOR_TIPO_DATO")]
        public string AtributoDataType { get; set; }
    }
}
