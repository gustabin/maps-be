using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.DataAccess.DBResponse
{
    internal class OperacionesDisponiblesFondosDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "SUSCRIPCION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Suscripcion { get; set; }

        [DBFieldDefinition(Name = "RESCATE", ValueConverter = typeof(ResponseConvert<string>))]
        public string Rescate { get; set; }
    }
}
