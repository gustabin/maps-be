using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles.Compuestos;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    internal class ConfigFechaCompuesta : ICrearComponente
    {
        private FechaCompuesta _componente;
        private FormularioResponse _entity;

        public ConfigFechaCompuesta(FormularioResponse _formulario, FechaCompuesta item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var valores = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);

            if (valores != null)
            {
                for (int i = 0; i < valores.Length; i++)
                {
                    var propInfo = _componente.GetType().GetProperty(valores[i].AtributoDesc, bindFlags);
                    if (propInfo != null && valores[i].AtributoValor != null)
                    {
                        propInfo.SetValue(_componente, valores[i].AtributoValor.ParseGenericVal(valores[i].AtributoDataType.ToType()));
                    }
                }
            }
        }
    }
}