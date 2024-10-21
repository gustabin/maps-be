
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_CREATE_AT_ORDER", Package = Package.Ordenes_Mep, Owner = Owner.SMC)]
    public class InsertOrderDbReq : BaseSmcRequest, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OPERATION_TYPE", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string OperationType { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUSTODY_ACCOUNT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal? CustodyAccount { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_AMOUNT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal? Amount { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CURRENCY", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Currency { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SECURITY_ID", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string SecurityId { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DEBIT_BRANCH", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal? DebitBranch { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DEBIT_PRODUCT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string DebitProduct { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DEBIT_SUBPRODUCT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string DebitSubProduct { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DEBIT_ACCOUNT_TYPE", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal? DebitAccountType { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DEBIT_OPERATIVE_ACCOUNT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal? DebitOperativeAccount { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CREDIT_BRANCH", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal? CreditBranch { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CREDIT_PRODUCT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string CreditProduct { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CREDIT_SUBPRODUCT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string CreditSubProduct { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CREDIT_ACCOUNT_TYPE", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal CreditAccountType { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CREDIT_OPERATIVE_ACCOUNT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Decimal)]
        public decimal? CreditOperativeAccount { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USER", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Usuario { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_IP", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Ip { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SEGMENT", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Segment { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CHANNEL", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string Channel { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUBCHANNEL", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Varchar2)]
        public string SubChannel { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_MAPS_ID", BindOnNull = true, DefaultBindValue = null, Type = OracleDbType.Long)]
        public long MapsId { get; set; }
    }
}
