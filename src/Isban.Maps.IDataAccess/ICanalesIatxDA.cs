using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;

namespace Isban.Maps.IDataAccess
{
    public interface ICanalesIatxDA
    {
        DatosCuentaIATXResponse ConsultaDatosCuenta(Cabecera cabecera, ClienteCuentaDDC cuenta, string nup);

        GeneracionCuentaResponse GeneracionCuentaIATX(Cabecera cabecera, FormularioResponse formularioResponse, string sucursal, string producto, string subProducto);

        GeneracionCuentaResponse AltaCuentaIATX(Cabecera cabecera, FormularioResponse formularioResponse, string sucursal, string producto, string subProducto,string numeroCuenta);

        GeneracionCuentaResponse ConsultaCuentaIATX(Cabecera cabecera);
    }
}
