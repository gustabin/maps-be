
namespace Isban.Maps.DataAccess.DBRequest
{
    using Isban.Maps.DataAccess.Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using System;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteFecha, Owner = Owner.Maps)]
    internal class CompMontoSuscripcionDbReq : FormularioBaseReq, IProcedureRequest
    {
        public override void CheckError()
        {
            base.CheckError();
        }
    }
}
