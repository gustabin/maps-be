
namespace Isban.Maps.Entity.Controles.Customizados
{
    using Constantes.Enumeradores;
    using Isban.Mercados.LogTrace;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ConsultaAdhesiones : ControlSimple
    {
        public ConsultaAdhesiones()
        {
            Activas = new List<FormularioResponse>();
            Inactivas = new List<FormularioResponse>();
        }

        [DataMember]
        public  List<FormularioResponse> Activas { get; set; }

        [DataMember]
        public List<FormularioResponse> Inactivas { get; set; }
     
        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            base.Validar();
            
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

       
    }
}
