using System.Xml.Serialization;

namespace Isban.Maps.Business.Test
{
    public class ConsDeAdhesionesMockResp
    {
        [XmlElement(ElementName = "TEXTO_JSON")]
        public string TextoJson { get; set; }

        [XmlElement(ElementName = "ESTADO")]
        public string Estado { get; set; }
    }
}
