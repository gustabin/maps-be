
namespace Isban.Maps.Entity.Controles.Customizados
{
    using Constantes.Enumeradores;
    using Isban.Mercados.LogTrace;
    using Responsabilidad;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class Fecha : InputText<DateTime?>
    {
        //private BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        public Fecha()
            : base(new AsignarDatosFecha())
        { }
          
        [DataMember]
        public DateTime FechaMin { get; set; }

        [DataMember]

        public DateTime FechaMax { get; set; }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            if (Requerido)
                EsVacio("Valor", Valor);

            EsVacio("FechaMin", FechaMin);
            EsVacio("FechaMax", FechaMax);


            if (Valor != null && EsFecha(Valor.ToString()))
            {
                DateTime inFecha = Convert.ToDateTime(Valor);

                if (!(inFecha.Date >= FechaMin.Date && inFecha.Date <= FechaMax.Date))
                {
                    TieneErrores = true;
                    Errores += $"El valor de ser entre {FechaMin} y {FechaMax}. \r\n";
                }
            }
            else
            {
                TieneErrores = true;
                Errores += $"El valor {Valor} es una fecha incorrecta.\r\n";


            }

            if (TieneErrores)
            {
                Error = (int)TiposDeError.ErrorValidacion;
                Error_desc = Errores;
                Error_tecnico += "Error de validación. \r\n";
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
        //                try
        //                {
        //                    //List<string> ListaFechas = new List<string>(new string[] { NombreComponente.FechaHasta, NombreComponente.FechaDesde, NombreComponente.FechaHastaSafBP, NombreComponente.FechaDesdeSafBP, NombreComponente.FechaVigenciaPDC, NombreComponente.FechaAltaPdcAdhesion, NombreComponente.Fecha, NombreComponente.FechaSafBP, NombreComponente.FechaBaja });
        //                    if (atr.AtributoValor.ToLower().Equals("today"))
        //                    {
        //                        propInfo.SetValue(this, DateTime.Now);
        //                    }
        //                    else if (atr.AtributoValor.ToLower().Equals("tomorrow"))
        //                    {
        //                        propInfo.SetValue(this, DateTime.Now.AddDays(1D));
        //                    }
        //                    else
        //                        propInfo.SetValue(this, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);

        //                }
        //                catch (Exception ex)
        //                {
        //                    throw new InvalidCastException($"{Etiqueta}: El valor {atr.AtributoValor} no se puede convertir a {atr.AtributoDataType}", ex);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
