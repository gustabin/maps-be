
namespace Isban.Maps.IDataAccess
{
    using Entity.Response;
    using Isban.Maps.Entity;
    using Isban.Maps.Entity.Base;
    using Isban.Maps.Entity.Request;
    using System.Collections.Generic;

    public interface IOpicsDA
    {
        string ConnectionString { get; }
        ChequeoAcceso Chequeo(EntityBase entity);
        string GetInfoDB(long id);

        List<ConsultaLoadAtisResponse> ObtenerAtis(ConsultaLoadAtisRequest consultaLoadAtisRequest);

        LoadSaldosResponse EjecutarLoadSaldos(LoadSaldosRequest entity);

        long? AltaCuentaTiluloOpics(AltaCuentaOpicsReq entity);
    }
}
