using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Entity.Response
{
    [DataContract]
    public class ValidaRestriccionMEPResponse
    {
        /// <summary>
        /// Listado de cuentas titulo
        /// </summary>
        [DataMember]
        public List<string> ListaCuentas { get; set; }

        /// <summary>
        /// Listado de cuentas titulo
        /// </summary>
        [DataMember]
        public List<string> ListaCuentasNoHabilitadas { get; set; }

        /// <summary>
        /// NUP del Cliente
        /// </summary>
        [DataMember]
        public string Nup { get; set; }

        /// <summary>
        /// Codigo de respuesta restriccion centralizado
        /// </summary>
        [DataMember]
        public long CodigoRestriccion { get; set; }

        /// <summary>
        /// Mensaje de restriccion centralizado
        /// </summary>
        [DataMember]
        public string NotificacionRestriccion { get; set; }

        /// <summary>
        /// Codigo de respuesta validacion 7106
        /// </summary>
        [DataMember]
        public long Codigo7106 { get; set; }
        /// <summary>
        /// Mensaje de respuesta validacion 7106
        /// </summary>
        [DataMember]
        public string Notificacion7106 { get; set; }
        /// <summary>
        /// Codigo de respuesta validacion de cuentas firmadas
        /// </summary>
        [DataMember]
        public long CodigoCuentas { get; set; }
        /// <summary>
        /// Mensaje de respuesta validacion de cuentas firmadas
        /// </summary>
        [DataMember]
        public string NotificacionCuentas { get; set; }
        /// <summary>
        /// Codigo de respuesta validacion test inversor
        /// </summary>
        [DataMember]
        public long CodigoTest { get; set; }
        /// <summary>
        /// Mensaje de respuesta validacion test inversor
        /// </summary>
        [DataMember]
        public string NotificacionTest { get; set; }

        /// <summary>
        /// Codigo de respuesta validacion fuera de horario
        /// </summary>
        [DataMember]
        public long CodigoHorario { get; set; }

        /// <summary>
        /// Mensaje de respuesta validacion fuera de horario
        /// </summary>
        [DataMember]
        public string NotificacionHorario { get; set; }

        public long CodigoOperacionSimultanea { get; set; }

        public string NotificacionOperacionSimultanea { get; set; } 

    }
}
