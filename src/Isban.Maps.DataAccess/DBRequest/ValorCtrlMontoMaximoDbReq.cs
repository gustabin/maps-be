
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_COMP_DEFINICION_CUR", Package = Package.ComponenteMontoMaximo, Owner = Owner.Maps)]
    internal class ValorCtrlMontoMaximoDbReq : FormularioBaseReq, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdComponente { get; set; }
    }
}
