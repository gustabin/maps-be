
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_UPDATE_AT_ORDER", Package = Package.Ordenes_Mep, Owner = Owner.SMC)]
    public class UpdateOrderDbReq : BaseSmcRequest, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ORDER_ID", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Long)]
        public long OrderId { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_MAPS_ID", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Long)]
        public long MapsId { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_STATUS", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Status { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OBS", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Obs { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USER", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Usuario { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_IP", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Ip { get; set; }
    }
}
