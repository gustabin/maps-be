
namespace Isban.Maps.DataAccess.DBResponse
{
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;

    internal class ConsultaOrigenDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "ORIGEN", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? Origen { get; set; }
    }
}
