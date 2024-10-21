using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    public class ConfigAlias : ICrearComponente
    {
        private InputText<string> _componente;
        private FormularioResponse _entity;

        public ConfigAlias(FormularioResponse _formulario, InputText<string> item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var ctrlAtributosControl = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);

            if (ctrlAtributosControl != null)
            {
                foreach (var atr in ctrlAtributosControl)
                {
                    if (!string.IsNullOrWhiteSpace(atr.AtributoDesc))
                    {
                        var propInfo = _componente.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                        if (propInfo != null && atr.AtributoValor != null)
                        {
                            propInfo.SetValue(_componente, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                        }

                        if (string.Compare(atr.AtributoDesc, "MinLength") == 0
                            && (atr.AtributoValor == null || string.Compare(atr.AtributoValor, "0") == 0))
                        {

                            propInfo.SetValue(_componente, 3M);
                        }

                        if (string.Compare(atr.AtributoDesc, "MaxLength") == 0
                           && (atr.AtributoValor == null || string.Compare(atr.AtributoValor, "0") == 0))
                        {

                            propInfo.SetValue(_componente, 13M);
                        }
                    }
                }
            }
        }
    }
}