
namespace Isban.Maps.DataAccess.Base
{
    using Common.Data;
    using Mercados;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    public class BcainvRequestBase : IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_RETORNO", Type = OracleDbType.Varchar2, Size = 3, ValueConverter = typeof(RequestConvert<string>))]
        public string Retorno { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COD_RETORNO", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodRetorno { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESCRIPCION", Type = OracleDbType.Varchar2, Size = 300, ValueConverter = typeof(RequestConvert<string>))]
        public string Descripcion { get; set; }

        public virtual void CheckError()
        {
            if (this.CodRetorno.GetValueOrDefault() != 0)
            {
                throw new DBCodeException(this.CodRetorno.GetValueOrDefault(), string.Format("Descripción: {0}, Retorno: {1}", this.Descripcion, this.Retorno));
            }
        }
    }
}
