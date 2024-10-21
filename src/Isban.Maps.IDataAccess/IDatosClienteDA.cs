using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.IDataAccess
{
    public interface IDatosClienteDA
    {
        List<ClienteDDC> ObtenerClientesDDC(GetCuentas search);

        VincularCuentasActivasReq VincularCuentasActivas(VincularCuentasActivasReq request);
    }
}
