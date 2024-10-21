using Isban.Maps.Entity.Controles.Compuestos;
using System;

namespace Isban.Maps.Entity
{
    public class SeleccionFrecuencia
    {
        public string DiaDeSemana { get; set; }
        public DateTime? Fecha { get; set; }
        public string Frecuencia { get; set; }
        public int NumeroDeDia { get; set; }
    }


    public class SeleccionVigencia
    {
        public FechaCompuesta FechaCompuesta { get; set; }
        public long idVigencia { get; set; }
        public long idFechaHasta { get; set; }
        public long idFechaDesde { get; set; }
        public long idPeriodos { get; set; }
    }
}
