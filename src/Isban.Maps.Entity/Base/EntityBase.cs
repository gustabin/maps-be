using Isban.Mercados;
using Isban.Mercados.Security.Adsec.Entity;

namespace Isban.Maps.Entity.Base
{
    using System.Runtime.Serialization;

    [DataContract]
    public class EntityBase : ITraceEntity
    {
        [DataMember]
        public System.Guid? IdTrace { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }
        
    }

    [DataContract]
    public class DatoFirmaMaps 
    {
        #region atributos
        /// <summary>
        /// Tipo de Hash a utilizar en la firma B64, PEM
        /// </summary>
        [DataMember]
        public TipoHash TipoHash { get; set; }
        /// <summary>
        /// Dato Firmados
        /// </summary>
        [DataMember]
        public string Firma { get; set; }
        /// <summary>
        /// Dato
        /// </summary>
        [DataMember]
        public string Dato { get; set; }
        /// <summary>
        /// Informa si los datos viene en la firma
        /// </summary>
        [DataMember]
        public DatosFirmado DatosFirmado { get; set; }
        #endregion
    }
}
