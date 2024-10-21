
namespace Isban.Maps.DataAccess.DBRequest
{
    using Isban.Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteMoneda, Owner = Owner.Maps)]
    internal class ValorCtrlMonedaDbReq : FormularioBaseReq, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", Type = OracleDbType.Long)]
        public long IdComponente { get; set; }
    }
}
