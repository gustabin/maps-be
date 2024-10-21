
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using IDataAccess;
    using Isban.Mercados.DataAccess.OracleClient;
    using Mercados.DataAccess.ConverterDBType;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;
    using System;
    using Entity.Constantes.Enumeradores;
    using Mercados;

    [ProcedureRequest("OBTENER_SIGUIENTE_FORMULARIO", Package = Package.MapsGraphTools, Owner = Owner.Maps)]
    internal class ObtenerSiguienteFormularioDbReq : IProcedureRequest, IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_LISTA_COMPONENTES", BindOnNull = true, Type = OracleDbType.Varchar2, Size = 4000, ValueConverter = typeof(RequestConvert<string>))]
        public string ListaComponentes { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ORIGEN", BindOnNull = true, Type = OracleDbType.Varchar2, Size = 200, ValueConverter = typeof(RequestConvert<string>))]
        public string Origen { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SEGMENTO", BindOnNull = true, Type = OracleDbType.Varchar2, Size = 4, ValueConverter = typeof(RequestConvert<string>))]
        public string Segmento { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_FRM_ID", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? FormularioId { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", Type = OracleDbType.Varchar2, Size = 4000, ValueConverter = typeof(RequestConvert<string>))]
        public string IdServicio { get; set; }

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

        public virtual void CheckError()
        {
            if (this.CodigoError.GetValueOrDefault() != 0)
            {
                switch (this.CodigoError.GetValueOrDefault())
                {
                    case (long)TiposDeError.ErrorBaseDeDatos:
                        throw new DBCodeException(this.CodigoError.GetValueOrDefault(), $"Excepción de BD: { this.Error}");

                    case (long)TiposDeError.NoSeEncontraronDatos:
                        throw new DBCodeException(this.CodigoError.GetValueOrDefault(), $"No se encontró información en la BD { this.Error}");

                    default:
                        throw new DBCodeException(this.CodigoError.GetValueOrDefault(), $"Error no controlado: { this.Error}");
                }
            }
        }
    }
}
