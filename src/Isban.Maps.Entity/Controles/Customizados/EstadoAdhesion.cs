
namespace Isban.Maps.Entity.Controles.Customizados
{
    using Constantes.Enumeradores;
    using Isban.Mercados.LogTrace;
    using Responsabilidad;
    using System.Runtime.Serialization;

    [DataContract]
    public class EstadoAdhesion<T> : InputText<T>
    {
        //private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        public EstadoAdhesion()
          : base(new AsignarDatosEstadoAdhesion())
        { }
        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            base.Validar();

            if (TieneErrores)
            {
                Error = (int)TiposDeError.ErrorValidacion;
                Error_desc = Errores;
                Error_tecnico += "Error de validación.";
                this.Validado = false;
                LoggingHelper.Instance.Error($"El formulario presenta los siguientes errores de validacion: {Errores}");
            }
            else
            {
                Error = (int)TiposDeError.NoError;
                Error_desc = null;
                Error_tecnico = null;
                this.Validado = true;
                this.Bloqueado = true;
            }

            return !TieneErrores;
        }
        
        /// <summary>
        /// Método que es llamado para asignar los valores del control.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="entiy"></param>
        //public override void AsignarDatosBackend(ValorCtrlResponse[] entiy, string idServicio = null, ControlSimple obj = null)
        //{
        //    if (entiy != null)
        //    {
        //        var vals = entiy.Cast<ValorCtrlResponse>().ToList();

        //        foreach (var atr in vals)
        //        {
        //            var propInfo = this.GetType().GetProperty(atr.AtributoDesc, bindFlags);
        //            if (propInfo != null && atr.AtributoValor != null)
        //            {

        //                propInfo.SetValue(this, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()));
        //            }
        //        }

        //    }
        //}
    }
}
