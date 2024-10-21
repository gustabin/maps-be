using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Bussiness.Factory.Componentes
{


    internal class ConfigDescripcionDinamica : ICrearComponente
    {
        private FormularioResponse _entity;
        private DescripcionDinamica<string> _componente;


        public ConfigDescripcionDinamica(FormularioResponse entity, DescripcionDinamica<string> item)
        {
            _entity = entity;
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
                    }
                }
            }
        }
    }
}

