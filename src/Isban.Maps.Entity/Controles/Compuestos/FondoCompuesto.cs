using Isban.Maps.Entity.Constantes.Enumeradores;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Controles.Independientes;
using Isban.Maps.Entity.Constantes.Estructuras;
using System.Linq;
using Isban.Mercados.LogTrace;

namespace Isban.Maps.Entity.Controles.Compuestos
{
    [DataContract]
    public class FondoCompuesto : ControlSimple
    {
        public FondoCompuesto()
        {
            Items = new List<ControlSimple>();
        }

        [DataMember]
        public List<ControlSimple> Items { get; set; }


        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            base.Validar(idServicio, idFormulario);
            ValidarLegal();

            if (TieneErrores)
            {
                Error = (int)TiposDeError.ErrorValidacion;
                Error_desc = Errores;
                Error_tecnico += "Error de validación..\r\n";
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

        private void ValidarLegal()
        {
            var legal = Items.GetControlMaps<Lista<ItemLegal<string>>>(NombreComponente.LegalAgendamiento);            

            if (legal != null && legal.Items.Count > 0)
            {
                legal.Items.ForEach(x =>
                {
                    if (x.Items.Any(y => y.Checked == false))
                    {
                        TieneErrores = true;
                        Errores = "Se deben acepatar todos los legales.\n\r";
                    }
                    else
                    {
                        TieneErrores = false;
                        legal.Bloqueado = true;
                        legal.Validado = true;
                    }

                });
            }
            
        }
    }
}
