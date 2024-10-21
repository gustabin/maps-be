
namespace Isban.Maps.DataAccess.DBResponse
{
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;

    internal class SaldoConcertadoNoLiquidadoDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "SALDO", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? Saldo { get; set; }
    }
}
