﻿using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ComponenteListadoFondos, Owner = Owner.Maps)]
    internal class ValorCompListadoFondo : FormularioBaseReq, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_COMPONENTE", BindOnNull = true, Type = OracleDbType.Long)]
        public long IdComponente { get; set; }
                
    }
}
