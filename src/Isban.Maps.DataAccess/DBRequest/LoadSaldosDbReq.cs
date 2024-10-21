
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using Base;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("LOAD_SALDOS", Package = Package.BpServiciosA3, Owner = Owner.BCAINV)]
    public class LoadSaldosDbReq : BcainvRequestBase, IProcedureRequest
    {

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2, Size = 20)]
        public string Cuenta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA_DESDE", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Date)]
        public DateTime FechaDesde { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA_HASTA", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Date)]
        public DateTime FechaHasta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANAL", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2, Size = 3)]
        public string Canal { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USUARIO", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2, Size = 20)]
        public string Usuario { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_PASSWORD", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2, Size = 20)]
        public string Password { get; set; }
    }
}
