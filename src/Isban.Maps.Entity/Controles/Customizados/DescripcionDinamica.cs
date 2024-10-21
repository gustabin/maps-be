

namespace Isban.Maps.Entity.Controles.Customizados
{
    using Constantes.Enumeradores;
    using Extensiones;
    using Isban.Mercados.LogTrace;
    using Responsabilidad;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    [DataContract]
    public class DescripcionDinamica<T> : InputText<T>
    {   
        public DescripcionDinamica()
            : base(new AsignarDatosDescripcionDinamica())
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idServicio"></param>
        /// <param name="idFormulario"></param>
        /// <returns></returns>
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
