using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_CONSULTA_CONFIRMACION", Package = Package.GraphToolsWizard, Owner = Owner.Maps)]
    internal class ConsultarDatosConfirmacionDbReq : BaseRequest, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANAL", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 2)]
        public string Canal { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUBCANAL", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4)]
        public string SubCanal { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_SESSION", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string SessionId { get; set; }
    }
}
