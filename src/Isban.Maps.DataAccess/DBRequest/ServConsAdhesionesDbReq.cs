
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Common.Data;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_CONSULTA_ADHESION", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ServConsAdhesionesDbReq : BaseReq
    {        
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ALTA_ADHE", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_BAJA_ADHE", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public string Comprobante { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTAS_OPERATIVAS", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CuentasOperativas { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTAS_TITULO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CuentasTitulos { get; set; }
    }
}


