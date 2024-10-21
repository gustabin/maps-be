using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_OBTENER_OPERACION_FONDO", Package = Package.MapsTools, Owner = Owner.Maps)]
    internal class OperacionesDisponiblesFondosDbReq : BaseRequest, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_FONDO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 10)]
        public string CodigoFondo { get; set; }
    }
}
