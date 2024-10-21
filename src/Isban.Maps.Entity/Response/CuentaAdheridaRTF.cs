using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    public class ObtenerRTFDisponiblesResponse
    {
        public List<CuentaAdheridaRTF> ListaRTF { get; set; }

        public List<Cuenta> ListaCuentas { get; set; }

        public string UltimoPeriodoGenerado { get; set; }
        public int UltimoAnioGenerado { get; set; }
    }


    public class CuentaAdheridaRTF
    {
        public long Id { get; set; }
        public string Cuenta { get; set; }

        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string Anio { get; set; }

        public bool Visto { get; set; }

    }

    public class ArchivoRTF
    {
        public long Id { get; set; }
        public string Cuenta { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public bool Visto { get; set; }
        public string Archivo { get; set; }
        public string Path { get; set; }
    }

    public class Cuenta
    {
        public string NumeroCuenta { get; set; }

        public string Segmento { get; set; }

    }

    public class UltimoPeriodoRTF
    {
        public int Anio { get; set; }
        public string Periodo { get; set; }

    }
}
