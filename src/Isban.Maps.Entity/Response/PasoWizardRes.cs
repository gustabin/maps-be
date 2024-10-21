using System.Runtime.Serialization;

namespace Isban.Maps.Entity.Response
{
    [DataContract]
    public class PasoWizardRes
    {
        [DataMember]
        public long? FormularioId { get; set; }

        [DataMember]
        public long? Paso { get; set; }
    }
}
