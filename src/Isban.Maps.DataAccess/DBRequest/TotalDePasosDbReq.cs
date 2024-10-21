using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ControlFormWizard, Owner = Owner.Maps)]
    public class TotalDePasosDbReq : BaseRequest, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SEGMENTO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 5)]
        public string Segmento { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 10)]
        public string IdServicio { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANAL", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 2)]
        public string Canal { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUBCANAL", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4)]
        public string SubCanal { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO", BindOnNull = true, Size = 30, Type = OracleDbType.Varchar2)]
        public string Estado { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FORMULARIO_ID", BindOnNull = true, Type = OracleDbType.Long)]
        public long? FormularioId { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 10)]
        public string Nup { get; set; }
    }
}
