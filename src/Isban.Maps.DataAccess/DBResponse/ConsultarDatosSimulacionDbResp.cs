using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;

namespace Isban.Maps.DataAccess.DBResponse
{
    public class ConsultarDatosSimulacionDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "TEXTO_JSON", ValueConverter = typeof(ResponseConvert<string>))]
        public string TextoJson { get; set; }
    }
}
