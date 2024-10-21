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
    /// <summary>
    /// class Request:  GetConsultaClienteRequest
    /// </summary>
    /// <seealso cref="Isban.ProfitAndLoss.DataAccess.Requests.RequestBase" />
    [ProcedureRequest("SP_CONSULTA_CLIENTE_VM", Package = Package.ConsultaDDC, Owner = "DDC")]
    internal class ObtenerConsultaClienteDDCReq : IProcedureRequest
    {
        /// <summary>
        /// Atributo: DatoConsulta
        /// Direccion: Input
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The dato consulta.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, BindOnNull = true, Name = "P_TEXTO", Type = OracleDbType.Varchar2)]
        public string DatoConsulta { get; set; }

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
