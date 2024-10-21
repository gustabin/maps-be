namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Common.Data;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_VAL_SIMULA_BAJA", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ServBajaAdhesionSimulacionDbReq : BaseReq
    {        
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ALTA_ADHE", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long>))]
        public long IdAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SIM_ORDEN_ORIGEN", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? SimOrdenOrigen { get; set; }
    }
}

