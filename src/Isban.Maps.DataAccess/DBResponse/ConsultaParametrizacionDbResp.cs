
namespace Isban.Maps.DataAccess.DBResponse
{
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;

    internal class ConsultaParametrizacionDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "NOM_PARAMETRO", ValueConverter = typeof(ResponseConvert<string>))]
        public string NombreParametro { get; set; }

        [DBFieldDefinition(Name = "VALOR", ValueConverter = typeof(ResponseConvert<string>))]
        public string Valor { get; set; }

        [DBFieldDefinition(Name = "DES_PARAMETRO", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescripcionParametro { get; set; }

        [DBFieldDefinition(Name = "COD_SISTEMA", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodigoSistema { get; set; }

    }
}
