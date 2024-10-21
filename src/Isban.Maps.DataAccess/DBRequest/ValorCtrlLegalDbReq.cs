
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteLegal, Owner = Owner.Maps)]
    internal class ValorCtrlLegalDbReq: FormularioBaseReq, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Decimal)]
        public long IdComponente { get; set; }
    }
}
