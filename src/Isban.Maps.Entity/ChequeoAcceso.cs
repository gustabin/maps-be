

namespace Isban.Maps.Entity
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChequeoAcceso
    {
        [DataMember]
        public string BasedeDatos { get; set; }

        [DataMember]
        public string UsuarioDB { get; set; }

        [DataMember]
        public string UsuarioWin { get; set; }

        [DataMember]
        public string ServidorDB { get; set; }

        [DataMember]
        public string ServidorWin { get; set; }

        [DataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public bool Ok { get; set; }
    }
}

