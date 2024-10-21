using Isban.Common.Data;
using Isban.Maps.DataAccess.Base;
using Isban.Maps.Entity.Constantes.Enumeradores;
using Isban.Mercados;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Isban.Maps.DataAccess.DBRequest
{
    [ProcedureRequest("SP_PASO_ANTERIOR", Package = Package.GraphToolsWizard, Owner = Owner.Maps)]
    internal class ConsultaPasoAnteriorAdhDbReq : BaseRequest, IProcedureRequest
    {
        /// <summary>
        /// Id de Servicio
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, Size = 60, ValueConverter = typeof(RequestConvert<string>))]
        public string IdServicio { get; set; }

        /// <summary>
        /// Id de Formulario
        /// </summary>
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_ID_FORMULARIO", BindOnNull = true, Type = OracleDbType.Int64, ValueConverter = typeof(RequestConvert<long?>), Size = 10)]
        public long? FormularioId { get; set; }

        public virtual void CheckError()
        {
            if (this.CodigoError.GetValueOrDefault() != 0)
            {
                switch (this.CodigoError.GetValueOrDefault())
                {
                    case (long)TiposDeError.ErrorBaseDeDatos:
                        throw new DBCodeException(this.CodigoError.GetValueOrDefault(), $"Excepción de BD: { this.Error}");

                    case (long)TiposDeError.NoSeEncontraronDatos:
                        throw new DBCodeException(this.CodigoError.GetValueOrDefault(), $"No se encontró información en la BD { this.Error}");

                    default:
                        throw new DBCodeException(this.CodigoError.GetValueOrDefault(), $"Error no controlado: { this.Error}");
                }
            }
        }
    }
}
