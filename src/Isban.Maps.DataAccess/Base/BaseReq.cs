
namespace Isban.Maps.DataAccess.Base
{
    using Common.Data;
    using Entity.Constantes.Enumeradores;
    using IDataAccess;
    using Mercados;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    internal class BaseReq : BaseRequest, IProcedureRequest, IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NUP", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 10)]
        public string Nup { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SEGMENTO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 5)]
        public string Segmento { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 10)]
        public string IdServicio { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CANAL", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 2)]
        public string Canal { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUBCANAL", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4)]
        public string SubCanal { get; set; }

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
