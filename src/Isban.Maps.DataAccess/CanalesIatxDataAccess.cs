
namespace Isban.Maps.DataAccess
{
    using DBResponse;
    using Entity.Request;
    using Entity.Response;
    using IDataAccess;
    using Isban.Common.Data;
    using Isban.Maps.Entity.Constantes.Estructuras;
    using Isban.Mercados.LogTrace;
    using Mercados;
    using System.Data;

    public class CanalesIatxDataAccess : ServiceProxy, ICanalesIatxDA
    {
        protected override string ProviderName => ConstantesIATX.Iatx;
        
        public virtual DatosCuentaIATXResponse ConsultaDatosCuenta(Cabecera cabecera, ClienteCuentaDDC cuenta, string nup)
        {
            ConsultaDatosCuentaRequest request = cabecera.MapperClass<ConsultaDatosCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Tipo_Cuenta = cuenta.TipoCta.ToString().PadLeft(2, '0');
            request.Sucursal_Cuenta = int.Parse(cuenta.SucursalCta).ToString().PadLeft(3, '0');
            request.Nro_Cuenta = long.Parse(cuenta.NroCta).ToString().PadLeft(7, '0');
            request.H_Nup = nup;

            DatosCuentaDbResp responseIatx = null;
            DatosCuentaIATXResponse result = null;

            responseIatx = this.Provider.GetFirst<DatosCuentaDbResp>(ConstantesIATX.CONSULTADATOSCUENTA, CommandType.Text, request);

            result = responseIatx.MapperClass<DatosCuentaIATXResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }

        public virtual GeneracionCuentaResponse GeneracionCuentaIATX(Cabecera cabecera, FormularioResponse formularioResponse, string sucursal, string producto, string subProducto)
        {
            GeneracionCuentaRequest request = cabecera.MapperClass<GeneracionCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Producto = "60";
            request.Sucursal = "0000";
            request.Subproducto = "0000";
            request.H_Nup = formularioResponse.Nup;

            GeneracionCuentaDbResp responseIatx = null;

            responseIatx = this.Provider.GetFirst<GeneracionCuentaDbResp>(ConstantesIATX.GeneracionCuenta, CommandType.Text, request);

            LoggingHelper.Instance.Information($"Llamado a servicio IATX: GENNROCTA correctamente, Response: {responseIatx.Código_retorno_extendido} ", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");

            var result = responseIatx.MapperClass<GeneracionCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }

        public virtual GeneracionCuentaResponse ConsultaCuentaIATX(Cabecera cabecera)
        {
            ConsultaCuentaRequest request = cabecera.MapperClass<ConsultaCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            //

            request.Nro_Cuenta = "000003506000";
            request.CajaAhorro = "00000000000000000000";
            request.CuentaC = "00000000000000000000";
            request.CuentaDolares = "00000000000000000000";
            request.Sucursal = "792";


            GeneracionCuentaDbResp responseIatx = null;
            //DatosCuentaIATXResponse result = null;

            responseIatx = this.Provider.GetFirst<GeneracionCuentaDbResp>(ConstantesIATX.ConsultaCuntaTitulo, CommandType.Text, request);

            LoggingHelper.Instance.Debug($"Llamado a servicio IATX: CNSCTATITU correctamente, Response: {responseIatx.Código_retorno_extendido}  ", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");

            var result = responseIatx.MapperClass<GeneracionCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }

        public virtual GeneracionCuentaResponse AltaCuentaIATX(Cabecera cabecera, FormularioResponse formularioResponse, string sucursal, string producto,string subproducto, string numeroCuenta)
        {
            AltaCuentaRequest request = cabecera.MapperClass<AltaCuentaRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Nro_Cuenta = numeroCuenta;
            request.Producto = "60";
            request.Sucursal = "0000";
            request.SubProducto = "0000";
            request.H_Nup = formularioResponse.Nup;
            request.Cónyuge_Indicador_cta_mancomunado = "N";
            request.Adhiere_paquete = "N";
            request.Tipo_intervención = "TI";       
            request.Firmante = "1"; 
            request.Código_moneda = "USD";
            request.Sucursal_operación = "0000";
            request.Canal_venta = "BM";
            request.Tipo_uso = "1"; 
            request.Cod_condicion = "1";
            request.Forma_Operar = "1"; 
            request.ADHIERE_RIOLINE_RIOSELF = "N";
            request.DOMICILIO_CORRESPONDENCIA = "P";
            request.SEC_DOM_LABORAL = "1"; 
            request.CajaAhorro = "0"; 
            request.CuentaCorriente = "0"; 


            AltaCuentaDbResp responseIatx = null;
            GeneracionCuentaResponse result = null;

            responseIatx = this.Provider.GetFirst<AltaCuentaDbResp>(ConstantesIATX.AltaCuenta, CommandType.Text, request);

            LoggingHelper.Instance.Debug($"Llamado a servicio IATX: AltaCuentaIATX correctamente, Response: {responseIatx.Cod_retorno} CodRetorno: {responseIatx.Nup}", "BusinessHelper", "CreacionCuentaTitulosRepatriacion");

            result = responseIatx.MapperClass<GeneracionCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }


        public virtual GeneracionCuentaResponse RelacionClienteContrato(Cabecera cabecera, FormularioResponse formularioResponse, string sucursal, string subproducto)
        {
            RelacionClienteContratoRequest request = cabecera.MapperClass<RelacionClienteContratoRequest>(TypeMapper.IgnoreCaseSensitive,
                ModeExcludeWord.All, new string[] { "H", "_" });

            request.Oficina_Contrato = "0000";
            request.Numero_Contrato = "p_CUENTA_BSR??";
            request.Motivo_Baja = "00";
            request.H_Nup = formularioResponse.Nup;
            request.Fecha_Baja = "00000000";
            request.Opción = "M";
            request.Datos_Cliente_1 = "v_Datos_Cliente??";
            request.Orden_Cliente_1 = "v_orden_cliente??";
            request.Responsabilidad_1 = "00000";
            request.Forma_Operar_1 = "04";
            request.Adhiere_Rioline_Rioself_1 = "1";
            request.Aplicacion = "";


            GeneracionCuentaDbResp responseIatx = null;
            GeneracionCuentaResponse result = null;

            responseIatx = this.Provider.GetFirst<GeneracionCuentaDbResp>(ConstantesIATX.RelacionClienteContrato, CommandType.Text, request);

            result = responseIatx.MapperClass<GeneracionCuentaResponse>(TypeMapper.IgnoreCaseSensitive);

            return result;
        }
    }
}
