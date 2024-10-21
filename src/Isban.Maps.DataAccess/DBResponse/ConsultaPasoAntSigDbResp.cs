using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;

namespace Isban.Maps.DataAccess.DBResponse
{
    internal class ConsultaPasoAntSigDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "FRM_SIGUIENTE", ValueConverter = typeof(ResponseConvert<long?>))]
        public long? FormularioId { get; set; }
    }
}
