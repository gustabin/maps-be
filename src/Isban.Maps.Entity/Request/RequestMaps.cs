
namespace Isban.Maps.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class RequestMaps : EntityBase
    {

        private string _operacion;
        private string _segmento;

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        public int FormCompleted { get; set; }

        [DataMember]
        public string Operacion
        {
            get { return _operacion; }
            set { _operacion = !string.IsNullOrEmpty(value) ? value.ToUpper() : string.Empty; }
        }

        [DataMember]
        public string Segmento
        {
            get { return _segmento; }
            set { _segmento = !string.IsNullOrEmpty(value) ? value.ToUpper() : string.Empty; }
        }

        [DataMember]
        public List<dynamic> FormCampos { get; set; }
        /*
        public bool Validar()
        {
            var rtrn = true;

            foreach (var item in FormCampos)
            {

                if (!item.IsValid()) { rtrn = false; }
            }

            return rtrn;
        }*/
    }
}
