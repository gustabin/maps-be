using Isban.Maps.Business.Factory;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System;
using System.Reflection;

namespace Isban.Maps.Business.Componente.Factory
{
    internal class ConfigFecha : ICrearComponente
    {
        private Fecha _componente;
        private FormularioResponse _entity;

        public ConfigFecha(FormularioResponse _formulario, Fecha item)
        {
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var vals = daMapsControles.ObtenerDatosPorComponente(_componente, _entity);

            foreach (var atr in vals)
            {
                var propInfo = _componente.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                if (propInfo != null && atr.AtributoValor != null)
                {
                    try
                    {
                        //List<string> ListaFechas = new List<string>(new string[] { NombreComponente.FechaHasta, NombreComponente.FechaDesde, NombreComponente.FechaHastaSafBP, NombreComponente.FechaDesdeSafBP, NombreComponente.FechaVigenciaPDC, NombreComponente.FechaAltaPdcAdhesion, NombreComponente.Fecha, NombreComponente.FechaSafBP, NombreComponente.FechaBaja });
                        if (atr.AtributoValor.ToLower().Equals("today"))
                        {
                            propInfo.SetValue(_componente, DateTime.Now);
                        }
                        else if (atr.AtributoValor.ToLower().Equals("tomorrow"))
                        {
                            propInfo.SetValue(_componente, DateTime.Now.AddDays(1D));
                        }
                        else
                            propInfo.SetValue(_componente, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);

                    }
                    catch (Exception ex)
                    {
                        throw new InvalidCastException($"{_componente.Nombre}: El valor {atr.AtributoValor} no se puede convertir a {atr.AtributoDataType}", ex);
                    }
                }
            }
        }
    }
}