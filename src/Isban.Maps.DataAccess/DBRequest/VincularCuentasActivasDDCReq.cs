using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.DataAccess.DBRequest
{

    [ProcedureRequest("SP_ACT_CTAS_VINCULADAS", Package = Package.MapsGeneralDDC, Owner = "DDC")]
    internal class VincularCuentasActivasDDCReq : IProcedureRequest
    {

        /// <summary>
        /// Atributo: DatoConsulta
        /// Direccion: Input
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The dato consulta.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, BindOnNull = true, Name = "P_NUP", Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_TITULOS", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CuentaTitulos { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUC_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long>))]
        public long Sucursal { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO_CTA", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? TipoCuenta { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, BindOnNull = true, Name = "P_MONEDA", Type = OracleDbType.Varchar2)]
        public string CodMoneda { get; set; }


        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_PRODUCTO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Producto { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUBPRODUCTO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string SubProducto { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, BindOnNull = true, Name = "P_ALIAS", Type = OracleDbType.Varchar2)]
        public string Alias { get; set; }


        [DBParameterDefinition(Direction = ParameterDirection.Input, BindOnNull = true, Name = "P_SEGMENTO", Type = OracleDbType.Varchar2)]
        public string Segmento { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, BindOnNull = true, Name = "P_DESCRIPCION", Type = OracleDbType.Varchar2)]
        public string Descripcion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long>))]
        public long CuentaOperativa { get; set; }

        /// <summary>
        /// Gets or sets the codigo error.
        /// </summary>
        /// <value>
        /// The codigo error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COD_ERROR", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESC_ERROR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
        public string Error { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_IP", BindOnNull = false, Type = OracleDbType.Varchar2)]
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USUARIO", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Usuario { get; set; }
    }

}
