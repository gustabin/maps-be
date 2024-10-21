
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using Isban.Maps.Entity.Constantes.Enumeradores;
    using Isban.Maps.IDataAccess;
    using Isban.Mercados;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_CONSULTA_ORIGEN", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ConsultaOrigenDbReq : IProcedureRequest, IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_ADHESIONES", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdSimulacion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_COD_ALTA_ADHESION", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>))]
        public long? IdAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 10)]
        public string IdServicio { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COD_RES", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESCR_ERR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
        public string Error { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_IP", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 20)]
        public string Ip { get; set; }

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
