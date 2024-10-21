
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using IDataAccess;
    using Isban.Mercados.DataAccess.OracleClient;
    using Mercados.DataAccess.ConverterDBType;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_CONSULTA_PARAMETROS", Package = Package.MapsParametros, Owner = Owner.Maps)]
    internal class ConsultaParametrizacionDbReq : IRequestBase, IProcedureRequest
    {
        /// <summary>
        /// 
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NOM_PARAMETRO", BindOnNull = true, Type = OracleDbType.Varchar2, Size = 60, ValueConverter = typeof(RequestConvert<string>))]
        public string NomParametro { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_SISTEMA", BindOnNull = true, Type = OracleDbType.Varchar2, Size = 4, ValueConverter = typeof(RequestConvert<string>))]
        public string CodigoSistema { get; set; }

        /// <summary>
        /// Gets or sets the codigo error.
        /// </summary>
        /// <value>
        /// The codigo error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COD_RES", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESCR_ERR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
        public string Error { get; set; }
        /// <summary>
        /// Gets or sets the ip.
        /// </summary>
        /// <value>
        /// The ip.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_IP", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 20)]
        public string Ip { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USUARIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 30)]
        public string Usuario { get; set; }

        public void CheckError()
        {
            ;
        }
    }
}
