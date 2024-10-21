
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_SALDO_CONC_NO_LIQ", Package = Package.Ordenes, Owner = Owner.SMC)]
    public class SaldoConcertadoNoLiquidadoDbReq : BaseSmcRequest, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NRO_CTA_OPER", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string NroCtaOper { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUC_CTA_OPER", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string SucCtaOper { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO_CTA_OPER", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal TipoCtaOper { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Date)]
        public DateTime Fecha { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_MONEDA", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Moneda { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USUARIO", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Usuario { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_IP", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Ip { get; set; }



    }
}
