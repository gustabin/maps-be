
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using DataAccess;
    using Isban.Maps.DataAccess.Base;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_CONFIRMA_ADHESION", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ServConsAdhesionConfimacionReq : BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Long, ValueConverter = typeof(RequestConvert<long>))]
        public long IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CTA_TITULO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CuentaTitulos { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ORDEN_ORIGEN", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? OrdenOrigen { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TEXTO_JSON", BindOnNull = true, Type = OracleDbType.Clob, ValueConverter = typeof(RequestConvert<string>))]
        public string TextoJson { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_ID_ALTA_ADHE", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal>))]
        public decimal Comprobante { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DISCLAIMER", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string TextoDisclaimer { get; set; }
    }
}
