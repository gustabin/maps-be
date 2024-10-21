
namespace Isban.Maps.DataAccess.DBRequest
{
    using Isban.Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_REG_LOGS_FORMULARIO", Package = Package.ControlFormulario, Owner = Owner.Maps)]
    internal class LogFormularioDbReq : FormularioBaseReq, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TEXTO_JSON", BindOnNull = true, Type = OracleDbType.Clob)]
        public string TextoJson { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_SIMU_ADHE", BindOnNull = true, Type = OracleDbType.Long)]
        public long? CodSimuAdhe { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ALTA_ADHE", BindOnNull = true, Type = OracleDbType.Long)]
        public long? CodAltaAdhe { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_BAJA_ADHE", BindOnNull = true, Type = OracleDbType.Long)]
        public long? CodBajaAdhe { get; set; }

    }
}
