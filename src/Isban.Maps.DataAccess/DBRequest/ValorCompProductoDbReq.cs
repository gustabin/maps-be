
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteProducto, Owner = Owner.Maps)]
    internal class ValorCompProductoDbReq: BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdComponente { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FORMULARIO_ID", BindOnNull = true, Type = OracleDbType.Long)]
        public long? FormularioId { get; set; }
    }
}
