namespace Isban.Maps.Host.Controller
{
    using Configuration.Backend.Interception;
    using Entity.Constantes.Enumeradores;
    using Mercados;
    using Mercados.Service;
    using Mercados.Service.InOut;
    using System;
    using Entity.Interfaces;
    using Isban.Mercados.Security.Adsec.Entity;
    using Entity.Request;

    public class ServiceWebApiMaps : ServiceWebApiBase
    {
        public ServiceWebApiMaps()
        {
            this.OnBeforeCheckSecurity += ServiceWebApiMotor_OnBeforeCheckSecurity;
        }

        private void ServiceWebApiMotor_OnBeforeCheckSecurity(BeforeCheckSecurityEvent e)
        {
            if (e.Request == null)
            {
                throw new ArgumentNullException("", "Json mal formateado o no envia datos");
            }
            var request = e.Request as IDatoFirma;
            var request2 = e.Request as IRequest;
            var request3 = e.Request as RequestSecurity<FormularioRequest>;
            
            if (request != null)
            {
                if (string.IsNullOrWhiteSpace(request.Firma))
                    throw new ArgumentNullException("", "Firma Es obligatorio");

                if (string.IsNullOrWhiteSpace(request.Dato))
                    throw new ArgumentNullException("", "Dato Es obligatorio");
            }
            if (request2 != null)
            {
                if (string.IsNullOrWhiteSpace(request2.Canal))
                    throw new ArgumentNullException("", "Canal Es obligatorio");
                else if (request2.Canal.Length != 2)
                    throw new ArgumentException("Canal El parámetro debe tener 2 caracteres", "");

                if (string.IsNullOrWhiteSpace(request2.SubCanal))
                    throw new ArgumentNullException("", "SubCanal Es obligatorio");
                else if (request2.SubCanal.Length != 4)
                    throw new ArgumentException("SubCanal El parámetro debe tener 4 caracteres", "");
            }
            if (request3 != null)
            {
                request3.Datos.Usuario = Entity.KnownParameters.UsuarioDefault;
                request3.Datos.Ip = Entity.KnownParameters.IpDefault;

                if (string.IsNullOrWhiteSpace(request3.Datos.Ip))
                    throw new ArgumentException("IP debe contener un valor");
                else if (string.IsNullOrWhiteSpace(request3.Datos.Usuario))
                    throw new ArgumentException("Usuario debe contener un valor");
            }
        }

        public override void FillException<TRp>(Exception ex, Response<TRp> response)
        {
            if (typeof(TRp).IsClass)
            {
                response.Datos = (TRp)Activator.CreateInstance(typeof(TRp));

                if (ex is MapsException && response.Datos is IFormulario)
                {
                    var argException = (MapsException)ex;
                    response.Codigo = (long)TiposDeError.NoError;
                    response.Mensaje = $"Error: {argException.Message}";

                    var responseProperties = typeof(TRp).GetProperties();

                    foreach (System.Reflection.PropertyInfo propertyInfo in responseProperties)
                    {
                        try
                        {
                            propertyInfo.SetValue(response.Datos, propertyInfo.GetValue(argException.ParametroRequest));
                        }
                        catch (Exception ex1)
                        {
                            Isban.Mercados.LogTrace.LoggingHelper.Instance.Warning(ex1, "Fill Datos error.");
                            throw;
                        }

                    }


                    return;
                }

                response.Mensaje = ex.Message;

                response.Codigo = (long)TiposDeError.ErrorBusiness;
                if (ex.InnerException != null)
                {
                    response.MensajeTecnico = ex.InnerException.ToString();
                }
                else
                {
                    response.MensajeTecnico = ex.ToString();
                }
                if (ex is ICodeException)
                {
                    response.Codigo = (long)TiposDeError.ErrorBusiness;
                    if (ex.InnerException != null)
                    {
                        response.MensajeTecnico = ex.InnerException.ToString();
                    }
                }
            }
        }
    }

}