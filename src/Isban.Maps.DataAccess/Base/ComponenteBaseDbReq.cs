
namespace Isban.Maps.DataAccess.Base
{
    using Isban.Common.Data;
    using Isban.Maps.IDataAccess;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    internal class ComponenteBaseDbReq : BaseReq, IProcedureRequest, IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdComponente { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FORMULARIO_ID", BindOnNull = true, Type = OracleDbType.Long)]
        public long? FormularioId { get; set; }
    }
}
