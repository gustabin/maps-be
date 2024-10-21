using Isban.Maps.Entity.Extensiones;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Isban.Maps.Entity.Base
{
    public abstract class Validaciones
    {
        public bool TieneErrores { get; set; }

        public string Errores { get; set; }

        public abstract bool Validar(string idServicio = null, string idFormulario = null);

        protected void ValidarLargo<T>(string nombre, T valor, int maximo)
        {
            if (string.Format("{0}", valor).Length > maximo)
            {
                Errores += ($"Largo de la propiedad {nombre} (Valor: {valor}) debe ser menor o igual a {maximo}.\r\n");
                TieneErrores = true;
            }
        }

        protected void ValidarDecimalesLargo<T>(string nombre, T valor, decimal maximo)
        {
            if (string.Format("{0}", valor).Length > maximo)
            {
                Errores += ($"La cantidad de decimales de la propiedad {nombre} debe ser menor o igual a {maximo}.\r\n");
                TieneErrores = true;
            }
        }

        protected void ValidarLargo<T>(string nombre, T valor, decimal maximo)
        {
            if (string.Format("{0}", valor).Length > maximo)
            {
                Errores += ($"Largo de la propiedad {nombre} (Valor: {valor}) debe ser menor o igual a {maximo}.\r\n");
                TieneErrores = true;
            }
        }

        protected void ValidarLargoMinimo<T>(string nombre, T valor, int minimo)
        {
            if (string.Format("{0}", valor).Length < minimo)
            {
                Errores += ($"Largo de la propiedad {nombre} (Valor: {valor}) debe ser mayor o igual a {minimo}.\r\n");
                TieneErrores = true;
            }
        }

        protected void ValidarLargoMinimo<T>(string nombre, T valor, decimal minimo)
        {
            if (string.Format("{0}", valor).Length < minimo)
            {
                Errores += ($"Largo de la propiedad {nombre} (Valor: {valor}) debe ser mayor o igual a {minimo}.\r\n");
                TieneErrores = true;
            }
        }

        protected void ValidarLargoRango<T>(string nombre, T valor, int minimo, int maximo)
        {
            ValidarLargoMinimo(nombre, valor, minimo);
            ValidarLargo(nombre, valor, maximo);
        }

        protected void ValidarLargoRango<T>(string nombre, T valor, decimal minimo, decimal maximo)
        {
            ValidarLargoMinimo(nombre, valor, minimo);
            ValidarLargo(nombre, valor, maximo);
        }

        protected void ValidarLargoRango<T>(string nombre, T valor, int minimoMaximo)
        {
            ValidarLargoMinimo(nombre, valor, minimoMaximo);
            ValidarLargo(nombre, valor, minimoMaximo);
        }

        protected bool EsNumerico(string numero)
        {
            if (string.IsNullOrEmpty(numero))
            {
                return true;
            }
            return Regex.IsMatch(numero, @"^\d+$");
        }

        protected bool EsDecimal(string numero)
        {
            if (!string.IsNullOrWhiteSpace(numero))
            {
                decimal result;

                return decimal.TryParse(numero, out result);
            }
            else
                return false;
        }

        protected void CaracteresValidos(string nombre, string texto, string regEXP, string invalidos)
        {
            if (!Regex.IsMatch(texto, regEXP))
            {
                Errores += ($"Caracteres inválidos para el campo {nombre}: ({invalidos})");
                TieneErrores = true;
            }
        }

        protected bool EsFecha(string fecha)
        {
            DateTime temp;
            return DateTime.TryParse(fecha, out temp);
        }

        protected void ValidarFormatoFecha(string nombre, string valor)
        {
            DateTime result;
            string dateFormat = "yyyy/MM/dd";

            if (!DateTime.TryParseExact(valor, dateFormat, null, DateTimeStyles.None, out result))
            {
                Errores += ($"Formato de la propiedad {nombre} (Valor: {valor}) debe ser igual a {dateFormat}.\r\n");
                TieneErrores = true;
            }
        }

        protected void ValidarFormatoHora(string nombre, string valor)
        {
            TimeSpan result;
            string hrFormat = "HH:mm:ss";

            if (!TimeSpan.TryParseExact(valor, hrFormat, null, TimeSpanStyles.None, out result))
            {
                Errores += ($"Formato de la propiedad {nombre} (Valor: {valor}) debe ser igual a {hrFormat}.\r\n");
                TieneErrores = true;
            }
        }

        protected void ValidarContenido<T>(string nombre, T valor, params T[] valoresValidos)
        {
            if (!valor.Is(valoresValidos))
            {
                Errores += ($"{nombre} debe ser {string.Join(",", valoresValidos)}.\r\n");
                TieneErrores = true;
            }
        }

        /// <summary>
        /// Método para validar los campos requeridos
        /// </summary>
        /// <typeparam name="T">Tipo de dato</typeparam>
        /// <param name="nombre">Nombre de la propiedad</param>
        /// <param name="valor">Valor a ser evaluado</param>
        protected void EsVacio<T>(string nombre, T valor)
        {
            if (EqualityComparer<T>.Default.Equals(valor, default(T)))
            {
                Errores += ($"{nombre} es requerido.\r\n");
                TieneErrores = true;
            }
            else if (valor is string && string.IsNullOrWhiteSpace(valor as string))
            {
                Errores += ($"{nombre} es requerido y no puede ser vacio.\r\n");
                TieneErrores = true;
            }
        }

        protected void EsVacio<T>(string nombre, params T[] valores)
        {
            foreach (var valor in valores)
            {
                if (EqualityComparer<T>.Default.Equals(valor, default(T)))
                {
                    Errores += ($"{nombre} es requerido.\r\n");
                    TieneErrores = true;
                }
                else if (valor is string && string.IsNullOrWhiteSpace(valor as string))
                {
                    Errores += ($"{nombre} es requerido y no puede ser vacio.\r\n");
                    TieneErrores = true;
                }
            }
        }
    }
}
