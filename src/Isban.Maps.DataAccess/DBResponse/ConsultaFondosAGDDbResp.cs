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
    internal class ConsultaFondosAGDDbResp : BaseResponse
    {

        [DBFieldDefinition(Name = "COD_FONDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string CodigoFondo { get; set; }

        [DBFieldDefinition(Name = "DESC_FONDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescripcionFondo { get; set; }

        [DBFieldDefinition(Name = "MONEDA_FONDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string MonedaFondo { get; set; }

        [DBFieldDefinition(Name = "RESCATE", ValueConverter = typeof(ResponseConvert<string>))]
        public string Rescate { get; set; }

        [DBFieldDefinition(Name = "SUSCRIPCION", ValueConverter = typeof(ResponseConvert<string>))]
        public string Suscripcion { get; set; }

        [DBFieldDefinition(Name = "DESC_FONDO_GRUPO", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescripcionGrupoFondo { get; set; }

        [DBFieldDefinition(Name = "ID_FGRUPO", ValueConverter = typeof(ResponseConvert<long>))]
        public long OrdenGrupoFondo { get; set; }

        [DBFieldDefinition(Name = "ORDEN_GRUPO", ValueConverter = typeof(ResponseConvert<long>))]
        public long OrdenFondo { get; set; }
    }
}
