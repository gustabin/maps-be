using Isban.Maps.Entity.Interfaces;
using Isban.Mercados;

namespace Isban.Maps.Configuration.Backend.Interception
{
    public class MapsException : IsbanException
    {
        private IFormulario form;
        private string mensaje;

        public MapsException(string mensaje, IFormulario form)
            : base(mensaje, new System.Exception(form.Error_desc))
        {
            this.mensaje = mensaje;
            this.form = form;
        }

        public IFormulario ParametroRequest
        {
            get { return form; }
        }

        public override TipoExcepcion TipoExcepcion { get; }
    }
}