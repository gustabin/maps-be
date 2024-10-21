
namespace Isban.Maps.Host.Webapi.Web
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using Isban.Mercados.Service.InOut;
    using Maps.Entity;
    using Entity.Base;

    public partial class Estado : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //ChequeoServiceController service = new ChequeoServiceController();
                //var req = new Request<EntityBase>
                //{
                //    Canal = "00",
                //    SubCanal = "00",
                //    Datos = new EntityBase { Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault }
                //};
                //var response = service.Chequeo(req);
                hEstado.InnerText =  "OK";
            }
            catch (Exception)
            {
                hEstado.InnerText = "KO";
            }
        }
    }
}