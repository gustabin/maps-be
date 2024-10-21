using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Entity.Responsabilidad
{
    public class AsignarDatosInputNumber : IAsignarDatos
    {
        private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        public void AsignarDatosBackend(ValorCtrlResponse[] entity, string idServicio , ControlSimple obj )
        {
            if (entity != null)
            {
                var ctrlAtributosControl = entity.Cast<ValorCtrlResponse>().ToList();

                //TODO: evaluar quitarlo
                obj.IdComponente = (ctrlAtributosControl.First()).IdComponente;
                obj.IdPadreDB = (ctrlAtributosControl.First()).ControlPadreId;

                foreach (var atr in ctrlAtributosControl)
                {
                    var propInfo = obj.GetType().GetProperty(atr.AtributoDesc, bindFlags);

                    if (propInfo != null && atr.AtributoValor != null)
                    {
                        var val = atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType());
                        propInfo.SetValue(obj, val);
                    }
                }
            }
        }
    }
}
