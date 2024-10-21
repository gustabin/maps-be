using Isban.Mercados.DataAccess.OracleClient;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_OBTENER_ORIGEN_BAJA", Package = Package.GraphToolsWizard, Owner = Owner.Maps)]
    internal class ObtenerFormularioIdOrigenFlujoBaja : ObtenerFormularioIdOrigenFlujo
    {
    }
}
