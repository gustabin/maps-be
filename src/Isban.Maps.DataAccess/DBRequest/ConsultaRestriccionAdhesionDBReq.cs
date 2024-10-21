
namespace Isban.Maps.DataAccess.DBRequest
{
    using Isban.Common.Data;
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;
    [ProcedureRequest("SP_OBTENER_OPS_MEP", Package = Package.MapsTools, Owner = Owner.Maps)]
    internal class ConsultaRestriccionAdhesionDBReq : BaseSmcRequest
    {
        /// <summary>
        /// Gets or sets the codigo error.
        /// </summary>
        /// <value>
        /// Nup
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", Type = OracleDbType.Varchar2)]
        public string Nup { get; set; }

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
        /// Mensaje de retorno
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_mensaje", Type = OracleDbType.Varchar2, Size = 4000, ValueConverter = typeof(RequestConvert<string>))]
        public string Mensaje { get; set; }


        /// <summary>
        /// Estado de la operacion
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_ESTADO", Type = OracleDbType.Varchar2, Size = 100, ValueConverter = typeof(RequestConvert<string>))]
        public string Estado { get; set; }


    }
}
