namespace Isban.Maps.Entity.Constantes.Estructuras
{
    public struct TipoControl
    {
        public const string Servicio = "servicio";
        public const string InputText = "input-text";
        public const string Lista = "lista";
        public const string Legal = "legal";
        public const string InputNumber = "input-number";
        public const string FechaCompuesta = "fecha-compuesta";
        //public const string ImporteCompuesto = "importe-compuesto";
        public const string Fecha = "fecha";
        public const string Email = "email";
        public const string Fondo = "fondo";
        public const string Moneda = "moneda";
        public const string CuentaTitulo = "cuenta-titulo";
        public const string CuentaOperativa = "cuenta-operativa";
        public const string ConsultaAdhesiones = "consulta-adhesiones";
        public const string Disclaimer = "disclaimer";
        public const string Formulario = "formulario";
        public const string Operacion = "operacion";

    }

    public struct TipoPersona
    {
        public const string Fisica = "F";
        public const string Jurídica = "J";
    }

    public struct Servicio
    {
        public const string SAF = "SAF";
        public const string PoderDeCompra = "PDC";
        public const string Agendamiento = "AGD";
        public const string AgendamientoFH = "AGDFH";
        public const string DolarMEP = "DMEP";
        public const string DolarMEPGD30 = "DMEPG";
        public const string DolarMEPReverso = "VMEP";
        public const string DolarMEPRestringido = "VMEPR";
        public const string Rtf = "RTF";
        public const string Repatriacion = "CTR";
        public const string AltaCuenta = "ACT";

    }

    public struct TipoBono
    {
        public const string Local = "L";
        public const string Extranjero = "E";
    }

    public struct IdForm
    {
        public const string CargaDatosDMEP = "frm-vmep-001-1";
        public const string CargaDatosCMEP = "frm-dmep-001-1";
        public const string CargaDatosCMEPG = "frm-dmepg-001-1";
        public const string CargaDatosDMEPBP = "frm-vmep-003-1";
    }

    public struct Keys
    {
        public const string FechaDeSolicitud = "FECHA_DE_SOLICITUD";
        public const string AyudaFechaDeBaja = "AYUDA_FECHA_DE_BAJA";
        public const string Comprobante = "COMPROBANTE";
        public const string AyudaMail = "AYUDA_MAIL";
        public const string ConsultaSAFAlta = "DetalleConsultaSAF";
        public const string ConsultaPDCAlta = "DetalleConsultaPDC";
        public const string ConsultaDefaultAlta = "DetalleConsultaDefault";
        public const string ConsultaAGDAlta = "DetalleConsultaAGD";
        public const string DetalleConsultaRTF = "DetalleConsultaRTF";
        public const string TituloFormConfirmacionAGD = "TituloFormConfirmacionAGD";
        public const string TituloFormConfirmacionDefault = "TituloFormConfirmacionDefault";
        public const string TituloFormConfirmacionPDC = "TituloFormConfirmacionPDC";
        public const string TituloFormConfirmacionSAF = "TituloFormConfirmacionSAF";
        public const string TituloFormConfirmacionRTF = "TituloFormConfirmacionRTF";
        public const string TituloFormConfirmacionCTR = "TituloFormConfirmacionCTR";
        public const string TituloFormConfirmacionACT = "TituloFormConfirmacionACT";
        public const string TituloFormBajaRTF = "TituloFormBajaRTF";
        public const string CodigoSistemaMAPS = "MAPS";
        public const string PrecioMEP = "PRECIO_MEP_MAPS";
        public const string LimiteNominales = "LIMITE_MEP_MAPS";
    }

    public struct TipoEstadoFormulario
    {
        public const string Disclaimer = "disclaimer";
        public const string Carga = "carga";
        public const string Baja = "baja";
        public const string Consulta = "consulta";
        public const string Simulacion = "simulacion";
        public const string Confirmacion = "confirmacion";
    }

    public struct TipoEstado
    {
        public const string SimulacionBaja = "simulacion-baja";
        public const string ConsultaAdhesion = "consulta_adhesion";
        public const string AltaConsulta = "A";
        public const string Activo = "Activo";
        public const string Inactivo = "Inactivo";
        public const string CuentaBloqueada = "S";
        public const string Procesado = "PR";
        public const string BajaOrden = "BA";
    }

    public struct Segmento
    {
        public const string BancaPrivada = "bp";
        public const string Retail = "rtl";
    }

    public struct CuentaPDC
    {
        public const string CuentaNoApta = "N";
        public const string EstadoCuentaBaja = "B";
        public const string EstadoCuentaNoAdherido = "N";
        public const string SimularBaja = "BA";
        public const string ProcesarBaja = "CB";
        public const string Simular = "S";
        public const string Disclaimer = "D";
        public const string SimularAlta = "SA";
        public const string ProcesarAlta = "CA";
        public const string Procesar = "P";
        public const string Alta = "A";
        public const string Baja = "Inactivo";
    }

    public struct Monedas
    {
        public const string ARS = "ARS";
    }

    public struct Descripcion
    {
        public const string OtroIntervalo = "otro intervalo";
        public const string DetalleConsultaSAF = "Suscripción automática de fondos";
        public const string DetalleConsultaPDC = "Suscripción automática de Poder de Compra";
        public const string DetalleConsultaDefault = "Suscripción automática";
    }

    public struct NombreComponente
    {
        public const string Alias = "alias";
        public const string Fecha = "fecha";
        public const string Fondo = "fondo";
        public const string FechaCompuesta = "fecha-compuesta";
        public const string ImporteCompuesto = "importe-compuesto";
        public const string Servicio = "servicio";
        public const string InputNumber = "input-number";
        public const string DescripcionDinamica = "descripcion-dinamica";
        public const string Disclaimer = "disclaimer";
        public const string Email = "e-mail";
        public const string EstadoAdhesion = "estado-adhesion";
        public const string FechaAltaPdcAdhesion = "fecha-alta-pdc";
        public const string FechaVigenciaPdc = "fecha-vigencia-pdc";
        public const string FechaSafBP = "fecha-saf-bp";
        public const string FechaBaja = "fecha-baja";
        public const string Legal = "legal";
        public const string LegalAgendamiento = "legal-agendamiento";
        public const string ListaAdhesiones = "lista-adhesiones";
        public const string InputText = "input-text";
        public const string Comprobante = "comprobante";
        public const string CuentaOperativa = "cuenta-operativa";
        public const string CuentaOperativaPesos = "cuenta-operativa-1";
        public const string CuentaOperativaDolares = "cuenta-operativa-2";
        public const string CuentaTitulo = "cuenta-titulo";
        public const string Periodos = "periodos";
        public const string Moneda = "moneda";
        public const string MonedaAGDFH = "moneda-seleccionada-agdfh";
        public const string Producto = "producto";
        public const string ListaServicio = "lista-servicio";
        public const string MontoSuscripcion = "monto-suscripcion";
        public const string Operacion = "operacion";
        public const string SaldoMinimo = "saldo-minimo";
        public const string Vigencia = "vigencia";
        public const string FechaDesde = "fecha-desde";
        public const string FechaHasta = "fecha-hasta";
        public const string FechaDesdeSafBP = "fecha-desde-saf-bp";
        public const string FechaHastaSafBP = "fecha-hasta-saf-bp";
        public const string MontoSuscripcionMinimo = "monto-suscripcion-minimo";
        public const string MontoSuscripcionMaximo = "monto-suscripcion-maximo";
        public const string ListaMoneda = "lista-moneda";
        public const string ListaFondos = "lista-fondos";
        public const string ListadoFondos = "listado-fondos";
        public const string FechaVigenciaPDC = "fecha-vigencia-pdc";
        public const string ListadoGenerico = "lista-generica";
        public const string ListadoAsesoramiento = "lista-asesoramiento";
        public const string ListaPep = "lista-pep";
        public const string ConsultaAdhesiones = "consulta-adhesiones";
        public const string CuentasVinculadas = "cuentas-vinculadas";
        public const string Frecuencia = "frecuencia";
        public const string FechaSolBaja = "fecha-solicitiud-baja";
        public const string PrimerLegalPDC = "legal-pdc-001";
        public const string LegalBajaPDC = "legal-pdc-baja";
        public const string FondoCompuesto = "fondo-compuesto";
        public const string FechaEjecucion = "fecha-ejecucion-agd";
        public const string ListaFrecuencia = "lista-frecuencia";
        public const string ListaDias = "lista-dias-frecuencia";
        public const string Numeros = "numeros-frecuencia";
        public const string FechaFrecuencia = "fecha-frecuencia";
        public const string FondoAGDFH = "fondo-seleccionado-agdfh";
        public const string OperacionAGDFH = "operacion-seleccionada-agdfh";
        public const string SaldoMinimoMEP = "saldo-minimo-1";

    }

    public struct ConstantesIATX
    {
        public const string Iatx = "IATX";

        public const string ConsultaPdcNombreServicio = "CNSEXPPCPA";

        public const string CompraVtaAccionesBonosNombreServicio = "SIMCVTACBO";

        public const string CompraVtaSimulacionDirectaBonosNombreServicio = "CVTAACCBON";

        public const string DirectaCompraVtaAccionesNombreServicio = "CVTAEXTBUR";

        public const string ConsultaSuscripcionFondoComunInversion = "CNSSUSFCI";

        public const string SuscripcionFondoComunInversion = "SUSFCI";

        public const string CONSULTADATOSCUENTA = "CNSCTADATO";

        public const string RescateDeFondos = "RESFCIBCAP"; 

        public const string GeneracionCuenta = "GENNROCTA_100";

        public const string AltaCuenta = "ALTCTATITU100";

        public const string RelacionClienteContrato = "CMBRELCLIC140";

        public const string ConsultaCuntaTitulo = "CNSCTATITU";


        

    }

    public struct AccionWizard
    {
        public const string Siguiente = "siguiente";
        public const string Anterior = "anterior";
    }

    public struct Operaciones
    {
        public const string Suscripcion = "SUS";
        public const string Rescate = "RES";
        public const string OperSuscripcion = "Suscripción";
        public const string OperRescate = "Rescate";
    }

    public struct SiNo
    {
        public const string SI = "S";
        public const string NO = "N";
    }

    public struct TipoFrecuencia
    {
        public const string Semanal = "SEM";
        public const string FechaUnica = "FU";
        public const string MismoDiaCadaMes = "MDTM";
    }

    public struct Producto
    {
        public const string CuentaUnica = "07";
        public const string ProductoACT = "02";
    }

    public struct SubProducto
    {
        public const string CuentaAUH = "0009";
        public const string CuentaUniversal = "0016";
    }

}
