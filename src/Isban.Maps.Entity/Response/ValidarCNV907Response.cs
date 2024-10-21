namespace Isban.Maps.Entity.Response
{
    using System;
    using System.Runtime.Serialization;
    [DataContract]
    public class ValidarCNV907Response
    {
        [DataMember]
        public string MensajeTecnico { get; set; }

        [DataMember]
        public int NivelRestriccion { get; set; }

        [DataMember]
        public string Notificaciones { get; set; }

        [DataMember]
        public int CantidadNominales
        {
            get
            {
                return String.IsNullOrEmpty(Notificaciones) ? 0 : Convert.ToInt32(Notificaciones.Replace(" Nominales.", ""));
            }
        }
    }
}
