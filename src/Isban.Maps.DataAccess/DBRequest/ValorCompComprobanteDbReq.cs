
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Common.Data;
    using Isban.Maps.DataAccess;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_COMP_DEFINICION_CUR", Package = Package.ComponenteComprobante, Owner = Owner.Maps)]
    internal class ValorCompComprobanteDbReq : BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Decimal)]
        public long IdComponente { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FORMULARIO_ID", BindOnNull = true, Type = OracleDbType.Decimal)]
        public long? FormularioId { get; set; }
    }
}
