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
    internal class ObtenerFormAdhesionesDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "P_FORMULARIO", ValueConverter = typeof(ResponseConvert<string>))]
        public string TextoJson { get; set; }

        [DBFieldDefinition(Name = "P_COD_RES", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodError { get; set; }

        [DBFieldDefinition(Name = "P_DESCR_ERR", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescError { get; set; }
    }
}
