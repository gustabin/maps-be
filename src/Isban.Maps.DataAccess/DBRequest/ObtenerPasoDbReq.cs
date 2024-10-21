using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ObtenerPasoAdhDbReq : BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "", BindOnNull = true, Type = OracleDbType.Long, ValueConverter = typeof(RequestConvert<long?>))]
        public long? PasoPuntual { get; set; }
    }
}
