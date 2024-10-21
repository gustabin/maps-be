using Isban.Maps.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.IBussiness
{
    public interface IWizardMaps
    {
        FormularioResponse SiguientePaso();
        FormularioResponse PasoAnterior();
        void RegistrarPasoWizard();
        void PasoActual();
    }
}
