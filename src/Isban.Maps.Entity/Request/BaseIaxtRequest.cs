
namespace Isban.Maps.Entity.Request
{
    public abstract class BaseIaxtRequest
    {
        #region atributos
        public string H_Canal_Tipo { get; set; }

        public string H_SubCanal_Id { get; set; }

        public string H_Canal_Ver { get; set; }

        public string H_SubCanal_Tipo { get; set; }

        public string H_Canal_Id { get; set; }

        public string H_Usuario_Tipo { get; set; }

        public string H_Usuario_Id { get; set; }

        public string H_Usuario_Attr { get; set; }

        public string H_Usuario_Pwd { get; set; }

        public string H_Idus_Conc { get; set; }

        public string H_NumSec { get; set; }

        public string H_Nup { get; set; }

        public string H_Tipo_Cliente { get; set; }

        public string H_Tipo_Id_Cliente { get; set; }

        public string H_Nro_Id_Cliente { get; set; }

        public string H_Fecha_Nac { get; set; }

        public string H_Ind_Sincro { get; set; }
        #endregion
    }
}
