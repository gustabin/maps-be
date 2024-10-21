using Isban.Common.Data;
using Isban.Mercados;
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
    [ProcedureRequest("sp_cons_ultimo_per", Package = Package.MapsRtf, Owner = Owner.Maps)]
    internal class ConsultaUltimoPeriodoRTFReq : IProcedureRequest
    {

        /// <summary>
        /// Gets or sets the codigo error.
        /// </summary>
        /// <value>
        /// The codigo error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "p_anio", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<int>))]
        public int Anio { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "p_periodo", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 1000)]
        public string Periodo { get; set; }
        /// <summary>
        /// Gets or sets the codigo error.
        /// </summary>
        /// <value>
        /// The codigo error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "p_cod_error", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "p_desc_error", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 1000)]
        public string Error { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 20, Name = "p_ip", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Size = 20, Name = "p_usuario", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Usuario { get; set; }

        /// <summary>
        /// Checks the error.
        /// </summary>
        /// <exception cref="DBCodeException"></exception>
        public void CheckError()
        {
            if (this.CodigoError.GetValueOrDefault() != 0)
            {
                throw new DBCodeException(this.CodigoError.GetValueOrDefault(), this.Error);
            }
        }
    }
}
