
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Common.Data;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_COMP_DEFINICION_CUR", Package = Package.ComponenteDescripcionDinamica, Owner = Owner.Maps)]
    internal class ValorCompDescripcionDinamicaDbReq : FormularioBaseReq, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Decimal)]
        public long IdComponente { get; set; }
    }
}
