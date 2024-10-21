using Isban.Maps.Host.Controller;

namespace Isban.Maps.Host.Webapi.Web
{
    using Entity.Base;
    using Maps.Entity;
    using Mercados.Service.InOut;
    using System;
    using System.Linq;
    public partial class Chequeo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                    this.lblError.Text = string.Empty;

                ChequeoServiceController service = new ChequeoServiceController();
                var req = new Request<EntityBase>
                {
                    Canal = "00",
                    SubCanal = "00",
                    Datos = new EntityBase { Ip = KnownParameters.IpDefault, Usuario = KnownParameters.UsuarioDefault }
                };
                var il = service.Chequeo(req);
                this.gvResultado.DataSource = il.Datos;
                this.gvResultado.DataBind();
                if (il.Datos.Any(o => !o.Ok))
                {
                    this.lblEstado.InnerText = "KO";
                }
                else
                {
                    this.lblEstado.InnerText = "OK";
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.ToString();
            }

        }
    }
}