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
    internal class ConsultaClienteDDCResp : BaseResponse
    {
        /// <summary>
        /// Atributo: TipoDocumento
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The tipo documento.
        /// </value>
        [DBFieldDefinition(Name = "TIPODOCUMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string TipoDocumento { get; set; }

        /// <summary>
        /// Atributo: TipoDocumento
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The descripcion documento.
        /// </value>
        [DBFieldDefinition(Name = "DESCRIPCIONDOCUMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string DescripcionDocumento { get; set; }

        /// <summary>
        /// Atributo: NumeroDocumento
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The numero documento.
        /// </value>
        [DBFieldDefinition(Name = "NUMERODOCUMENTO", ValueConverter = typeof(ResponseConvert<string>))]
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Atributo: Nombre
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        [DBFieldDefinition(Name = "NOMBRE", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nombre { get; set; }

        /// <summary>
        /// Atributo: Apellido
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The apellido.
        /// </value>
        [DBFieldDefinition(Name = "APELLIDO", ValueConverter = typeof(ResponseConvert<string>))]
        public string Apellido { get; set; }

        /// <summary>
        /// Atributo: Nup
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The nup.
        /// </value>
        [DBFieldDefinition(Name = "NUP", ValueConverter = typeof(ResponseConvert<string>))]
        public string Nup { get; set; }

        /// <summary>
        /// Atributo: Sucursal
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The sucursal.
        /// </value>
        [DBFieldDefinition(Name = "SUC_CTA", ValueConverter = typeof(ResponseConvert<string>))]
        public string Sucursal { get; set; }

        /// <summary>
        /// Atributo: FechaNacimiento
        /// Tipo de Dato: DateTime
        /// </summary>
        /// <value>
        /// The fecha nacimiento.
        /// </value>
        [DBFieldDefinition(Name = "FECHANACIMIENTO", ValueConverter = typeof(ResponseConvert<DateTime>))]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Atributo: CuentaTitulos
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The cuenta titulos.
        /// </value>
        [DBFieldDefinition(Name = "NRO_CTA", ValueConverter = typeof(ResponseConvert<string>))]
        public string CuentaTitulos { get; set; }

        /// <summary>
        /// Atributo: CuentaTitulos
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The cuenta titulos.
        /// </value>
        [DBFieldDefinition(Name = "NRO_CTA_OPER", ValueConverter = typeof(ResponseConvert<string>))]
        public string CuentaOperativa { get; set; }


        /// <summary>
        /// Atributo: CuentaTitulos
        /// Tipo de Dato: string
        /// </summary>
        /// <value>
        /// The cuenta titulos.
        /// </value>
        [DBFieldDefinition(Name = "PRODUCTO", ValueConverter = typeof(ResponseConvert<long>))]
        public long Producto { get; set; }
    }
}
