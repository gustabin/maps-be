
namespace Isban.Maps.Entity.Response
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ConsultaParametrizacionResp
    {
        [DataMember]
        public string NombreParametro { get; set; }

        [DataMember]
        public string Valor { get; set; }

        [DataMember]
        public string DescripcionParametro { get; set; }

        [DataMember]
        public string CodigoSistema { get; set; }
    }
}
