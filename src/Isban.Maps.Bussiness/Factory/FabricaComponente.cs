using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Compuestos;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Controles.Independientes;

namespace Isban.Maps.Bussiness.Factory
{
    public class FabricaComponente : IFabricarComponente
    {
        public ControlSimple Fabricar(string nombreDelComponente)
        {
            ControlSimple result = null;

            switch (nombreDelComponente.ToLower())
            {
                case NombreComponente.CuentaOperativa:
                    result = new Lista<ItemCuentaOperativa<string>>();
                    break;
                case NombreComponente.CuentaTitulo:
                    result = new Lista<ItemCuentaTitulos<string>>();
                    break;
                case NombreComponente.Moneda:
                    result = new Lista<ItemMoneda<string>>();

                    break;
                case NombreComponente.ListaFrecuencia:
                case NombreComponente.ListaDias:
                case NombreComponente.ListadoFondos:
                case NombreComponente.ListadoAsesoramiento:
                case NombreComponente.ListaPep:
                    result = new Lista<Item<string>>();
                    break;
                case NombreComponente.ListadoGenerico:
                case NombreComponente.Operacion: //"operacion"
                    result = new Lista<Item<string>>();

                    break;
                case NombreComponente.Periodos: //"periodos"
                    result = new Lista<Item<decimal>>();

                    break;
                case NombreComponente.Servicio:
                    result = new Lista<ItemServicio<string>>();

                    break;
                case NombreComponente.Email:
                    result = new InputText<string>();

                    break;
                case NombreComponente.Alias:
                    result = new InputText<string>();

                    break;
                case NombreComponente.Numeros:
                case NombreComponente.SaldoMinimo:
                case NombreComponente.MontoSuscripcionMinimo:
                case NombreComponente.MontoSuscripcionMaximo:
                    result = new InputNumber<decimal?>();

                    break;
                case NombreComponente.FechaFrecuencia:
                case NombreComponente.FechaEjecucion:
                case NombreComponente.FechaDesde:
                case NombreComponente.FechaHasta:
                case NombreComponente.Fecha:
                    result = new Fecha();

                    break;
                case NombreComponente.Legal:
                case NombreComponente.LegalAgendamiento:
                    result = new Lista<ItemLegal<string>>();

                    break;
                case NombreComponente.Vigencia:
                case NombreComponente.FechaCompuesta:
                    result = new FechaCompuesta();

                    break;
                case NombreComponente.ImporteCompuesto:
                case NombreComponente.MontoSuscripcion:
                    result = new ImporteCompuesto();

                    break;
                case NombreComponente.FondoCompuesto:
                    result = new FondoCompuesto();

                    break;

                case NombreComponente.ListaFondos://usado para fondo compuesto
                    result = new Lista<ItemGrupoAgd>();

                    break;
                case NombreComponente.Frecuencia:
                    result = new Frecuencia();

                    break;
                case NombreComponente.DescripcionDinamica:
                    result = new DescripcionDinamica<string>();

                    break;

                default:
                    result = null;
                    break;
            }

            return result;
        }
    }
}
