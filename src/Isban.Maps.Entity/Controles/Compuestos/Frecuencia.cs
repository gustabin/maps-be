using Isban.Maps.Entity.Constantes.Enumeradores;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using System.Linq;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Mercados.LogTrace;

namespace Isban.Maps.Entity.Controles.Compuestos
{
    [DataContract]
    public class Frecuencia : ControlSimple
    {
        bool frecuenciaValidada = false;

        public Frecuencia()
        {
            Items = new List<ControlSimple>();
        }

        [DataMember]
        public List<ControlSimple> Items { get; set; }

        public SeleccionFrecuencia ObtenerFrecuenciaSeleccionada()
        {
            var frecuenciaSeleccionada = (Items.Where(x => string.Compare(x.Nombre, NombreComponente.ListaFrecuencia, true) == 0).FirstOrDefault() as Lista<Item<string>>)
                                            .Items.Where(y => y.Seleccionado).ToList();
            var cantSeleccionada = frecuenciaSeleccionada?.Count();
            var seleccionFrecuencia = new SeleccionFrecuencia();

            if (frecuenciaSeleccionada != null && cantSeleccionada != null && cantSeleccionada == 1)
            {
                switch (frecuenciaSeleccionada.FirstOrDefault().Valor)
                {
                    case TipoFrecuencia.Semanal:
                        seleccionFrecuencia.Frecuencia = TipoFrecuencia.Semanal.ToString();
                        seleccionFrecuencia.DiaDeSemana = (Items.Where(x => string.Compare(x.Nombre, NombreComponente.ListaDias, true) == 0).FirstOrDefault() as Lista<Item<string>>)
                                            .Items.Where(y => y.Seleccionado).FirstOrDefault().Valor;

                        break;
                    case TipoFrecuencia.FechaUnica:
                        seleccionFrecuencia.Frecuencia = TipoFrecuencia.FechaUnica.ToString();
                        seleccionFrecuencia.Fecha = (Items.Where(x => string.Compare(x.Nombre, NombreComponente.FechaFrecuencia, true) == 0).FirstOrDefault() as Fecha).Valor;

                        break;
                    case TipoFrecuencia.MismoDiaCadaMes:
                        seleccionFrecuencia.Frecuencia = TipoFrecuencia.MismoDiaCadaMes.ToString();
                        seleccionFrecuencia.NumeroDeDia = (Items.Where(x => string.Compare(x.Nombre, NombreComponente.Numeros, true) == 0).FirstOrDefault() as InputNumber<int>).Valor;

                        break;

                    default:
                        break;
                }
            }

            return seleccionFrecuencia;
        }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            base.Validar(idServicio, idFormulario);
            ValidarListados(NombreComponente.ListaFrecuencia);
            ValidarListados(NombreComponente.ListaDias);

            ValidarFrecuenciaFechaUnica();
            ValidarFrecuenciaDiaDelMes();


            if (TieneErrores)
            {
                Error = (int)TiposDeError.ErrorValidacion;
                Error_desc = Errores;
                Error_tecnico += "Error de validación.\r\n";
                this.Validado = false;
                LoggingHelper.Instance.Error($"El formulario presenta los siguientes errores de validacion: {Errores}");
            }
            else
            {
                Error = (int)TiposDeError.NoError;
                Error_desc = null;
                Error_tecnico = null;
                this.Validado = true;
                this.Bloqueado = true;
            }

            return !TieneErrores;
        }

        private void ValidarListados(string nombreLista)
        {
            TieneErrores = (Items.Where(x => string.Compare(x.Nombre, nombreLista, true) == 0)
                                .FirstOrDefault() as Lista<Item<string>>)
                                    .Items.Where(x => x.Seleccionado).ToArray().Length > 1;

            Errores = $"Existe más de una frecuencia seleccionada";
        }


        private void ValidarFrecuenciaDiaDelMes()
        {
            if (frecuenciaValidada)
            {
                var numeroDeDia = Items.Where(x => string.Compare(x.Nombre, NombreComponente.Numeros, true) == 0).FirstOrDefault() as InputNumber<int>;

                if (numeroDeDia != null && numeroDeDia.Valor != 0)
                {
                    frecuenciaValidada = true;
                }
                else
                {
                    TieneErrores = true;
                }
            }
        }

        private void ValidarFrecuenciaFechaUnica()
        {
            if (frecuenciaValidada)
            {
                var fechaFrecuencia = Items.Where(x => string.Compare(x.Nombre, NombreComponente.FechaFrecuencia, true) == 0).FirstOrDefault() as Fecha;

                if (fechaFrecuencia != null && fechaFrecuencia.Valor.HasValue)
                {
                    frecuenciaValidada = true;
                }
                else
                {
                    TieneErrores = true;
                }

            }
        }
    }
}
