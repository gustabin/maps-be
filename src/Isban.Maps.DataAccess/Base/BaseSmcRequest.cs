
namespace Isban.Maps.DataAccess.Base
{
    using Common.Data;
    using Mercados;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    public class BaseSmcRequest : IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COD_RES", Type = OracleDbType.Long, Size = 10, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESCR_ERR", Type = OracleDbType.Varchar2, Size = 100, ValueConverter = typeof(RequestConvert<string>))]
        public string DescripcionError { get; set; }

        public virtual void CheckError()
        {
            if (CodigoError.HasValue && this.CodigoError.GetValueOrDefault() != 0)
            {
                throw new DBCodeException(this.CodigoError.GetValueOrDefault(), string.Format("Descripción: {0}, Retorno: {1}", this.DescripcionError, this.CodigoError));
            }
        }
    }
}
