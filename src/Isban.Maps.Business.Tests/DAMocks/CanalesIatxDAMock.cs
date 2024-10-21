using Isban.Maps.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;

namespace Isban.Maps.Business.Tests
{
    public class CanalesIatxDAMock : ICanalesIatxDA
    {
        public DatosCuentaIATXResponse ConsultaDatosCuenta(Cabecera cabecera, ClienteCuentaDDC cuenta, string nup)
        {
            return new DatosCuentaIATXResponse
            {
                Dispuesto_ARS_cuenta = "78945612310120+",
                Saldo_Cuenta = "78945612310120+"
            };
        }

        public GeneracionCuentaResponse GeneracionCuentaIATX(Cabecera cabecera, FormularioResponse formularioResponse, string sucursal, string producto, string subProducto)
        {
            return new GeneracionCuentaResponse();
        }

        public GeneracionCuentaResponse AltaCuentaIATX(Cabecera cabecera, FormularioResponse formularioResponse, string sucursal, string producto, string subProducto, string numeroCuenta)
        {
            return new GeneracionCuentaResponse();
        }

        public GeneracionCuentaResponse ConsultaCuentaIATX(Cabecera cabecera)
        {
            return new GeneracionCuentaResponse();
        }
    }
}
