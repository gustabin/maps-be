
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Mercados.DataAccess.OracleClient;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteListaDias, Owner = Owner.Maps)]
    internal class ValorCompListaDias : ComponenteBaseDbReq
    {
    }
}
