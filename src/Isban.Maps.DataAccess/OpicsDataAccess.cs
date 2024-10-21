
namespace Isban.Maps.DataAccess
{
    using DBRequest;
    using DBResponse;
    using Entity.Request;
    using Entity.Response;
    using IDataAccess;
    using Isban.Maps.Entity;
    using Isban.Maps.Entity.Base;
    using Mercados;
    using Mercados.DataAccess;
    using Mercados.DataAccess.OracleClient;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    [ProxyProvider("DBOPICS", Owner = "OPICS")]
    public class OpicsDataAccess : BaseProxy, IOpicsDA
    {
        /// <summary>
        /// Busco Las Atits Para el Nup y/o cuenta_bp ingresada
        /// </summary>
        /// <param name="entity">Request del servicio</param>
        /// <returns>Atits Para el Nup y/o cuenta_bp ingresada</returns>
        public virtual List<ConsultaLoadAtisResponse> ObtenerAtis(ConsultaLoadAtisRequest entity)
        {
            var request = entity.MapperClass<ConsultaLoadAtisDbReq>(TypeMapper.IgnoreCaseSensitive);
            var atisData = this.Provider.GetCollection<ConsultaLoadAtisDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            var a = from b in atisData
                    select new ConsultaLoadAtisResponse
                    {
                        CuentaBp = long.Parse(b.CuentaBp.ToString().Substring(2, 2)),
                        CuentaAtit = b.CuentaAtit
                    };

            var loadAtis = atisData.MapperEnumerable<ConsultaLoadAtisResponse>(TypeMapper.IgnoreCaseSensitive);

            return loadAtis.ToList();
        }

        /// <summary>
        /// Consulta los saldos de la Banca Privada
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual LoadSaldosResponse EjecutarLoadSaldos(LoadSaldosRequest entity)
        {
            var request = entity.MapperClass<LoadSaldosDbReq>(TypeMapper.IgnoreCaseSensitive);
            var resp = this.Provider.GetCollection<LoadSaldosDataAccessResponse>(CommandType.StoredProcedure, request);

            request.CheckError();
            var result = new LoadSaldosResponse();

            result.ListaSaldos = resp.MapperEnumerable<Saldos>(TypeMapper.IgnoreCaseSensitive).ToList();

            //validar si devuelve un valor o varios.
            return result;
        }
        
        public virtual long? AltaCuentaTiluloOpics(AltaCuentaOpicsReq entity)
        {
            var request = entity.MapperClass<AltaCuentaOpicsDBReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            //request.CheckError();

            return request.CuentaTitulo;
        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            var request = entity.MapperClass<ChequeoAccesoGSAReq>(TypeMapper.IgnoreCaseSensitive);
            var li = Provider.GetCollection<ChequeoAccesoResp>(CommandType.StoredProcedure, request);
            if (li.Any())
                return li.First().MapperClass<ChequeoAcceso>(TypeMapper.IgnoreCaseSensitive);
            throw new DBCodeException(-1, "Error OPICS");
        }

        public string GetInfoDB(long id)
        {
            BaseProxySeguridad seg = new BaseProxySeguridad();
            UsuarioPasswordBaseClaves usuario = seg.ObtenerUsuarioBaseDeClaves(id);
            return Encrypt.EncryptToBase64String(string.Format("{0}||{1}", usuario.Usuario, usuario.Password));
        }
    }
}
