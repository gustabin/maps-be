using Isban.Maps.Entity.Base;
using System.Runtime.Serialization;

namespace Isban.Maps.Entity.Request
{
    /// <summary>
    /// class GetConsultaCliente
    /// </summary>
    /// <seealso cref="Isban.PDC.Entity.EntityBase" />
    [DataContract]
    public class GetTitulares : EntityBase
    {
        [DataMember]
        public string CuentaTitulos { get; set; }
		
		[DataMember]
        public string NroCtaOperativa { get; set; }
		
		[DataMember]
        public string SucursalCtaOperativa { get; set; }
		
		[DataMember]
        public string TipoCtaOperativa { get; set; }
		
		[DataMember]
        public string CodMoneda { get; set; }
        
        [DataMember]
        public string Canal { get; set; }
        
        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        public bool TieneErrores { get; set; }

        [DataMember]
        public string Errores { get; set; }

        [DataMember]
        public Cabecera Cabecera { get; set; }
    }
}
