using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_REGISTRA_PASO", Package = Package.GraphToolsWizard, Owner = Owner.Maps)]
    internal class RegistrarPasoWizardDbReq : BaseReq, IProcedureRequest
    {
        /// <summary>
        /// Id de la tabla recien guardado.
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.InputOutput, Name = "P_ID_MAPS_WIZARD", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? Id { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ALTA_ADHESION", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoAltaAdhesion {get;set;}

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_BAJA_ADHESION", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoBajaAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TEXTO_JSON", BindOnNull = true, Type = OracleDbType.Clob, ValueConverter = typeof(RequestConvert<string>))]
        public string TextoJson { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_TITULOS", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CuentaTitulo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NRO_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CuentaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_FONDO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CodigoFondo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUC_CTA_OPER", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string SucursalCtaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? TipoCtaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_MONEDA", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CodigoMoneda { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ESTADO_FORM", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string EstadoFormulario { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_MAPS_WIZARD_ANTERIOR", BindOnNull = true, Type = OracleDbType.Long, ValueConverter = typeof(RequestConvert<long?>))]
        public long? FormularioIdAnterior { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_MAPS_WIZARD_SIGUIENTE", BindOnNull = true, Type = OracleDbType.Long, ValueConverter = typeof(RequestConvert<long?>))]
        public long? FormularioIdSiguiente { get; set; }        

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_OBSERVACION", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string Observacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_SESSION", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string SessionId { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_FORMULARIO_ID", BindOnNull = true, Type = OracleDbType.Long, ValueConverter = typeof(RequestConvert<long?>))]
        public long? FormularioId { get; set; }

    }
}
