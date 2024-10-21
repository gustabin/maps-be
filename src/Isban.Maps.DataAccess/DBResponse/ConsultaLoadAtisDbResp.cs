namespace Isban.Maps.DataAccess.DBResponse
{
    using Common.Data;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    internal class ConsultaLoadAtisDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "CUENTA_BP", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? CuentaBp { get; set; }

        [DBFieldDefinition(Name = "CUENTA_ATIT", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? CuentaAtit { get; set; }

        [DBFieldDefinition(Name = "CNO", ValueConverter = typeof(ResponseConvert<long>))]
        public long Cno { get; set; }
    }
}