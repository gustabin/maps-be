using System;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Response;
using System.Reflection;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.IBussiness;
using Isban.Maps.Business.Factory;

namespace Isban.Maps.Business.Componente.Factory
{
    internal class ConfigEmail : ICrearComponente
    {
        private InputText<string> _componente;
        private FormularioResponse _entity;

        public ConfigEmail(FormularioResponse _formulario, InputText<string> item)
        {
            this._entity = _formulario;
            this._componente = item;
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

            if (string.Compare(_entity.Segmento, Segmento.Retail, true) == 0)
            {
                IServicesClient srvClient = DependencyFactory.Resolve<IServicesClient>();
                _componente.Valor = srvClient.GetMailXNup(_entity.Canal, _entity.SubCanal, _entity.Nup);
                _componente.Bloqueado = true;
            }
            else if (string.Compare(_entity.Segmento, Segmento.BancaPrivada, true) == 0)
            {
                _componente.Bloqueado = false;
            }    
        }
    }
}