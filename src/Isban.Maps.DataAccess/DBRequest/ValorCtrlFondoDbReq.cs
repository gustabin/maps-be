
namespace Isban.Maps.DataAccess.DBRequest
{
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteFondo, Owner = Owner.Maps)] 
    internal class ValorCtrlFondoDbReq: ComponenteBaseDbReq, IProcedureRequest
    {
       
    }
}
