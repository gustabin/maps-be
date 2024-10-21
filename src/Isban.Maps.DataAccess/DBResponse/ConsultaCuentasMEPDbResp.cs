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
    internal class ConsultaCuentasMEPDbResp : BaseResponse
    {

        [DBFieldDefinition(Name = "CUENTA_TITULOS", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaTitulos { get; set; }

        [DBFieldDefinition(Name = "NRO_CTA_OPER", ValueConverter = typeof(ResponseConvert<long>))]
        public long CuentaOperativa { get; set; }

        [DBFieldDefinition(Name = "SUC_CTA_OPER", ValueConverter = typeof(ResponseConvert<int>))]
        public int Sucursal { get; set; }

        [DBFieldDefinition(Name = "TIPO_CTA_OPER", ValueConverter = typeof(ResponseConvert<int>))]
        public int TipoCuentaOperativa { get; set; }

        
    }
}
