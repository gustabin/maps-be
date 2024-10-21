using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.LegalesFondosAgs, Owner = Owner.Maps)]
    internal class ObtenerLegalesFondosAgdDbReq : BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_FONDO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 5)]
        public string CodigoDeFondo { get; set; }              

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FORMULARIO_ID", BindOnNull = true, Type = OracleDbType.Long)]
        public long? FormularioId { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdComponente { get; set; }
    }
}
