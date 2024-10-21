
namespace Isban.Maps.DataAccess.DBRequest
{
    using Base;
    using Isban.Mercados.DataAccess.OracleClient;
    using Common.Data;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;

    [ProcedureRequest("GET_MAPS_CTRL_DEFINICION_CUR", Package = Package.ControlServicios, Owner = Owner.Maps)]
    internal class ValorCtrlServicioDbReq : FormularioBaseReq, IProcedureRequest
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTAS_OPERATIVAS", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string ListaCtasOperativas { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTAS_REPATRIACION", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string ListaCtasRepatriacion{ get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTAS_TITULOS", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string ListaCtasTitulo { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTAS_PDC", BindOnNull = true, Type = OracleDbType.Varchar2)]
        public string ListaCtasPDC { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVIDOR_WIN", BindOnNull = true, Size = 100, Type = OracleDbType.Varchar2)]
        public string ServidorWin { get; set; }

        
    }
}
