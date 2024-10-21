using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using System;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Entity.Responsabilidad
{
    public class AsignarDatosFecha : IAsignarDatos
    {
        private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        /// <summary>
        /// Método que es llamado para asignar los valores del control.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="entiy"></param>
        public void AsignarDatosBackend(ValorCtrlResponse[] entiy, string idServicio , ControlSimple obj )
        {
            if (entiy != null)
            {
                var vals = entiy.Cast<ValorCtrlResponse>().ToList();

                foreach (var atr in vals)
                {
                    var propInfo = obj.GetType().GetProperty(atr.AtributoDesc, bindFlags);
                    if (propInfo != null && atr.AtributoValor != null)
                    {
                        try
                        {
                            if (atr.AtributoValor.ToLower().Equals("today"))
                            {
                                propInfo.SetValue(obj, DateTime.Now);
                            }
                            else if (atr.AtributoValor.ToLower().Equals("tomorrow"))
                            {
                                propInfo.SetValue(obj, DateTime.Now.AddDays(1D));
                            }
                            else
                                propInfo.SetValue(obj, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);

                        }
                        catch (Exception ex)
                        {
                            throw new InvalidCastException($"{obj.Etiqueta}: El valor {atr.AtributoValor} no se puede convertir a {atr.AtributoDataType}", ex);
                        }
                    }
                }
            }
        }
    }
}
