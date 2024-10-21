using Isban.Maps.Business.Factory;
using Isban.Maps.Business.Formularios.Factory;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Helpers;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.UnityInject;
using System.Linq;
using System.Text;

namespace Isban.Maps.Bussiness.Wizard
{
    public class WizardMapsBaja : IWizardMaps
    {
        private FormularioResponse _formulario;
        private DatoFirmaMaps _firma;

        public WizardMapsBaja(FormularioResponse formulario, DatoFirmaMaps firma)
        {
            _formulario = formulario;
            _firma = firma;
        }
        public FormularioResponse SiguientePaso()
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            EstrategiaComp context = null;

            ObtenerFormularioIdActual();

            var req = _formulario.MapperClass<PasoWizardReq>(TypeMapper.IgnoreCaseSensitive);
            _formulario.FormularioId = daMapsControles.ConsultaPasoSiguienteBaja(req)?.FormularioId;

            context = new EstrategiaComp(new ConfigFormularioBaja(_formulario, _firma));
            context.Crear();   

            ObtenerSessionId();
            _formulario.Config = GenerarConfiguracion();
            _formulario.Items = _formulario.Items?.OrderBy(x => (x as ControlSimple).Posicion).ToList();

            return _formulario;
        }

        private EstrategiaComp ObtenerContexto()
        {
            EstrategiaComp context = null;

            switch (_formulario.IdServicio)
            {
                case Servicio.SAF:


                    break;
                case Servicio.PoderDeCompra:


                    break;
                case Servicio.Agendamiento:


                    break;
                default:

                    break;
            }

            return context;
        }

        private void ObtenerFormularioIdActual()
        {
            #region quitar cuando t-banco pueda recibir FormularioId
            //TODO: cuando t-banco pueda ver FormularioId adapatar esto para que se asigne correctamente.

            var frmID = _formulario.Config?.Split('|')?.Where(x => x.Contains("FormularioId")).FirstOrDefault();

            if (frmID != null && frmID.Split(':').Length > 1 && !string.IsNullOrWhiteSpace(frmID.Split(':')[1]))
            {
                _formulario.FormularioId = frmID.Split(':')[1].ParseGenericVal<long?>();

            }
            #endregion
            else if (_formulario.FormularioId == null && _formulario.IdServicio != null)
            {
                var da = DependencyFactory.Resolve<IMapsControlesDA>();
                _formulario.FormularioId = da.ObtenerFormularioIdOrigenFlujoBaja(_formulario);
            }
        }

        private string GenerarConfiguracion()
        {
            string configuracion = $"FormularioId:{_formulario.FormularioId}|SessionId:{_formulario.SessionId}";

            return configuracion;
        }

        public FormularioResponse PasoAnterior()
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

            //TODO: el request puede ser similar al SiguientePaso()
            var req = new PasoWizardReq();

            daMapsControles.ConsultaPasoAnterior(req);

            return _formulario;
        }

        /// <summary>
        /// Guarda el Json del paso completado y validado
        /// </summary>
        public void RegistrarPasoWizard()
        {
            ObtenerSessionId();
        }

        private void ObtenerSessionId()
        {
            if (_formulario.IdServicio != null && _formulario.Nup != null && _formulario.Segmento != null)
            {
                StringBuilder cadena = new StringBuilder();

                cadena.Append(_formulario.Nup);
                cadena.Append(_formulario.Segmento);
                cadena.Append(_formulario.IdServicio);

                _formulario.SessionId = Crypto.ObtenerMD5(cadena.ToString());
            }
            else
            {
                var session = _formulario.Config?.Split('|')?.Where(x => x.Contains("SessionId")).FirstOrDefault();
                if (session != null && session.Split(':').Length > 1 && !string.IsNullOrWhiteSpace(session.Split(':')[1]))
                {
                    _formulario.SessionId = session.Split(':')[1];
                }
            }
        }

        public void PasoActual()
        {
            //var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            //var req = new ObtenerPasoReq();   //TODO: llenar con valores

            //daMapsControles.ObtenerPaso(req);
        }

        public FormularioResponse FormularioConsulta()
        {
            var daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            EstrategiaComp context = null;

            context = new EstrategiaComp(new ConfigFormularioConsulta(_formulario, _firma));
            context.Crear();

            return _formulario;
        }
    }
}
