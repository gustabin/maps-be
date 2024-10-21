
namespace Isban.Maps.Entity.Request
{
    using Isban.Maps.Entity.Base;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Cabecera : Validaciones
    {
        [DataMember]
        public string H_CanalTipo { get; set; }

        [DataMember]
        public string H_SubCanalId { get; set; }

        [DataMember]
        public string H_CanalVer { get; set; }

        [DataMember]
        public string H_SubCanalTipo { get; set; }

        [DataMember]
        public string H_CanalId { get; set; }

        [DataMember]
        public string H_UsuarioTipo { get; set; }

        [DataMember]
        public string H_UsuarioID { get; set; }

        [DataMember]
        public string H_UsuarioAttr { get; set; }

        [DataMember]
        public string H_UsuarioPwd { get; set; }

        [DataMember]
        public string H_IdusConc { get; set; }

        [DataMember]
        public string H_NumSec { get; set; }

        [DataMember]
        public string H_Nup { get; set; }

        [DataMember]
        public string H_IndSincro { get; set; }

        [DataMember]
        public string H_TipoCliente { get; set; }

        [DataMember]
        public string H_TipoIDCliente { get; set; }

        [DataMember]
        public string H_NroIDCliente { get; set; }

        [DataMember]
        public string H_FechaNac { get; set; }

        [DataMember]
        public string Usuario { get; set; }

        [DataMember]
        public string Ip { get; set; }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            EsVacio("H_CanalTipo", H_CanalTipo);
            EsVacio("H_SubCanalId", H_SubCanalId);
            EsVacio("H_CanalVer", H_CanalVer);
            EsVacio("H_SubCanalTipo", H_SubCanalTipo);
            EsVacio("H_CanalId", H_CanalId);
            EsVacio("H_UsuarioTipo", H_UsuarioTipo);
            EsVacio("H_UsuarioID", H_UsuarioID);
            EsVacio("H_IdusConc", H_IdusConc);
            EsVacio("H_NumSec", H_NumSec);
            EsVacio("H_Nup", H_Nup);
            EsVacio("H_IndSincro", H_IndSincro);
            EsVacio("H_TipoCliente", H_TipoCliente);
            EsVacio("H_TipoIDCliente", H_TipoIDCliente);
            EsVacio("H_NroIDCliente", H_NroIDCliente);
            EsVacio("H_FechaNac", H_FechaNac);


            if (TieneErrores)
            {
                throw new ArgumentNullException(Errores);
            }

            return true;
        }
    }
}
