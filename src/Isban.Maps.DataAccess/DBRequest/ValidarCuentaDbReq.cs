
namespace Isban.Maps.DataAccess.DBRequest
{
    using Common.Data;
    using IDataAccess;
    using Isban.Mercados.DataAccess.OracleClient;
    using Mercados.DataAccess.ConverterDBType;
    using Oracle.ManagedDataAccess.Client;
    using System.Data;
    using System;
    using Entity.Constantes.Enumeradores;
    using Mercados;

    [ProcedureRequest("SP_VAL_CUENTA", Package = Package.MapsAdhesiones, Owner = Owner.Maps)]
    internal class ValidarCuentaDbReq : IProcedureRequest, IRequestBase
    {
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SUC_CTA_OPER", BindOnNull = true, Type = OracleDbType.Int32, ValueConverter = typeof(RequestConvert<int?>))]
        public int? SucursalCuentaOperativa { get; set; } 

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_CUENTA_TITULOS", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? CuentaTitulos { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_NRO_CTA_OPER", BindOnNull = true, Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? NroCuentaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_TIPO_CTA_OPER", BindOnNull = true, Type = OracleDbType.Int32, ValueConverter = typeof(RequestConvert<int?>))]
        public int? TipoCuentaOperativa { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SEGMENTO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string Segmento { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_SERVICIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>))]
        public string IdServicio { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_TIENE_ADHESION", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<decimal?>))]
        public decimal? TieneAdhesion { get; set; }

        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_COD_RES", Type = OracleDbType.Decimal, ValueConverter = typeof(RequestConvert<long?>))]
        public long? CodigoError { get; set; }
        
        [DBParameterDefinition(Direction = ParameterDirection.Output, Name = "P_DESCR_ERR", Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 4000)]
        public string Error { get; set; }
        
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_IP", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 20)]
        public string Ip { get; set; }
        
        [DBParameterDefinition(Direction = ParameterDirection.Input, Name = "P_USUARIO", BindOnNull = true, Type = OracleDbType.Varchar2, ValueConverter = typeof(RequestConvert<string>), Size = 30)]
        public string Usuario { get; set; }

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
