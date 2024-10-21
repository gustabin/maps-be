
namespace Isban.Maps.Entity.Request
{  
    public class ConsultaDatosCuentaRequest : BaseIaxtRequest
    {
        public string Tipo_Cuenta { get; set; }

        public string Sucursal_Cuenta { get; set; }

        public string Nro_Cuenta { get; set; }
    }

    public class GeneracionCuentaRequest : BaseIaxtRequest
    {

        public string Sucursal { get; set; }


        public string Producto { get; set; }
        public string Subproducto { get; set; }
    }

    public class ConsultaCuentaRequest : BaseIaxtRequest
    {

        public string Nro_Cuenta { get; set; }
        public string Sucursal { get; set; }
        public string CuentaC { get; set; }
        public string CajaAhorro { get; set; }
        public string CuentaDolares { get; set; }

    }

    public class AltaCuentaRequest : BaseIaxtRequest
    {
        public string Cónyuge_Indicador_cta_mancomunado { get; set; }
        public string Adhiere_paquete { get; set; }
        public string Tipo_intervención { get; set; }
        public string Sucursal { get; set; }
        public string Producto { get; set; }
        public string SubProducto { get; set; }
        public string Nro_Cuenta { get; set; }
        public string Firmante { get; set; }
        public string Código_moneda { get; set; }
        public string Sucursal_operación { get; set; }
        public string Canal_venta { get; set; }
        public string Tipo_uso { get; set; }

        public string Cod_condicion { get; set; }
        
        
        public string Forma_Operar { get; set; }
        
        public string ADHIERE_RIOLINE_RIOSELF { get; set; }
        public string DOMICILIO_CORRESPONDENCIA { get; set; }
        public string SEC_DOM_LABORAL { get; set; }
        public string CuentaCorriente { get; set; }
        public string CajaAhorro { get; set; }
    }


    public class RelacionClienteContratoRequest : BaseIaxtRequest
    {
        public string Oficina_Contrato { get; set; }

        public string Aplicacion { get; set; }

        public string Numero_Contrato { get; set; }
        public string Codigo_Producto { get; set; }
        public string Codigo_Subproducto { get; set; }
        public string Motivo_Baja { get; set; }
        public string Fecha_Baja { get; set; }
        public string Opción { get; set; }
        public string Datos_Cliente_1 { get; set; }
        public string Orden_Cliente_1 { get; set; }
        public string Responsabilidad_1 { get; set; }
        public string Código_Condicion_1 { get; set; }
        public string Categoria_1 { get; set; }

        public string Forma_Operar_1 { get; set; }


        public string Adhiere_Rioline_Rioself_1 { get; set; }


    }
}
