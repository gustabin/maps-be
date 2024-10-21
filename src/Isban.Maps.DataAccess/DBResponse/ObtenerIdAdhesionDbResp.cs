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
    public class ObtenerIdAdhesionDbResp : BaseResponse
    {
        [DBFieldDefinition(Name = "COD_ALTA_ADHESION", ValueConverter = typeof(ResponseConvert<long>))]
        public long IdAltaAdhesion { get; set; }
    }
}
