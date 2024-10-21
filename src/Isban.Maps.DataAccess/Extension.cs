
namespace Isban.Maps.DataAccess
{
    using Mercados;
    using Mercados.DataAccess.OracleClient;

    public static class BaseRequestExten
    {
        public static void CheckError(this BaseRequest request)
        {
            if (request.CodigoError.GetValueOrDefault() != 0)
            {
                throw new DBCodeException(request.CodigoError.GetValueOrDefault(), request.Error);
            }
        }
    }
}
