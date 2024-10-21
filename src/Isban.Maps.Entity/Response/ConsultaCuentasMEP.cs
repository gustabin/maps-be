using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    public class ConsultaCuentasMEP
    {
        public long CuentaTitulos { get; set; }

        public long CuentaOperativa { get; set; }

        public int Sucursal { get; set; }

        public int TipoCuentaOperativa { get; set; }
    }
}
