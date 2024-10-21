
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Mercados.DataAccess.OracleClient;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteListaFrecuencia, Owner = Owner.Maps)]
    internal class ValorCompListaFrecuencia : ComponenteBaseDbReq
    {
    }
}
