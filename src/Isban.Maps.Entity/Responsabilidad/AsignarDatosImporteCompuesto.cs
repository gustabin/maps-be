using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Response;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Entity.Responsabilidad
{
    public class AsignarDatosImporteCompuesto : IAsignarDatos
    {
        private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        public void AsignarDatosBackend(ValorCtrlResponse[] entity, string idServicio , ControlSimple obj)
        {
            var ctrlAtributosControl = entity.Cast<ValorCtrlResponse>().ToList();

            //TODO: evaluar que se mantenga esto acá
            obj.IdComponente = (ctrlAtributosControl.First()).IdComponente;
            obj.IdPadreDB = (ctrlAtributosControl.First()).ControlPadreId;

            foreach (var atr in ctrlAtributosControl)
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
