
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Common.Data;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("SP_OBTENER_ORIGEN", Package = Package.MapsGraphTools, Owner = Owner.Maps)]
    internal class ObtenerOrigenDbReq : BaseReq
    {
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_FRM_ORIGEN", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 50)]
        public string FrmOrigen { get; set; }

    }
}
