
namespace Isban.Maps.DataAccess.DBResponse
{
    using Isban.Common.Data;
    using Isban.Common.Data.Providers.IATX;

    [IATXCollectionDefinition(TableName = "Output")]
    public class DatosCuentaDbResp : IContract
    {
        [DBFieldDefinition(Name = "Ceros")]
        public string Codigo_retorno_extendido { get; set; }

        public string Clase_Paquete { get; set; }

        public string Tipo_Cuenta { get; set; }

        public string Sucursal_Cuenta { get; set; }

        public string Nro_Cuenta { get; set; }

        public string Nombre_Titular_Cuenta { get; set; }

        public string Tipo_Cobertura_Cuenta { get; set; }

        public string Tipo_Posicionamiento_Cuenta { get; set; }

        public string Saldo_Cuenta { get; set; }

        public string Saldo_impago { get; set; }

        public string Saldo_por_Conformar { get; set; }

        public string Depositos_Efectivo { get; set; }

        public string Depositos_24hs_CC { get; set; }

        public string Depositos_48hs_CC { get; set; }

        public string Depositos_72hs_CC { get; set; }

        public string Intereses_Ganados_CA { get; set; }

        public string Limite_Acuerdo_CC { get; set; }

        public string Cheques_Rechazados { get; set; }

        public string Fecha_Apertura { get; set; }

        public string Saldo_Cuenta_D { get; set; }

        public string Saldo_impago_D { get; set; }

        public string Saldo_por_Conformar_D { get; set; }

        public string Depositos_Efectivo_D { get; set; }

        public string Depositos_24hs_CC_D { get; set; }

        public string Depositos_48hs_CC_D { get; set; }

        public string Depositos_72hs_CC_D { get; set; }

        public string Intereses_Ganados_CA_D { get; set; }

        public string Limite_Acuerdo_CC_D { get; set; }

        public string Cheques_Rechazados_D { get; set; }

        public string Fecha_Apertura_D { get; set; }

        public string Indicador_direcciona { get; set; }

        public string Saldo_ACTE { get; set; }

        public string Saldo_ACAH { get; set; }

        public string Saldo_ACCD { get; set; }

        public string Saldo_ACAD { get; set; }

        public string Tope_USD_cuenta { get; set; }

        public string Tope_ARS_cuenta { get; set; }

        public string Acumulado_USD_cuenta { get; set; }

        public string Acumulado_ARS_cuenta { get; set; }

        public string Disponible_USD_cuenta { get; set; }

        public string Disponible_ARS_cuenta { get; set; }

        public string Disponible_USD_menor_cuenta_conjunto { get; set; }

        public string Disponible_ARS_menor_cuenta_conjunto { get; set; }

        public string Dispuesto_USD_cuenta { get; set; }

        public string Dispuesto_ARS_cuenta { get; set; }

        public string Fecha_Ultima_Operacion { get; set; }

        public string Ind_Sobregiro { get; set; }

        public string Estado { get; set; }
    }


    [IATXCollectionDefinition(TableName = "Output")]
    public class GeneracionCuentaDbResp : IContract
    {
        [DBFieldDefinition(Name = "Código_retorno_extendido")]
        public string Código_retorno_extendido { get; set; }

        //[DBFieldDefinition(Name = "NumeroCuenta")]
        //public string NumeroCuenta { get; set; }

        //[DBFieldDefinition(Name = "Cant_Desc_Status_Resultado")]
        //public string Cant_Desc_Status_Resultado { get; set; }

        //[DBFieldDefinition(Name = "Descripción_Status_Resultado")]
        //public string Descripción_Status_Resultado { get; set; }
        //[DBFieldDefinition(Name = "NumeroCuenta")]
        //public string NumeroCuenta { get; set; }

    }


    [IATXCollectionDefinition(TableName = "Output")]
    public class AltaCuentaDbResp : IContract
    {
        //[DBFieldDefinition(Name = "Código_retorno_extendido")]
        public string Cod_retorno { get; set; }

        //[DBFieldDefinition(Name = "Cod_retorno")]
        public string Nup { get; set; }

        //public string Nup { get; set; }


        //[DBFieldDefinition(Name = "Cant_Desc_Status_Resultado")]
        //public string Cant_Desc_Status_Resultado { get; set; }

        //[DBFieldDefinition(Name = "Descripción_Status_Resultado")]
        //public string Descripción_Status_Resultado { get; set; }
        //[DBFieldDefinition(Name = "NumeroCuenta")]
        //public string NumeroCuenta { get; set; }

    }
}
