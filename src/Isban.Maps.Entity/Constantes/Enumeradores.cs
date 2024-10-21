
namespace Isban.Maps.Entity.Constantes.Enumeradores
{
    public enum EstadosFormulario //TODO: si que quieren devolver string cambia a "struct": public const string Actort = "actort";
    {
        Carga = 0,
        Disclaimer = 1,
        Simulacion = 2,
        Confirmacion = 3,
        Consulta = 4
    }

    public enum TiposDeError
    {
        NoError = 0,
        ErrorFatal = 1,
        ErrorBusiness = 2,
        NoSeEncontraronDatos = 3,
        ErrorBaseDeDatos = 4,
        ErrorParcial = 5,
        ErrorSimulacionBaja = 6,
        ErrorValidacion = 7,
        ErrorSimulacionAlta = 8
    }
}