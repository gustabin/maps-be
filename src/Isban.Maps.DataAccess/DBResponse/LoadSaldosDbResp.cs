namespace Isban.Maps.DataAccess.DBResponse
{
    using Common.Data;
    using Mercados.DataAccess.ConverterDBType;
    using Mercados.DataAccess.OracleClient;
    using System;

    internal class LoadSaldosDataAccessResponse : BaseResponse
    {
        [DBFieldDefinition(Name = "Cuenta", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? Cuenta { get; set; }

        [DBFieldDefinition(Name = "Descripcion", ValueConverter = typeof(ResponseConvert<string>))]
        public string Descripcion { get; set; }

        [DBFieldDefinition(Name = "Sucursal", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? Sucursal { get; set; }

        [DBFieldDefinition(Name = "Asesor", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal? Asesor { get; set; }

        [DBFieldDefinition(Name = "Fecha", ValueConverter = typeof(ResponseConvert<DateTime?>))]
        public DateTime Fecha { get; set; }

        [DBFieldDefinition(Name = "C.Ahorro $", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal CAhorroPesos { get; set; }

        [DBFieldDefinition(Name = "C.Ahorro U$S", ValueConverter = typeof(ResponseConvert<decimal?>))]
        public decimal CAhorroDolares { get; set; }
    }
}