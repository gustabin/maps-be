
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [ExcludeFromCodeCoverage]
    [ProxyProvider("DBSMC", Owner = "SMC")]
    public class SmcDataAccess : BaseProxy, ISmcDA
    {        
        public virtual SaldoConcertadoNoLiquidadoResponse EjecutarSaldoConcertadoNoLiquidado(SaldoConcertadoNoLiquidadoRequest entity)
        {
            var request = entity.MapperClass<SaldoConcertadoNoLiquidadoDbReq>(TypeMapper.IgnoreCaseSensitive);
            var resp = this.Provider.GetFirst<SaldoConcertadoNoLiquidadoDbResp>(CommandType.StoredProcedure, request);

            request.CheckError();

            return resp.MapperClass<SaldoConcertadoNoLiquidadoResponse>(TypeMapper.IgnoreCaseSensitive);
        }

        public virtual void InsertOrder(InsertOrderRequest entity)
        {
            var request = entity.MapperClass<InsertOrderDbReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();
        }

        public virtual void UpdateOrder(UpdateOrderRequest entity)
        {
            var request = entity.MapperClass<UpdateOrderDbReq>(TypeMapper.IgnoreCaseSensitive);

            Provider.ExecuteNonQuery(CommandType.StoredProcedure, request);

            request.CheckError();
        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            var request = entity.MapperClass<ChequeoAccesoGSAReq>(TypeMapper.IgnoreCaseSensitive);
            var li = Provider.GetCollection<ChequeoAccesoResp>(CommandType.StoredProcedure, request);
            if (li.Any())
                return li.First().MapperClass<ChequeoAcceso>(TypeMapper.IgnoreCaseSensitive);
            throw new DBCodeException(-1, "Error SMC");
        }

        public string GetInfoDB(long id)
        {
            BaseProxySeguridad seg = new BaseProxySeguridad();
            UsuarioPasswordBaseClaves usuario = seg.ObtenerUsuarioBaseDeClaves(id);
            return Encrypt.EncryptToBase64String(string.Format("{0}||{1}", usuario.Usuario, usuario.Password));
        }
    }
}
