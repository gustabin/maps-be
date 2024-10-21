
namespace Isban.Maps.DataAccess.DBRequest
{
    using Isban.Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteListadoGenerico, Owner = Owner.Maps)]
    internal class ValorCtrlListadoGenericoDbReq : FormularioBaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CONFIG", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Config { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdComponente { get; set; }
    }
}
