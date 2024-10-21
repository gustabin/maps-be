using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using System;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Entity.Responsabilidad
{
    public class AsignarDatosSimple : IAsignarDatos
    {
        private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

        public void AsignarDatosBackend(ValorCtrlResponse[] entity, string idServicio , ControlSimple obj )
        {
            if (entity != null)
            {
                try
                {
                    var ctrlAtributosControl = entity.ToList();

                    if (ctrlAtributosControl != null)
                    {
                        foreach (var atr in ctrlAtributosControl)
                        {
                            if (!string.IsNullOrWhiteSpace(atr.AtributoDesc))
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
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
