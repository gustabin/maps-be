
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using DataAccess;
    using Isban.Maps.DataAccess.Base;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_CREA_SIMULACION_ADHESION", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ServConsAdhesionSimulacionReq : BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_TIT", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? CuentaTitulo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUC_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? SucCtaOper { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NRO_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? NroCtaOper { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO_CTA_OPER", BindOnNull = true, Type = OracleDbType.Int32, ValueConverter = typeof(RequestConvert<int?>))]
        public int? TipoCuentaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUC_CTA_OPER_ACT", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? SucCtaOperDolares { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NRO_CTA_OPER_ACT", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? NroCtaOperDolares { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO_CTA_OPER_ACT", BindOnNull = true, Type = OracleDbType.Int32, ValueConverter = typeof(RequestConvert<int?>))]
        public int? TipoCuentaOperativaDolares { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TEXTO_JSON", BindOnNull = true, Type = OracleDbType.Clob, ValueConverter = typeof(RequestConvert<string>))]
        public string TextoJson { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_CUENTA_ANTES", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? SaldoCuentaAntes { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_MONEDA", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CodigoMoneda { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_MONEDA_ACT", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CodigoMonedaDolares { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_DE_DISCLAIMER", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string DescDisclaimer { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FE_VIGENCIA_DESDE", BindOnNull = true, Type = OracleDbType.Date, ValueConverter = typeof(RequestConvert<DateTime?>))]
        public DateTime? FecVigenciaDesde { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FE_VIGENCIA_HASTA", BindOnNull = true, Type = OracleDbType.Date, ValueConverter = typeof(RequestConvert<DateTime?>))]
        public DateTime? FecVigenciaHasta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_MIN_OPERACION", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? SaldoMinOperacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_MAX_OPERACION", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? SaldoMaxOperacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_FONDO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CodigoFondo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ESPECIE", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CodEspecie { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_MIN_DEJAR_CTA", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? SaldoMinDejarCta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_PRODUCTO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CodProducto { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SIM_ORDEN_ORIGEN", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? SimOrdenOrigen { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FECHA_EJECUCION", BindOnNull = true, Type = OracleDbType.Date, ValueConverter = typeof(RequestConvert<DateTime?>))]
        public DateTime? FechaDeEjecucion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SALDO_ENVIADO", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? SaldoEnviado { get; internal set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OPERACION", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string Operacion { get; set; }
    }
}
