using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Maps.IDataAccess;
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
    [ProcedureRequest("SP_OBTENER_FORM", Package = Package.MapsTools, Owner = Owner.Maps)]
    internal class ObtenerFormAdhesionesDbReq : BaseRequest, IProcedureRequest
    {
        /// <summary>
        /// The IdSimulacion
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_SIMULACION", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<int?>))]
        public long IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 40)]
        public string Servicio { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_FORMULARIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 40000)]
        public string TextoJson { get; set; }

    }
}
