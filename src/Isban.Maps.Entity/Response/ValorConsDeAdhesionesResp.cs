
namespace Isban.Maps.Entity.Response
{
    using Interfaces;
    using System.Runtime.Serialization;

    [DataContract]
    public class ValorConsDeAdhesionesResp : IResponseEntity
    {
        [DataMember]
        public string TextoJson { get; set; }

        [DataMember]
        public char Estado { get; set; }

        [DataMember]
        public string CodigoFondo { get; set; }

        [DataMember]
        public decimal CuentaTitulo { get; set; }

        [DataMember]
        public decimal CuentaDebito { get; set; }
    }
}
