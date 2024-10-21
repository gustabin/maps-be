using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using System;
using System.Reflection;

namespace Isban.Maps.Entity.Responsabilidad
{
    public class AsignarDatosFechaCompuesta : IAsignarDatos
    {
        private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        public void AsignarDatosBackend(ValorCtrlResponse[] entities, string idServicio , ControlSimple obj)
        {
            try
            {
                if (entities != null)
                {
                    for (int i = 0; i < entities.Length; i++)
                    {
                        var propInfo = obj.GetType().GetProperty(entities[i].AtributoDesc, bindFlags);
                        if (propInfo != null && entities[i].AtributoValor != null)
                        {
                            propInfo.SetValue(obj, entities[i].AtributoValor.ParseGenericVal(entities[i].AtributoDataType.ToType()));
                        }
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException(string.Format("Error en: {0}. Tipo lista incorrecto, se espera {1} y llegó {2}", typeof(ValorCtrlResponse), this, this.GetType().Name), ex);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
