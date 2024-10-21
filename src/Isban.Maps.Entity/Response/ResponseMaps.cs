
namespace Isban.Maps.Entity.Request
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class MapsServiceDisclaimer
    {

        [DataMember]
        public string Tipo { get; set; }

        [DataMember]
        public string Formato { get; set; }

        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public string Contenido { get; set; }

        [DataMember]
        public List<MapsDisclaimerCheckBox> ListaCheckboxs { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataContract]
        public class MapsDisclaimerCheckBox
        {
            [DataMember]
            public string Desc { get; set; }

            [DataMember]
            public bool Checked { get; set; }
        }
        
        [DataContract]
        public class MapsServiceOperacionDisclaimer
        {

            [DataMember]
            public string operacion { get; set; }

            [DataMember]
            public List<MapsServiceDisclaimer> MapsServiceDisclaimerList { get; set; }

        }
    }
}
