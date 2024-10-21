
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Isban.Common.Data;
    using Isban.Maps.DataAccess;
    using Isban.Mercados.DataAccess.OracleClient;
    using Mercados.DataAccess.ConverterDBType;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ACTUALIZO_JSON", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ServConsAdhesionActualizarJsonReq : BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Long, ValueConverter = typeof(RequestConvert<long>))]
        public long IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TEXTO_JSON", BindOnNull = true, Type = OracleDbType.Clob, ValueConverter = typeof(RequestConvert<string>))]
        public string TextoJson { get; set; }
    }
}
