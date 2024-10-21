using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using System.Collections.Generic;

namespace Isban.Maps.IBussiness
{
    public interface IWizardBusiness
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        FormularioResponse ConsultaAdhesion(DatoFirmaMaps firma, FormularioResponse entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        FormularioResponse BajaAdhesion(DatoFirmaMaps firma, FormularioResponse entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        FormularioResponse AltaAdhesion(DatoFirmaMaps firma, FormularioResponse entity);
        List<FormularioResponse> ObtenerTodosLosPasos(MapsBase datos, DatoFirmaMaps firma);

        FormularioResponse ObtenerListaDeServicios(FormularioResponse entity);
       
    }

}
