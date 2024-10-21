
namespace Isban.Maps.Entity.Request
{
    using Constantes.Estructuras;
    using Controles;
    using Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class FormularioRequest : ControlSimple, IFormulario
    {
        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? IdSimulacion { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Comprobante { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? FormAnterior { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? FormSiguiente { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? IdAdhesion { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Titulo { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public string PerfilInversor { get; set; }

        [DataMember]
        public virtual List<JObject> Items { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? FormularioId { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Usuario { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Ip { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CodFondo { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Moneda { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Operacion { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CuentaTitulos { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CuentaOperativa { get; set; }
        public FormularioRequest ShallowCopy()
        {
            var form = (FormularioRequest)this.MemberwiseClone() as FormularioRequest;
            if (Items != null)
                form.Items = new List<JObject>(Items);

            return form;
        }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            #region formulario

            EsVacio("Nup", Nup);
            ValidarLargoRango("Nup", Nup, 8);

            if (!EsNumerico(Nup))
            {
                TieneErrores = true;
                Error_tecnico = $"El valor ingresado para NUP es incorrecto. Valor actual {Nup}";
            }

            EsVacio("Segmento", Segmento);
            ValidarLargo("Segmento", Segmento, 3);

            EsVacio("Estado", Estado);
            ValidarContenido("Estado", Estado, TipoEstadoFormulario.Baja, TipoEstadoFormulario.Carga, TipoEstadoFormulario.Confirmacion, TipoEstadoFormulario.Consulta, TipoEstadoFormulario.Disclaimer, TipoEstadoFormulario.Simulacion, TipoEstado.SimulacionBaja);
            #endregion

            if (TieneErrores)
            {
                Error_desc = Errores;
            }

            return !TieneErrores;
        }

    }
}
