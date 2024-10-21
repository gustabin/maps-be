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
    [ProcedureRequest("sp_consulta_arch", Package = Package.MapsRtf, Owner = Owner.Maps)]
    internal class ConsultaArchivoRTFDbReq : IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_nup", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_lista_id", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string Id { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "p_cuenta_tit", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string CuentaTitulo { get; set; }

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
