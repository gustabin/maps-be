using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;

namespace Isban.Maps.DataAccess.DBResponse
{
    internal class DetalleDeFondoDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "HORA_APERTURA", ValueConverter = typeof(ResponseConvert<string>))]
        public string HoraApertura { get; set; }

        [DBFieldDefinition(Name = "HORA_CIERRE", ValueConverter = typeof(ResponseConvert<string>))]
        public string HoraCierre { get; set; }

        [DBFieldDefinition(Name = "MINVALOR", ValueConverter = typeof(ResponseConvert<int>))]
        public int MinValor { get; set; }

        [DBFieldDefinition(Name = "MAXVALOR", ValueConverter = typeof(ResponseConvert<int>))]
        public int MaxValor { get; set; }

        [DBFieldDefinition(Name = "INFORMACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Informacion { get; set; }

        [DBFieldDefinition(Name = "OPERACION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Operacion { get; set; }

        [DBFieldDefinition(Name = "MONEDA", ValueConverter = typeof(ResponseConvert<string>))]
        public string Moneda { get; set; }

        [DBFieldDefinition(Name = "DESC_FONDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescFondo { get; set; }
    }
}
