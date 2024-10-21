using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Maps.IDataAccess;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class GuardarConfirmacionJsonDbReq : BaseReq, IProcedureRequest, IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "", BindOnNull = true, Type = OracleDbType.Long, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdAdhesiones { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
        public string TextoJson { get; set; }

    }
}
