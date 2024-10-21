
namespace Isban.Maps.DataAccess.Base
{
    using IDataAccess;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    internal class FormularioBaseReq : BaseReq, IProcedureRequest, IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FORMULARIO_ID", BindOnNull = true, Type = OracleDbType.Long)]
        public long? FormularioId { get; set; }
    }
}
