
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;


namespace Isban.Maps.DataAccess.DBRequest
{

    using Base;
    using Isban.Common.Data;
    using Isban.Mercados.DataAccess.ConverterDBType;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_ALTA_ATIT_REPA",Package = "pkg_repatriacion", Owner = "OPICS")]
    public class AltaCuentaOpicsDBReq : BaseEntornoRequest
    {

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "xnumatit", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CuentaTitulo { get; set; }
    }
}



