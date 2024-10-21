using Isban.Common.Data;
using Isban.Maps.IDataAccess;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_SIGUIENTE_PASO", Package = Package.GraphToolsWizard, Owner = Owner.Maps)]
    internal class ConsultarPasoSiguiente : BaseRequest, IProcedureRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, Size = 60, ValueConverter = typeof(RequestConvert<string>))]
        public string IdServicio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_FORMULARIO", BindOnNull = true, Type = OracleDbType.Decimal,  ValueConverter = typeof(RequestConvert<long?>))]
        public long? FormularioId { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SEGMENTO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 5)]
        public string Segmento { get; set; }
     
        public void CheckError()
        {
            ;
        }
    }

}