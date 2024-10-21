using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_OBTENER_DETALLE_FONDO", Package = Package.MapsTools, Owner = Owner.Maps)]
    internal class DetalleDeFondoDbReq : BaseRequest, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OPERACION", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 20)]
        public string Operacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_FONDO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 10)]
        public string CodigoDeFondo { get; set; }
    }
}
