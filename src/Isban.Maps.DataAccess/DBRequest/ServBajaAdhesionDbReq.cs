namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Common.Data;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_BAJA_ADHESION", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ServBajaAdhesionDbReq : BaseReq
    {        
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Long, ValueConverter = typeof(RequestConvert<long>))]
        public long IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_ID_BAJA_ADHE", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdAdhesionBaja { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TEXTO_JSON", BindOnNull = true, Type = OracleDbType.Clob, ValueConverter = typeof(RequestConvert<string>))]
        public string TextoJson { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA_EFECTIVA", BindOnNull = true, Type = OracleDbType.Date, ValueConverter = typeof(RequestConvert<DateTime?>))]
        public DateTime? FechaEfectiva { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ORDEN_ORIGEN", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? OrdenOrigen { get; set; }
    }
}

