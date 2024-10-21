namespace Isban.Maps.Host.Controller
{
    using Entity.Controles;
    using Entity.Controles.Customizados;
    using Entity.Extensiones;
    using Entity.Request;
    using Entity.Response;
    using IBussiness;
    using Isban.Maps.Entity.Base;
    using Isban.Maps.Entity.Constantes.Estructuras;
    using Isban.Mercados.LogTrace;
    using Isban.Mercados.Service;
    using Isban.Mercados.Service.InOut;
    using Mercados;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Http;
    /// <summary>
    /// comentario
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiciosMapsController : ServiceWebApiMaps
    {
        #region MAPS Original
        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public virtual Response<FormularioResponse> AltaAdhesion(RequestSecurity<FormularioRequest> entity)
        {
            try
            {

                Response<FormularioResponse> response = null;
                FormularioResponse result = null;

                string[] serviciosWizard = new string[] { "SAF", "RTF", "AGD" };

                if (entity.Datos.IdServicio != null && entity.Datos.IdServicio != "PDC")
                {
                    var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IWizardBusiness>();

                    entity.Datos.Canal = entity.Canal;
                    entity.Datos.SubCanal = entity.SubCanal;
                    var firma = entity.MapperClass<DatoFirmaMaps>();
                    response = CheckSecurityAndTrace(() => bussiness.AltaAdhesion(firma, entity.Datos.ToFormularioMaps()), entity);
                }
                else
                {
                    var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMapsServiciosBusiness>();
                    var bussinessPDC = Mercados.UnityInject.DependencyFactory.Resolve<IMapsPDCServiciosBusiness>();


                    entity.Datos.Canal = entity.Canal;
                    entity.Datos.SubCanal = entity.SubCanal;
                    var firma = entity.MapperClass<DatoFirmaMaps>();

                    if (entity.Datos.IdServicio == null) //-----> Servicios disponibles
                    {
                        response = CheckSecurityAndTrace(() => bussiness.AltaAdhesion(entity.Datos.ToFormularioMaps(), firma), entity);
                    }
                    else
                    {

                        LoggingHelper.Instance.Information($"Controller: Inicio PDCAltaAdhesion");
                        response = CheckSecurityAndTrace(() => bussinessPDC.PDCAltaAdhesion(entity.Datos.ToFormularioMaps(), firma), entity);
                        LoggingHelper.Instance.Information($"Controller: Fin PDCAltaAdhesion");
                    }
                }
                LoggingHelper.Instance.Information($"Controller. Data: {JsonConvert.SerializeObject(entity)}");
                LoggingHelper.Instance.Information($"Controller. Response: {JsonConvert.SerializeObject(response)}");
                response.Datos.Nup = entity.Datos.Nup;
                response.Datos.Segmento = entity.Datos.Segmento;
                response.Datos.Canal = entity.Canal;
                response.Datos.SubCanal = entity.SubCanal;
                response.Datos.IdServicio = entity.Datos.IdServicio;
                return response;
            }
            catch (Exception ex)
            {
                LoggingHelper.Instance.Error("Error:", $"{ex.Message} | {ex.InnerException.Message}");
                throw;
            }
            
        }

        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public virtual Response<FormularioResponse> BajaAdhesion(RequestSecurity<FormularioRequest> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMapsServiciosBusiness>();
            Response<FormularioResponse> response = null;

            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;
            var firma = entity.MapperClass<DatoFirmaMaps>();

            response = CheckSecurityAndTrace(() => bussiness.BajaAdhesion(entity.Datos.ToFormularioMaps(), firma), entity);

            response.Datos.Nup = entity.Datos.Nup;
            response.Datos.Segmento = entity.Datos.Segmento;
            response.Datos.Canal = entity.Canal;
            response.Datos.SubCanal = entity.SubCanal;
            response.Datos.IdServicio = entity.Datos.IdServicio;

            return response;
        }

        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public virtual Response<List<ConsultaFondosAGDResponse>> ConsultaFondosAGD(RequestSecurity<ConsultaFondosAGDRequest> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMapsServiciosBusiness>();
            return CheckSecurityAndTrace(() => bussiness.ConsultaFondosAGD(entity.Datos), entity);
        }

        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public virtual Response<FormularioResponse> ConsultaAdhesion(RequestSecurity<FormularioRequest> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMapsServiciosBusiness>();
            Response<FormularioResponse> response = null;

            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;
            var firma = entity.MapperClass<DatoFirmaMaps>();

            response = CheckSecurityAndTrace(() => bussiness.ConsultaAdhesion(entity.Datos.ToFormularioMaps(), firma), entity);

            response.Datos.Nup = entity.Datos.Nup;
            response.Datos.Segmento = entity.Datos.Segmento;
            response.Datos.Canal = entity.Canal;
            response.Datos.SubCanal = entity.SubCanal;
            response.Datos.IdServicio = entity.Datos.IdServicio;

            return response;
        }

        /// <summary>
        /// Obtener formulario simulacion a partir de IdSimulacion
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public virtual Response<ObtenerFormAdhesionesResp> ObtenerFormAdhesiones(RequestSecurity<ObtenerFormAdhesionesReq> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMapsServiciosBusiness>();
            var result = bussiness.ObtenerFormAdhesiones(entity.Datos);
            return CheckSecurityAndTrace(() => bussiness.ObtenerFormAdhesiones(entity.Datos), entity);
        }

        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public virtual Response<ObtenerIdAdhesionResp> ObtenerIdAdhesion(RequestSecurity<ObtenerIdAdhesionReq> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMapsServiciosBusiness>();
            var result = bussiness.ObtenerIdAdhesion(entity.Datos);
            return CheckSecurityAndTrace(() => bussiness.ObtenerIdAdhesion(entity.Datos), entity);
        }



        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<ObtenerRTFDisponiblesResponse> ObtenerRTFDisponiblesPorCliente(RequestSecurity<RTFWorkflowOnDemandReq> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMapsServiciosBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerRTFDisponiblesPorCliente(entity), entity);
        }


        [HttpPost]
        [MetodoInfo(ModoObtencion.Metodo)]
        public virtual Response<List<ArchivoRTF>> ObtenerPdfPorCuentaRTF(RequestSecurity<RTFWorkflowOnDemandReq> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IMapsServiciosBusiness>();

            return CheckSecurityAndTrace(() => bussiness.ObtenerPdfPorCuentaRTF(entity), entity);
        }
        #endregion


        #region Aplicativos No T-Banco
        [MetodoInfo(ModoObtencion.Metodo)]
        [HttpPost]
        public virtual Response<List<FormularioResponse>> ObtenerTodosLosPasos(RequestSecurity<MapsBase> entity)
        {
            var bussiness = Mercados.UnityInject.DependencyFactory.Resolve<IWizardBusiness>();
            Response<List<FormularioResponse>> response = null;
            var firma = entity.MapperClass<DatoFirmaMaps>();

            entity.Datos.Canal = entity.Canal;
            entity.Datos.SubCanal = entity.SubCanal;

            response = CheckSecurityAndTrace(() => bussiness.ObtenerTodosLosPasos(entity.Datos, firma), entity);

            if (response.Datos != null && (response.Datos as List<FormularioResponse>)?.Count > 0)
            {
                (response.Datos as List<FormularioResponse>).ForEach(x =>
                {
                    x.Segmento = entity.Datos.Segmento;
                    x.Canal = entity.Canal;
                    x.SubCanal = entity.SubCanal;
                    x.IdServicio = entity.Datos.IdServicio;
                });
            }

            return response;
        }
        #endregion
    }
}
