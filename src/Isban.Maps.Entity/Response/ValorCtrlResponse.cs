namespace Isban.Maps.Entity.Response
{
    using Interfaces;
    using System.Runtime.Serialization;

    [DataContract]
    public class ValorCtrlResponse : IResponseEntity
    {
        [DataMember]
        public string ItemGroupId { get; set; }

        [DataMember]
        public string ItemSubGrupId { get; set; }

        [DataMember]
        public string ServicioCodigo { get; set; }

        [DataMember]
        public string ServicioNombre { get; set; }

        [DataMember]
        public long? FormularioId { get; set; }

        [DataMember]
        public string FormularioNombre { get; set; }

        [DataMember]
        public string FormularioDescripcion { get; set; }

        [DataMember]
        public long BranchId { get; set; }

        [DataMember]
        public string BranchDesc { get; set; }

        [DataMember]
        public long? BranchFrmId { get; set; }

        [DataMember]
        public long IdComponente { get; set; }

        [DataMember]
        public string NombreComponente { get; set; }

        [DataMember]
        public string DefinicionDescripcion { get; set; }

        [DataMember]
        public long? ControlPadreId { get; set; }

        [DataMember]
        public long? ControlAtributoPadreId { get; set; }

        [DataMember]
        public long? ControlId { get; set; }

        [DataMember]
        public long AtributoId { get; set; }

        [DataMember]
        public string AtributoDesc { get; set; }

        [DataMember]
        public long? HasChildren { get; set; }

        [DataMember]
        public long? AtributoPadreId { get; set; }

        [DataMember]
        public string AtributoValor { get; set; }

        [DataMember]
        public string AtributoDataType { get; set; }

        [DataMember]
        public string ValorAtributoCompExtendido { get; set; }
    }
}
