
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ControlFormulario, Owner = Owner.Maps)]
    internal class ValorCtrlFormularioDbReq : FormularioBaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO", BindOnNull = true, Size = 30, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }

    }
}
