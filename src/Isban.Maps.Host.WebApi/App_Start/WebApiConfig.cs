namespace Isban.Maps.Host.Webapi
{
    using Isban.Mercados.Extensions;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Web.Http;
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "MAPS{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Web API configuration and services
            string jsonFormat = System.Configuration.ConfigurationManager.AppSettings["JsonFormat"];
            if (jsonFormat != null && jsonFormat.ToUpper() == "MICROSOFT")
            {
                config.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            }
            GlobalConfiguration.Configuration.MessageHandlers.Add(new MessageLoggingHandler());
            // Web API routes
            config.Formatters.JsonFormatter.SerializerSettings.Converters.AddRange(new List<JsonConverter>
            {
                new Isban.Mercados.CurrencyConverter(),
                new Isban.Mercados.IndexConverter(),
                new Isban.Mercados.NumberTruncate2Convert(),
                new Isban.Mercados.NumberTruncate3Convert(),
                new Isban.Mercados.NumberTruncate4Convert(),
                new Isban.Mercados.NumberTruncate5Convert(),
                new Isban.Mercados.NumberTruncate6Convert(),
                new Isban.Mercados.NumberTruncate7Convert(),
                new Isban.Mercados.NumberTruncate8Convert(),
                new Isban.Mercados.Number2Converter(),
                new Isban.Mercados.Number3Converter(),
                new Isban.Mercados.Number4Converter(),
                new Isban.Mercados.Number5Converter(),
                new Isban.Mercados.Number6Converter(),
                new Isban.Mercados.Number7Converter(),
                new Isban.Mercados.Number8Converter(),
                new Isban.Mercados.Number9Converter()

            });


        }
    }
}
