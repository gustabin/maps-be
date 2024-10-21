
namespace Isban.Maps.Entity.Controles
{
    using Constantes.Enumeradores;
    using Isban.Mercados.LogTrace;
    using Newtonsoft.Json;
    using Responsabilidad;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class InputNumber<T> : ControlSimple
    {
        public InputNumber()
            : base(new AsignarDatosInputNumber())
        { }

        [DataMember]
        public decimal? MaxValor { get; set; }

        [DataMember]
        public decimal? MinValor { get; set; }

        [DataMember]
        public T Valor { get; set; }

        [DataMember]        
        public decimal Incremento { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Simbolo { get; set; }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            if (Requerido)
            {
                EsVacio("Valor", Valor);
            }           

            if (Incremento < 0) {
                TieneErrores = true;
                Errores = $"El del incremento no puede ser negativo {Incremento}. \r\n";
            }


            if (Valor != null)
            {
                if (!EsDecimal(Valor.ToString()))
                {
                    TieneErrores = true;
                    Errores = $"El valor ingresado no es numérico. Valor actual: {Valor}";
                }
                else
                {
                    if ((Convert.ToDecimal(Valor) >= Convert.ToDecimal(Math.Pow(10, 12))))
                    {
                        TieneErrores = true;
                        Errores = $"El valor ingresado no puede tener mas de 12 digitos enteros. Valor actual: {Valor}";
                    }

                    string msgErrRango = $"El valor ingresado no se encuentra entre {MinValor} y {MaxValor}. \r\n";

                    if (MinValor == MaxValor)
                    {
                        if (Convert.ToDecimal(Valor) < MinValor)
                        {
                            TieneErrores = true;
                            Errores = $"El valor ingresado es menor a { MinValor }. \r\n";
                        }
                        else
                        {
                            if (Convert.ToDecimal(Valor) > MaxValor)
                            {
                                TieneErrores = true;
                                Errores = $"El valor ingresado es mayor a { MaxValor }. \r\n";
                            }
                        }
                    }
                    else
                    {

                        if (Convert.ToDecimal(Valor) < 0)
                        {
                            TieneErrores = true;
                            Errores = $"El valor ingresado no puede ser negativo { Valor }. \r\n";
                        }

                        if (MinValor.HasValue && Convert.ToDecimal(Valor) < MinValor)
                        {
                            TieneErrores = true;
                            Errores = msgErrRango;
                        }

                        if (MaxValor.HasValue && Convert.ToDecimal(Valor) > MaxValor)
                        {
                            TieneErrores = true;
                            Errores = msgErrRango;
                        }

                    }
                }
            }

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
       

      
    }
}
