
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("LOAD_ATITS", Package = Package.BpServiciosA3, Owner = Owner.BCAINV)]
    public class ConsultaLoadAtisDbReq : BcainvRequestBase, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_BP", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Long)]
        public long? CuentaBp { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Long)]
        public long? Nup { get; set; }
    }
}
