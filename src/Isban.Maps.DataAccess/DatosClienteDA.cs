
namespace Isban.Maps.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Isban.Maps.DataAccess.DBRequest;
    using Isban.Maps.DataAccess.DBResponse;
    using Isban.Maps.Entity.Request;
    using Isban.Maps.Entity.Response;
    using Isban.Maps.IDataAccess;
    using Mercados;
    using Mercados.DataAccess;
    using Mercados.DataAccess.OracleClient;


    /// <summary>
    /// class DatosClienteDA
    /// </summary>
    /// <seealso cref="Isban.Mercados.DataAccess.BaseProxy" />
    /// <seealso cref="Isban.ProfitAndLoss.IDataAccess.IDatosClienteDA" />
    [ProxyProvider("DBDDC")]
    public class DatosClienteDA : BaseProxy, IDatosClienteDA
    {


        public virtual List<ClienteDDC> ObtenerClientesDDC(GetCuentas search)
        {
            var request = search.MapperClass<ObtenerConsultaClienteDDCReq>(TypeMapper.IgnoreCaseSensitive);
            //request.DatoConsulta = search.Palabra ?? "";
            var listResult = Provider.GetCollection<ConsultaClienteDDCResp>(CommandType.StoredProcedure, request);
            //request.CheckError();
            return listResult.MapperEnumerable<ClienteDDC>().ToList();
        }


        public virtual VincularCuentasActivasReq VincularCuentasActivas(VincularCuentasActivasReq search)
        {
            var request = search.MapperClass<VincularCuentasActivasDDCReq>(TypeMapper.IgnoreCaseSensitive);
           
            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            //request.CheckError();
            return search;
        }


    }


}
