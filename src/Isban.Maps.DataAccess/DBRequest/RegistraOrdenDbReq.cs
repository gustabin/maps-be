
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Common.Data;
    using Isban.Maps.Entity.Constantes.Enumeradores;
    using Isban.Maps.IDataAccess;
    using Isban.Mercados;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System;
    using System.Data;

    [ProcedureRequest("SP_REGISTRA_ORDEN", Package = Package.MapsSolicitudOrden, Owner = Owner.Maps)]
    internal class RegistraOrdenDbReq : IProcedureRequest, IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ESTADO_PROCESO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string CodEstadoProceso { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string IdServicio { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_ID_SOLICITUD_ORDENES", Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdSolicitudOrdenes { get; set; }

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
