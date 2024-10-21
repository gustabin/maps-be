
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("", Package = Package.ControlFormulario, Owner = Owner.Maps)] /// -----> TODO: Cambiar por el SP que corresponde
    internal class ValorCtrlFormularioCargaCompletaPorServicioDbReq : FormularioBaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO", BindOnNull = true, Size = 30, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }
    }
}
