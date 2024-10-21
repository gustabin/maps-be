using Isban.Common.Data;
using Isban.Mercados.DataAccess.ConverterDBType;
using Isban.Mercados.DataAccess.OracleClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.DataAccess.DBResponse
{
    internal class ConsultaArchivoRTFDbResp : BaseResponse
    {

        //[DBFieldDefinition(Name = "FECHA_ALTA", ValueConverter = typeof(ResponseConvert<DateTime>))]
        //public DateTime CodMoneda { get; set; }

        [DBFieldDefinition(Name = "ID", ValueConverter = typeof(ResponseConvert<long>))]
        public long Id { get; set; }

        [DBFieldDefinition(Name = "NOMBRE_ARCH", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nombre { get; set; }

        [DBFieldDefinition(Name = "VISTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Visto { get; set; }

        [DBFieldDefinition(Name = "DESCRIPCION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Descripcion { get; set; }

        [DBFieldDefinition(Name = "CUENTA_TITULO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Cuenta { get; set; }

        [DBFieldDefinition(Name = "Path", ValueConverter = typeof(ResponseConvert<string>))]
        public string Path { get; set; }

        [DBFieldDefinition(Name = "Anio", ValueConverter = typeof(ResponseConvert<string>))]
        public string Anio { get; set; }
    }
}
