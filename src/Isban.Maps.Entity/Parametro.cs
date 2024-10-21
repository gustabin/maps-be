
namespace Isban.Maps.Entity
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Parametro
    {
        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>
        /// <value>
        /// The fecha.
        /// </value>
        [DataMember]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Gets or sets the estado.
        /// </summary>
        /// <value>
        /// The estado.
        /// </value>
        [DataMember]
        public string Estado { get; set; }

        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>
        /// The descripcion.
        /// </value>
        [DataMember]
        public string Descripcion { get; set; }

        /// <summary>
        /// Gets or sets the valor.
        /// </summary>
        /// <value>
        /// The valor.
        /// </value>
        [DataMember]
        public string Valor { get; set; }
    }
}
