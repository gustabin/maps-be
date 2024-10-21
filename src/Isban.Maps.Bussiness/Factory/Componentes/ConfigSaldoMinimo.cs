using Isban.Maps.Entity.Extensiones;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System;
using System.Reflection;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Response;
using System.Linq;
using Isban.Maps.Business.Factory;

namespace Isban.Maps.Business.Componente.Factory
{
    internal class ConfigSaldoMinimo : ICrearComponente
    {
        private InputNumber<decimal?> _componente;
        private FormularioResponse _entity;

        public ConfigSaldoMinimo(FormularioResponse _formulario, InputNumber<decimal?> item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var ctrlAtributosControl = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);

            //TODO: evaluar quitarlo
            _componente.IdComponente = (ctrlAtributosControl.First()).IdComponente;
            _componente.IdPadreDB = (ctrlAtributosControl.First()).ControlPadreId;

            foreach (var atr in ctrlAtributosControl)
            {
                var propInfo = _componente.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                if (propInfo != null && atr.AtributoValor != null)
                {
                    var val = atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType());
                    propInfo.SetValue(_componente, val);
                }
            }
        }
    }
}

