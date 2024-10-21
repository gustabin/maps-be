using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles.Compuestos;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    internal class ConfigImporteCompuesto : ICrearComponente
    {
        private ImporteCompuesto _componente;
        private FormularioResponse _entity;

        public ConfigImporteCompuesto(FormularioResponse _formulario, ImporteCompuesto item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var ctrlAtributosControl = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);

            //TODO: evaluar que se mantenga esto acá
            _componente.IdComponente = (ctrlAtributosControl.First()).IdComponente;
            _componente.IdPadreDB = (ctrlAtributosControl.First()).ControlPadreId;

            foreach (var atr in ctrlAtributosControl)
            {
                var propInfo = _componente.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                if (propInfo != null && atr.AtributoValor != null)
                {
                    propInfo.SetValue(_componente, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                }
            }
        }
    }
}
