
namespace Isban.Maps.DataAccess.DBResponse
{
    using Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Mercados.DataAccess.ConverterDBType;

    internal class ValoresFormularioDbResp : BaseResponse 
    {
        [DBFieldDefinition(Name = "ITEM_GROUP_ID", ValueConverter = typeof(ResponseConvert<string>))]
        public string ItemGroupId { get; set; }

        [DBFieldDefinition(Name = "ITEM_SUB_GROUP_ID",  ValueConverter = typeof(ResponseConvert<string>))]
        public string ItemSubGrupId { get; set; }

        [DBFieldDefinition(Name = "ID_SERVICIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string ServicioCodigo { get; set; }

        [DBFieldDefinition(Name = "NOMBRE_SERVICIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string ServicioNombre { get; set; }

        [DBFieldDefinition(Name = "ID_FORMULARIO", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? FormularioId { get; set; }

        [DBFieldDefinition(Name = "NOMBRE_FORMULARIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string FormularioNombre { get; set; }
        
        [DBFieldDefinition(Name = "DESC_FORMULARIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string FormularioDescripcion { get; set; }

        [DBFieldDefinition(Name = "ID_INSTANCIA_FORMULARIO", ValueConverter = typeof(ResponseConvert<long>))]
        public long BranchId { get; set; }

        [DBFieldDefinition(Name = "DESC_INSTANCIA_FORMULARIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string BranchDesc { get; set; }

        [DBFieldDefinition(Name = "ID_FORMULARIO_INSTANCIA", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? BranchFrmId { get; set; }

        [DBFieldDefinition(Name = "ID_COMPONENTE", ValueConverter = typeof(ResponseConvert<long>))]
        public long IdComponente { get; set; }

        [DBFieldDefinition(Name = "NOMBRE_COMPONENTE", ValueConverter = typeof(ResponseConvert<string>))]
        public string NombreComponente { get; set; }

        [DBFieldDefinition(Name = "DESC_COMPONENTE", ValueConverter = typeof(ResponseConvert<string>))]
        public string DefinicionDescripcion { get; set; }

        [DBFieldDefinition(Name = "ID_COMPONENTE_PADRE", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? ControlPadreId { get; set; }

        [DBFieldDefinition(Name = "ID_CONTROL_ATRIBUTO_COMPONENTE", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? ControlAtributoPadreId { get; set; }

        [DBFieldDefinition(Name = "ID_CONTROL_ATRIBUTO", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? ControlId { get; set; }

        [DBFieldDefinition(Name = "ID_ATRIBUTO", ValueConverter = typeof(ResponseConvert<long>))]
        public long AtributoId { get; set; }

        [DBFieldDefinition(Name = "DESC_ATRIBUTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string AtributoDesc { get; set; }

        [DBFieldDefinition(Name = "IN_TIENE_HIJO", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? HasChildren { get; set; }

        [DBFieldDefinition(Name = "ID_CONTROL_ATRIBUTO_PADRE", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? AtributoPadreId { get; set; }

        [DBFieldDefinition(Name = "VALOR_ATRIBUTO_COMPONENTE", ValueConverter = typeof(ResponseConvert<string>))]
        public string AtributoValor { get; set; }

        [DBFieldDefinition(Name = "VALOR_TIPO_DATO", ValueConverter = typeof(ResponseConvert<string>))]
        public string AtributoDataType { get; set; }

        [DBFieldDefinition(Name = "VALOR_ATRIBUTO_COMP_EXTENDIDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string ValorAtributoCompExtendido { get; set; }

        [DBFieldDefinition(Name = "VALOR_ATRIBUTO_COMP_EXTENDIDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string AtributoValorExtend { get; set; }
    }
}
