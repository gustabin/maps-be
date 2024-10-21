using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_OBTENER_ORIGEN", Package = Package.GraphToolsWizard, Owner = Owner.Maps)]
    internal class ObtenerFormularioIdOrigenFlujo : BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_FRM_ORIGEN", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? FormularioId { get; set; }
    }
}
