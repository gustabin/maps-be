using Isban.Maps.Business.Componente.Factory;
using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness.Factory.Componentes;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Compuestos;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Controles.Independientes;
using Isban.Maps.Entity.Response;

namespace Isban.Maps.Bussiness.Factory
{
    public class FabricaEstrategia : IFabricaEstrategia
    {
        public ControlSimple Fabricar(string estrategia, FormularioResponse entity, ControlSimple item, DatoFirmaMaps firma)
        {
            ControlSimple result = null;

            switch (estrategia)
            {
                case NombreComponente.CuentaOperativa: //"cuenta-operativa"
                    var co = new EstrategiaComp(new ConfigCuentaOperativa(entity, (Lista<ItemCuentaOperativa<string>>)item, firma));
                    co.Crear();
                    result = item;
                    break;
                case NombreComponente.CuentaTitulo: //"cuenta-titulo"
                    var ct = new EstrategiaComp(new ConfigCuentaTitulo(entity, (Lista<ItemCuentaTitulos<string>>)item, firma));
                    ct.Crear();
                    result = item;
                    break;
                case NombreComponente.Moneda: //"moneda"
                    var mo = new EstrategiaComp(new ConfigMoneda(entity, (Lista<ItemMoneda<string>>)item));
                    mo.Crear();
                    result = item;
                    break;
                case NombreComponente.ListadoAsesoramiento:
                case NombreComponente.ListaPep:
                case NombreComponente.ListadoFondos:
                    var lfondo = new EstrategiaComp(new ConfigListadoFondos(entity, (Lista<Item<string>>)item));
                    lfondo.Crear();
                    result = item;
                    break;
                case NombreComponente.ListaFrecuencia:
                case NombreComponente.ListaDias:
                case NombreComponente.ListadoGenerico:
                case NombreComponente.Operacion: //"operacion"
                    var oper = new EstrategiaComp(new ConfigOperacion(entity, (Lista<Item<string>>)item));
                    oper.Crear();
                    result = item;
                    break;
                case NombreComponente.Periodos: //"periodos"
                    var per = new EstrategiaComp(new ConfigPeriodos(entity, (Lista<Item<decimal>>)item));
                    per.Crear();
                    result = item;
                    break;
                case NombreComponente.Servicio:
                    var ser = new EstrategiaComp(new ConfigServicio(entity, (Lista<ItemServicio<string>>)item));
                    ser.Crear();
                    result = item;
                    break;
                case NombreComponente.Email:
                    var email = new EstrategiaComp(new ConfigEmail(entity, (InputText<string>)item));
                    email.Crear();
                    result = item;
                    break;
                case NombreComponente.Alias:
                    var alias = new EstrategiaComp(new ConfigAlias(entity, (InputText<string>)item));
                    alias.Crear();
                    result = item;
                    break;
                case NombreComponente.Numeros:
                case NombreComponente.SaldoMinimo:
                case NombreComponente.MontoSuscripcionMinimo:
                case NombreComponente.MontoSuscripcionMaximo:
                    var salMin = new EstrategiaComp(new ConfigSaldoMinimo(entity, (InputNumber<decimal?>)item));
                    salMin.Crear();
                    result = item;
                    break;
                case NombreComponente.FechaFrecuencia:
                case NombreComponente.FechaEjecucion:
                case NombreComponente.FechaDesde:
                case NombreComponente.FechaHasta:
                case NombreComponente.Fecha:
                    var fecha = new EstrategiaComp(new ConfigFecha(entity, (Fecha)item));
                    fecha.Crear();
                    result = item;
                    break;
                case NombreComponente.Legal:
                case NombreComponente.LegalAgendamiento:
                    var legal = new EstrategiaComp(new ConfigLegal(entity, (Lista<ItemLegal<string>>)item));
                    legal.Crear();
                    result = item;
                    break;
                case NombreComponente.Vigencia:
                case NombreComponente.FechaCompuesta:
                    var fecComp = new EstrategiaComp(new ConfigFechaCompuesta(entity, (FechaCompuesta)item));
                    fecComp.Crear();
                    result = item;
                    break;
                case NombreComponente.ImporteCompuesto:
                case NombreComponente.MontoSuscripcion:
                    var impComp = new EstrategiaComp(new ConfigImporteCompuesto(entity, (ImporteCompuesto)item));
                    impComp.Crear();
                    result = item;
                    break;
                case NombreComponente.FondoCompuesto:
                    var fonComp = new EstrategiaComp(new ConfigFondoCompuesto(entity, (FondoCompuesto)item));
                    fonComp.Crear();
                    result = item;
                    break;

                case NombreComponente.ListaFondos://usado para fondo compuesto
                    var listaFondos = new EstrategiaComp(new ConfigListaFondosAGD(entity, (Lista<ItemGrupoAgd>)item));
                    listaFondos.Crear();
                    result = item;
                    break;

                case NombreComponente.Frecuencia:
                    var frecuencia = new EstrategiaComp(new ConfigFrecuencia(entity, (Frecuencia)item));
                    frecuencia.Crear();
                    result = item;
                    break;
                case NombreComponente.DescripcionDinamica:
                    var descripcion = new EstrategiaComp(new ConfigDescripcionDinamica(entity, (DescripcionDinamica<string>)item));
                    descripcion.Crear();
                    result = item;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;

        }
    }
}
