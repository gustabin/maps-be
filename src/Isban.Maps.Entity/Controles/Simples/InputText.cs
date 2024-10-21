
namespace Isban.Maps.Entity.Controles
{
    using Constantes.Enumeradores;
    using Isban.Maps.Entity.Constantes.Estructuras;
    using Isban.Mercados.LogTrace;
    using Responsabilidad;
    using System;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    [DataContract]
    public class InputText<T> : ControlSimple
    {   
        public InputText()
            :base(new AsignarDatosSimple())
        {

        }

        public InputText(IAsignarDatos asignar)
            : base(asignar)
        {

        }


        [DataMember]
        public T Valor { get; set; }

        [DataMember]
        public decimal MaxLength { get; set; }

        [DataMember]
        public decimal MinLength { get; set; }
      
        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            base.Validar();

            if (Requerido)
                EsVacio(Nombre, Valor);

            ValidarLargoRango("Valor", Valor, MinLength, MaxLength);

            switch (this.Nombre)
            {
                case NombreComponente.Email:
                    try
                    {
                        string email = Valor.ToString();
                        string[] aEmail;

                        if (email.Length > 0 && email.Substring(email.Length - 1, 1) == ",")
                        {
                            email = email.Substring(0, email.Length - 1);
                        }
                        aEmail = email.Split(',');
                        foreach (var item in aEmail)
                        {
                            if (!this.Bloqueado && (!ValidarEmail1(item) || !ValidarEmailRE(item)))
                            {
                                Errores += ($"{this.Nombre} no tiene formato válido.\r\n");
                                TieneErrores = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TieneErrores = true;
                        Errores = ex.Message;
                        Error_tecnico += ($"{this.Nombre} {ex.InnerException}.\r\n");
                    }
                    break;
            }

            if (Valor != null)
            {

                switch (this.Nombre)
                {
                    case NombreComponente.Alias:
                        CaracteresValidos("Valor", Valor?.ToString(), @"^[a-zA-ZáéíóúüÁÉÍÓÚÜ0-9@.,-_/\s?¿¿!¡$]+$", Informacion);
                        break;
                    default:
                        CaracteresValidos("Valor", Valor?.ToString(), @"^[a-zA-ZáéíóúüÁÉÍÓÚÜ0-9@.,-_/\s$]+$", Informacion);
                        break;
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

        private bool ValidarEmail1(string emailAddress)
        {
            string sValidEmail = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            Regex reValidEmail = new Regex(sValidEmail);

            return reValidEmail.IsMatch(emailAddress);
        }

        private bool ValidarEmailRE(string emailAddress)
        {
            var sQtext = "[^\\x0d\\x22\\x5c\\x80-\\xff]";
            var sDtext = "[^\\x0d\\x5b-\\x5d\\x80-\\xff]";
            var sAtom = "[^\\x00-\\x20\\x22\\x28\\x29\\x2c\\x2e\\x3a-\\x3c\\x3e\\x40\\x5b-\\x5d\\x7f-\\xff]+";
            var sQuotedPair = "\\x5c[\\x00-\\x7f]";
            var sDomainLiteral = "\\x5b(" + sDtext + "|" + sQuotedPair + ")*\\x5d";
            var sQuotedString = "\\x22(" + sQtext + "|" + sQuotedPair + ")*\\x22";
            var sDomain_ref = sAtom;
            var sSubDomain = "(" + sDomain_ref + "|" + sDomainLiteral + ")";
            var sWord = "(" + sAtom + "|" + sQuotedString + ")";
            var sDomain = sSubDomain + "(\\x2e" + sSubDomain + ")*";
            var sLocalPart = sWord + "(\\x2e" + sWord + ")*";
            var sAddrSpec = sLocalPart + "\\x40" + sDomain; // complete RFC822 email address spec
            var sValidEmail = "^" + sAddrSpec + "$"; // as whole string

            var reValidEmail = new Regex(sValidEmail);

            return reValidEmail.IsMatch(emailAddress);
        }
        
    }
}
