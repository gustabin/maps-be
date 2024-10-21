namespace Isban.Maps.Host.Webapi
{
    using Mercados;
    using System;
    using System.Net.Http;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    /// <summary>
    /// Clase: MessageHandler
    /// </summary>
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public abstract class MessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
        /// </returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            var requestInfo = string.Format("{0} {1}", request.Method, request.RequestUri);
            var ipRequest = GetClientIp(request);
            var userRequest = request.GetUserPrincipal() != null ? request.GetUserPrincipal().Identity.Name : "N/A";
            var fechaHora = DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff");
            var requestMessage = await request.Content.ReadAsByteArrayAsync();

#if DEBUG
            await IncommingMessageAsync(corrId, requestInfo, requestMessage);
#endif

            HttpResponseMessage response;
            try
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                WebMessageLoggingHelper.Instance.Error(string.Format("Error {0}", ex));
                throw;
            }
            if (request.Method.Method == "OPTIONS")
                return response;

            fechaHora = string.Format("{0} - {1}", fechaHora, DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"));
            if (!response.IsSuccessStatusCode && request.Method.Method == "POST")
            {
                var mensaje = Encoding.UTF8.GetString(requestMessage);
                var contentOrigen = request.Content;
                if (!string.IsNullOrWhiteSpace(mensaje))
                {
                    //Reintento la llamada serializando otra vez.
                    try
                    {
                        var obj = mensaje.DeserializarToJson<dynamic>(GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
                        if (obj == null)
                        {
                            WebMessageLoggingHelper.Instance.Error(string.Format("{0} {1}- Error no puedo Deserializar Request: {2}\r\n{3}", corrId, fechaHora, requestInfo, mensaje));
                        }
                        else
                        {
                            request.Content = new ObjectContent(obj.GetType(), obj, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                            response = await base.SendAsync(request, cancellationToken);
                            if (!response.IsSuccessStatusCode)
                            {
                                WebMessageLoggingHelper.Instance.Error(string.Format("{0} {1}- Reintento Error.", corrId, fechaHora));
                            }
                        }
                    }
                    catch
                    {
                        request.Content = contentOrigen;
                    }
                }
            }


#if DEBUG
            byte[] responseMessage = null;
            if (response.IsSuccessStatusCode)
            {

                responseMessage = await response.Content.ReadAsByteArrayAsync();

            }
            else
            {

                responseMessage = Encoding.UTF8.GetBytes(response.ReasonPhrase);

            }
            await OutgoingMessageAsync(corrId, requestInfo, responseMessage);
#endif

#if !DEBUG

            byte[] responseMessage = await response.Content.ReadAsByteArrayAsync();
            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    byte[] responseReasonPhrase = Encoding.UTF8.GetBytes(response.ReasonPhrase);
                    WebMessageLoggingHelper.Instance.Error(string.Format("{0} - {1} Request: {2}\r\n{3}\r\nResponse: {4} ReasonPhrase: {5}", corrId, fechaHora, requestInfo, Encoding.UTF8.GetString(requestMessage), Encoding.UTF8.GetString(responseMessage), Encoding.UTF8.GetString(responseReasonPhrase)));
                }
            }
            catch { }
#endif

            return response;
        }

        /// <summary>
        /// Incommings the message asynchronous.
        /// </summary>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="requestInfo">The request information.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected abstract Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message);
        /// <summary>
        /// Outgoings the message asynchronous.
        /// </summary>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="requestInfo">The request information.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected abstract Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message);

        /// <summary>
        /// Gets the client ip.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private string GetClientIp(HttpRequestMessage request)
        {

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;

            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Limpiars the datos sensibles.
        /// </summary>
        /// <param name="texto">The texto.</param>
        /// <returns></returns>
        private string LimpiarDatosSensibles(string texto)
        {

            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["DatoSensibleLog"]))
            {
                return texto;
            }
            string[] camposLimpiar = System.Configuration.ConfigurationManager.AppSettings["DatoSensibleLog"].Split('|');
            foreach (string campo in camposLimpiar)
            {
                texto = this.LimpiarInfo(campo, texto);
            }
            return texto;

        }
        /// <summary>
        /// Limpiars the information.
        /// </summary>
        /// <param name="campo">The campo.</param>
        /// <param name="texto">The texto.</param>
        /// <returns></returns>
        private string LimpiarInfo(string campo, string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return texto;
            }
            string textOrig = texto;
            try
            {
                campo = string.Format("{0}{1}{2}", "\"", campo, "\"");
                if (texto.ToUpper().IndexOf(campo.ToUpper(), StringComparison.Ordinal) < 0)
                {
                    return texto;
                }
                string textReturn = string.Empty;
                while (texto.ToUpper().IndexOf(campo.ToUpper(), StringComparison.Ordinal) >= 0)
                {
                    textReturn += texto.Substring(0, (texto.ToUpper().IndexOf(campo.ToUpper(), StringComparison.Ordinal) + campo.Length));
                    texto = texto.Substring((texto.ToUpper().IndexOf(campo.ToUpper(), StringComparison.Ordinal) + campo.Length));
                    textReturn += texto.Substring(0, texto.IndexOf(':') + 1);
                    texto = texto.Substring(texto.IndexOf(':') + 1);
                    if (texto.IndexOf('\"') < texto.IndexOf(','))
                    {
                        bool inicio = true;
                        bool fin = false;
                        bool anterioBarra = false;
                        string limpio = "\"DATOBORRADO\"";
                        texto = texto.Substring(texto.IndexOf('\"') + 1);
                        foreach (var caracter in texto)
                        {
                            if (inicio)
                            {
                                inicio = false;
                                continue;
                            }
                            if (fin)
                            {
                                limpio += caracter;
                                continue;
                            }
                            if (caracter.Equals('\"') && !anterioBarra)
                            {
                                fin = true;
                                continue;
                            }
                            anterioBarra = caracter.Equals('\\');
                        }
                        texto = limpio;
                    }
                    else
                    {
                        textReturn += "DATOBORRADO, ";
                        texto = texto.Substring((texto.ToUpper().IndexOf(",", StringComparison.Ordinal) + 1));
                    }
                }
                textReturn += texto;
                return textReturn;
            }
            catch
            {
                return textOrig;
            }

        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MessageHandler" />
    public class MessageLoggingHandler : MessageHandler
    {
        /// <summary>
        /// Incommings the message asynchronous.
        /// </summary>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="requestInfo">The request information.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected override async Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message)
        {
            try
            {


                await Task.Run(() =>
                 WebMessageLoggingHelper.Instance.Debug(string.Format("{0} {1} - Request: {2}\r\n{3}", correlationId, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), requestInfo, Encoding.UTF8.GetString(message))));
            }
            catch (Exception ex )
            {  
                               
            }
        }


        /// <summary>
        /// Outgoings the message asynchronous.
        /// </summary>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="requestInfo">The request information.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected override async Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message)
        {
            await Task.Run(() =>
                WebMessageLoggingHelper.Instance.Debug(string.Format("{0} {1} - Response: {1}\r\n{2}", correlationId, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), requestInfo, Encoding.UTF8.GetString(message))));
        }
    }
}
