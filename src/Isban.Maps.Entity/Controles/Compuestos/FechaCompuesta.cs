namespace Isban.Maps.Entity.Controles.Compuestos
{
    using Constantes.Enumeradores;
    using Constantes.Estructuras;
    using Customizados;
    using Isban.Mercados.LogTrace;
    using Responsabilidad;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class FechaCompuesta : ControlSimple
    {
        public FechaCompuesta()
            : base(new AsignarDatosFechaCompuesta())
        {
            Items = new List<ControlSimple>();
        }

        [DataMember]
        public List<ControlSimple> Items { get; set; }

        public bool VigenciaElegida(List<Item<decimal>> rangos)
        {
            bool RangoSeleccionado = false;
            foreach (var item in rangos)
            {
                if (item.Seleccionado == true)
                {
                    RangoSeleccionado = true;
                }
            }
            return RangoSeleccionado;
        }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            base.Validar();

            if (Items == null)
            {
                TieneErrores = true;
                Errores += $"La lista debe poseer un valor.\r\n";
            }

            var ctrlLista = Items.Where(x => x.Tipo.ToLower().Equals(TipoControl.Lista)).ToList();// as Lista<Item<string>>;
            var fechas = Items.Where(x => x.Tipo.ToLower().Equals(TipoControl.Fecha)).ToList();

            if (ctrlLista != null && ctrlLista.Count > 1)
            {
                TieneErrores = true;
                Errores += $"El componente posee mas de un item Periodo.\r\n";
            }
            if (fechas.Count > 2)
            {
                TieneErrores = true;
                Errores += $"El componente posee mas de un item Fecha.\r\n";
            }

            if (ctrlLista != null && fechas != null)
            {
                var periodos = ctrlLista.FirstOrDefault() as Lista<Item<string>>;
                var fechaDesde = fechas.Where(x => x.Nombre.ToLower().Contains("desde")).FirstOrDefault() as Fecha;
                var fechaHasta = fechas.Where(x => x.Nombre.ToLower().Contains("hasta")).FirstOrDefault() as Fecha;

                if (periodos.Validar())
                {
                    //TODO: para obtener los valores que cada clase tenga su obtener valor o GetValor.
                    var esOtroIntervalo = periodos.Items.Where(x => x.Seleccionado).Any(y => y.Desc.ToLower().Equals(Descripcion.OtroIntervalo));

                    if (esOtroIntervalo)
                    {
                        if (fechaDesde.Valor > fechaHasta.Valor)
                        {
                            TieneErrores = true;
                            Errores += $"La fecha desde no puede ser mayor a la fecha hasta.\r\n";
                        }
                        if (fechaDesde.Valor < DateTime.Now.Date)
                        {
                            TieneErrores = true;
                            Errores += $"La fecha desde no puede ser menor a la fecha de solicitud.\r\n";
                        }
                    }
                    #region OtroIntervalo
                    else if (!esOtroIntervalo)
                    {
                        double intervalo = 0;
                        double.TryParse(periodos.Items.Where(x => x.Seleccionado).FirstOrDefault()?.Valor.ToString(), out intervalo);

                        fechaDesde.Valor = DateTime.Now.AddDays(1).Date;

                        fechaHasta.Valor = DateTime.Now.AddDays(intervalo + 1).Date;

                        if (intervalo != 0)
                        {
                            var dias = fechaHasta.Valor.Value.Subtract(fechaDesde.Valor.Value).TotalDays;

                            if (!dias.Equals(intervalo))
                            {
                                TieneErrores = true;
                                Errores += $"El rango de las fechas debe ser de {intervalo} días.\r\n";
                            }

                            if (fechaDesde.Valor > fechaHasta.Valor)
                            {
                                TieneErrores = true;
                                Errores += $"La fecha desde no puede ser mayor a la fecha hasta.\r\n";
                            }
                        }
                        else
                        {
                            TieneErrores = true;
                            Errores += $"Debe seleccionar un item de la lista e ingresar un intervalo de fechas.\r\n";
                            Errores += fechaDesde.Errores;
                            Errores += fechaHasta.Errores;
                        }
                    }
                    #endregion

                    #region Validar fechas min y max
                    if (fechaDesde.Valor.HasValue && fechaHasta.Valor.HasValue)
                    {
                        if (!(fechaDesde.Valor.Value.Date >= fechaDesde.FechaMin.Date && fechaDesde.Valor.Value.Date <= fechaDesde.FechaMax.Date))
                        {
                            TieneErrores = true;
                            Errores += $"La fecha debe estar entre {fechaDesde.FechaMin.Date} y {fechaDesde.FechaMax.Date}.\r\n";
                        }

                        if (!(fechaHasta.Valor.Value.Date >= fechaHasta.FechaMin.Date && fechaHasta.Valor.Value.Date <= fechaHasta.FechaMax.Date))
                        {
                            TieneErrores = true;
                            Errores += $"La fecha debe estar entre {fechaHasta.FechaMin.Date} y {fechaHasta.FechaMax.Date}.\r\n";
                        }

                        if ((fechaDesde.Valor.Value.Date <= DateTime.Now.Date) && !(idServicio.ToUpper() == Servicio.PoderDeCompra) && !(idServicio.ToUpper() == Servicio.Agendamiento))
                        {
                            TieneErrores = true;
                            Errores += $"La fecha desde debe ser mayor a la fecha actual.\r\n";
                        }

                        if (!ValidateDateIsBetween(DateTime.Now.Date, fechaDesde.Valor.Value.Date) && (fechaDesde.Valor.Value.Date <= DateTime.Now.Date) && (idServicio.ToUpper() == Servicio.Agendamiento))
                        {                         
                            TieneErrores = true;
                            Errores += $"La fecha desde debe ser mayor a la fecha actual.\r\n";
                        }
                    }
                    else if (fechaDesde.Valor.HasValue && !string.IsNullOrWhiteSpace(idServicio) && (idServicio.ToUpper() == Servicio.Rtf || idServicio.ToUpper() == Servicio.PoderDeCompra))
                    {
                        if (!(fechaDesde.Valor.Value.Date >= fechaDesde.FechaMin.Date && fechaDesde.Valor.Value.Date <= fechaDesde.FechaMax.Date))
                        {
                            TieneErrores = true;
                            Errores += $"La fecha debe estar entre {fechaDesde.FechaMin.Date} y {fechaDesde.FechaMax.Date}.\r\n";
                        }

                        if ((fechaDesde.Valor.Value.Date < DateTime.Now.Date))
                        {
                            TieneErrores = true;
                            Errores += $"La fecha desde debe ser mayor a la fecha actual.\r\n";
                        }
                    }
                    else
                    {
                        TieneErrores = true;
                        Errores += $"Se debe ingresar una Fecha Desde y una Fecha Hasta";
                    }
                    #endregion

                }
                else
                {
                    TieneErrores = true;
                    Errores += periodos.Errores;
                }
            }

            if (TieneErrores)
            {
                Error = (int)TiposDeError.ErrorValidacion;
                Error_desc = Errores;
                LoggingHelper.Instance.Error($"El formulario presenta los siguientes errores de validacion: {Errores}");
                Error_tecnico += "Error de validación..\r\n";
                this.Validado = false;
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

        public bool ValidateDateIsBetween(DateTime dateActual, DateTime dateCargado)
        {
            TimeSpan ts = new TimeSpan(0, 0, 0);
            var dateInicio = dateActual.Date + ts;

            TimeSpan ts2 = new TimeSpan(7, 28, 0);
            var dateFin = dateActual.Date + ts2;

            if (dateCargado >= dateInicio && dateCargado <= dateFin)
                return true;

            return false;

        }

    }
}
