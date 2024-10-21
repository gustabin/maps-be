namespace Isban.Maps.Host.Webapi
{
    using Isban.Mercados.Configuration;
    using System;
    using System.Web;
    using System.Web.Http;
    public class Global : HttpApplication
    {
        public Global()
        {
            start = start ?? new ConfigurationStart();
        }

        #region atributo
        private ConfigurationStart start;
        #endregion
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);

            if (start == null)
                start = new ConfigurationStart();

            start.Start();
        }
    }
}