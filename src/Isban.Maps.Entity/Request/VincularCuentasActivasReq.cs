using Isban.Maps.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Isban.Maps.Entity.Request
{
    [DataContract]
    public class VincularCuentasActivasReq : EntityBase

    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public long? CuentaTitulos { get; set; }

        [DataMember]
        public string Producto { get; set; }

        [DataMember]
        public string SubProducto { get; set; }
        [DataMember]
        public long Sucursal { get; set; }
        [DataMember]
        public long? TipoCuenta { get; set; }
        [DataMember]
        public string CodMoneda { get; set; }

        [DataMember]
        public string Alias { get; set; }


        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public long CuentaOperativa { get; set; }

    }

    [DataContract]
    public class InsertarCuentasVinculadasReq : EntityBase

    {
        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public long? CuentaTitulos { get; set; }

        [DataMember]
        public string Producto { get; set; }

        [DataMember]
        public string SubProducto { get; set; }
        [DataMember]
        public long Sucursal { get; set; }
        [DataMember]
        public long? TipoCuenta { get; set; }
        [DataMember]
        public string CodMoneda { get; set; }

        [DataMember]
        public string Moneda { get; set; }

        [DataMember]
        public string Alias { get; set; }


        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public long CuentaOperativa { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Apellido { get; set; }
        [DataMember]
        public string TipoDocumento { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }


        [DataMember]
        public string DescripcionDocumento { get; set; }

        [DataMember]
        public string TipoPersona { get; set; }

        [DataMember]
        public string CtaBloqueada { get; set; }

        [DataMember]
        public string ProductoOperativa { get; set; }

        [DataMember]
        public string SubProductoOperativa { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        public long? SucursalCtaDolares { get; set; }

        [DataMember]
        public long? TipoCuentaDolares { get; set; }

        [DataMember]
        public string CodMonedaDolares { get; set; }

        [DataMember]
        public long? CuentaOperativaDolares { get; set; }

        [DataMember]
        public long? CodAltaAdhesion { get; set; }

    }
}
