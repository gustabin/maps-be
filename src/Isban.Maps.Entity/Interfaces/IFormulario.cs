namespace Isban.Maps.Entity.Interfaces
{
    public interface IFormulario
    {
        string IdServicio { get; set; }

        long? IdSimulacion { get; set; }

        string Comprobante { get; set; }

        string Estado { get; set; }

        long? FormAnterior { get; set; }
        long? FormSiguiente { get; set; }

        long? IdAdhesion { get; set; }

        string Titulo { get; set; }

        string Nup { get; set; }

        string Segmento { get; set; }

        string Canal { get; set; }

        string SubCanal { get; set; }

        string PerfilInversor { get; set; }

        long? FormularioId { get; set; }

        string Error_desc { get; set; }

        string Usuario { get; set; }

        string Ip { get; set; }
    }
}
