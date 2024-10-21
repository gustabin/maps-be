
namespace Isban.Maps.Entity.Controles
{
    using Constantes.Enumeradores;
    using Constantes.Estructuras;
    using Isban.Maps.Entity.Base;
    using Isban.Mercados.LogTrace;
    using Newtonsoft.Json;
    using Request;
    using Response;
    using System.Runtime.Serialization;

    [DataContract]
    public abstract class ControlSimple : Validaciones, IError
    {
        private int _posicion;
        private IAsignarDatos _asignar;

        protected ControlSimple()
        { }

        protected ControlSimple(IAsignarDatos asignar) //TODO: crear un asignar por cada uno
        {
            _asignar = asignar;
        }

        [IgnoreDataMember]
        [JsonIgnore]
        public long IdComponente { get; set; }

        [DataMember]
        [JsonIgnore]
        public long? IdPadreDB { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Etiqueta { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Tipo { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Ayuda { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Requerido { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Bloqueado { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Posicion
        {
            get
            {
                if (_posicion == 0)
                    return 1;
                else
                    return _posicion;
            }
            set
            {
                if (value == 0)
                    _posicion = 1;
                else
                    _posicion = value;
            }
        }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Validado { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Informacion { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Error { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Error_desc { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Error_tecnico { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Implementa { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Config { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Cabecera Cabecera { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MensajeBusqueda { get; set; }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            if (!(string.IsNullOrWhiteSpace(Id) && string.IsNullOrWhiteSpace(Nombre) && string.IsNullOrWhiteSpace(Tipo)))
            {
                EsVacio("Id", Id);
                EsVacio("Nombre", Nombre);
                EsVacio("Tipo", Tipo);
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

        public virtual void AsignarDatosBackend(ValorCtrlResponse[] entity, string idServicio, ControlSimple obj)
        {  
            _asignar.AsignarDatosBackend(entity, idServicio, obj);

        }
    }
}
