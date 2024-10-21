
namespace Isban.Maps.Entity.Controles.Compuestos
{
    using Constantes.Enumeradores;
    using Constantes.Estructuras;
    using Extensiones;
    using Interfaces;
    using Isban.Mercados.LogTrace;
    using Responsabilidad;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    [DataContract]
    public class ImporteCompuesto : ControlSimple
    {
        [DataMember]
        public List<InputNumber<decimal?>> Items { get; set; }

        #region Methods
        public ImporteCompuesto()
            : base(new AsignarDatosImporteCompuesto())
        {
            Items = new List<InputNumber<decimal?>>();
        }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            base.Validar();

            var min = Items.Where(x => x.Nombre.ToLower().Equals(NombreComponente.MontoSuscripcionMinimo)).FirstOrDefault();
            var max = Items.Where(x => x.Nombre.ToLower().Equals(NombreComponente.MontoSuscripcionMaximo)).FirstOrDefault();

            if (min != null && max != null)
            {
                ValidarDecimalesLargo("MontoSuscripcionMinimo", min.Valor.GetDecimalPart(), min.Incremento.GetCountDecimalDigits());
                ValidarDecimalesLargo("MontoSuscripcionMaximo", max.Valor.GetDecimalPart(), max.Incremento.GetCountDecimalDigits());
            }
            else
            {
                TieneErrores = true;
                Errores += $"El valor ingresado no es numérico.";
            }
            if (min != null && max != null)
                if (Convert.ToDecimal(min.Valor) > Convert.ToDecimal(max.Valor))
            {
                TieneErrores = true;
                Errores += $"El monto máximo no puede ser menor al monto mínimo.";
            }

            if (Items == null)
            {
                TieneErrores = true;
                Errores += $"La propiedad Item debe poseer un valor";
            }
            else
            {
                foreach (var item in Items)
                {
                    if (!item.Validar())
                    {
                        TieneErrores = true;
                        Errores += item.Etiqueta + ": " + item.Error_desc;
                    }
                }
            }

            if (TieneErrores)
            {
                Error = (int)TiposDeError.ErrorValidacion;
                Error_desc = Errores;
                Error_tecnico += "Error de validación.";
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

        #endregion
    }
}
