using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Entity.Responsabilidad
{
    public class AsignarDatosEstadoAdhesion : IAsignarDatos
    {
        private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        /// Método que es llamado para asignar los valores del control.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="entiy"></param>
        public void AsignarDatosBackend(ValorCtrlResponse[] entiy, string idServicio , ControlSimple obj)
        {
            if (entiy != null)
            {
                var vals = entiy.Cast<ValorCtrlResponse>().ToList();

                foreach (var atr in vals)
                {
                    var propInfo = obj.GetType().GetProperty(atr.AtributoDesc, bindFlags);
                    if (propInfo != null && atr.AtributoValor != null)
                    {

                        propInfo.SetValue(obj, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
                    }
                }

            }
        }
    }
}
