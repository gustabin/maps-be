
namespace Isban.Maps.DataAccess.DBResponse
{
    using Common.Data;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    internal class ValorConsDeAdhesionesDbResp : BaseResponse
    {   
        [DBFieldDefinition(Name = "TEXTO_JSON", ValueConverter = typeof(ResponseConvert<string>))]
        public string TextoJson { get; set; }

        [DBFieldDefinition(Name = "ESTADO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Estado { get; set; }

        [DBFieldDefinition(Name = "COD_FONDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodigoFondo { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<decimal>))]
        public decimal CuentaTitulo { get; set; }

        [DBFieldDefinition(Name = "NRO_CTA_OPER", ValueConverter = typeof(ResponseConvert<decimal>))]
        public decimal CuentaDebito { get; set; }
    }
}
