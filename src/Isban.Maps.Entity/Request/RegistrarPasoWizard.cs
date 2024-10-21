using Isban.Maps.Entity.Base;
using System.Runtime.Serialization;


namespace Isban.Maps.Entity.Request
{
    [DataContract]
    public class RegistrarPasoWizard : EntityBase
    {
        [DataMember]
        public long? Id { get; set; }

        [DataMember]
        public long? IdAdhesion { get; set; }

        [DataMember]
        public long? CodigoAltaAdhesion { get; set; }

        [DataMember]
        public long? CodigoBajaAdhesion { get; set; }

        [DataMember]
        public string TextoJson { get; set; }

        [DataMember]
        public long? CuentaTitulo { get; set; }

        [DataMember]
        public long? CuentaOperativa { get; set; }

        [DataMember]
        public string CodigoFondo { get; set; }

        [DataMember]
        public string SucursalCtaOperativa { get; set; }

        [DataMember]
        public long? TipoCtaOperativa { get; set; }

        [DataMember]
        public string CodigoMoneda { get; set; }

        [DataMember]
        public string EstadoFormulario { get; set; }

        [DataMember]
        public long? FormularioIdAnterior { get; set; }

        [DataMember]
        public long? FormularioIdSiguiente { get; set; }       

        [DataMember]
        public string Observacion { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public string SessionId { get; set; }

        [DataMember]
        public long? FormularioId { get; set; }
    }
}
