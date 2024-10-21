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
    internal class ChequeoAccesoResp : BaseResponse
    {
        [DBFieldDefinition(Name = "BASEDEDATOS", ValueConverter = typeof(ResponseConvert<string>))]
        public string BasedeDatos { get; set; }
        [DBFieldDefinition(Name = "USUARIODB", ValueConverter = typeof(ResponseConvert<string>))]
        public string UsuarioDB { get; set; }
        [DBFieldDefinition(Name = "SERVIDORDB", ValueConverter = typeof(ResponseConvert<string>))]
        public string ServidorDB { get; set; }
        [DBFieldDefinition(Name = "SERVIDORWIN", ValueConverter = typeof(ResponseConvert<string>))]
        public string ServidorWin { get; set; }
        [DBFieldDefinition(Name = "USUARIOWIN", ValueConverter = typeof(ResponseConvert<string>))]
        public string UsuarioWin { get; set; }
    }
}
