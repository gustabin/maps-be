using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_CONSULTA_ALTA", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    public class ObtenerIdAdhesionDbReq : BaseRequest, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_CUENTA_PDC", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long>))]
        public long IdCuentaPdc { get; set; }
    }
}
