
namespace Isban.Maps.Entity.Response
{
    using Constantes.Enumeradores;
    using Constantes.Estructuras;
    using Controles;
    using Controles.Compuestos;
    using Extensiones;
    using Interfaces;
    using Isban.Maps.Entity.Base;
    using Isban.Mercados.LogTrace;
    using Mercados;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    [DataContract]
    public class FormularioResponse : ControlSimple, IFormulario
    {
        private static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        private static List<string> propExcluidas = new List<string> { "items", "nup", "segmento", "canal", "subcanal", "idsimulacion", "cabecera" };
     
        public FormularioResponse()
        {
            Items = new List<ControlSimple>();
        }          

        public FormularioResponse ShallowCopy()
        {             
            var form =(FormularioResponse)this.MemberwiseClone() as FormularioResponse;
            form.Items = new List<ControlSimple>(Items);

            return form;
        }
        [DataMember]
        [JsonIgnore]
        public string CodigoDeFondo { get; set; }

        [DataMember]
        [JsonIgnore]
        public string CuentaTitulos { get; set; }

        [DataMember]
        [JsonIgnore]
        public string CuentaOperativa { get; set; }



        [DataMember]
        public string IdServicio { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? IdSimulacion { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Comprobante { get; set; }

        [DataMember]
        public string Estado { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? FormAnterior { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? FormSiguiente { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? IdAdhesion { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Titulo { get; set; }

        [DataMember]
        public string Nup { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string Canal { get; set; }

        [DataMember]
        public string SubCanal { get; set; }

        [DataMember]
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PerfilInversor { get; set; }

        [DataMember]
        public List<ControlSimple> Items { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TextoJson { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Usuario { get; set; }

        [DataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Ip { get; set; }

        [IgnoreDataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? FormularioId { get; set; }

        [IgnoreDataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SessionId { get; set; }        

        [IgnoreDataMember]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MonedaFondo { get; set; }

        [DataMember]
        [JsonIgnore]
        public string ListaCtasOperativas { get; set; }

        [DataMember]
        [JsonIgnore]
        public string ListaCtasRepatriacion { get; set; }

        [DataMember]
        [JsonIgnore]
        public string ListaCtasTitulo { get; set; }

        [DataMember]
        [JsonIgnore]
        public string ListaCtasPDC { get; set; }

        [DataMember]
        [JsonIgnore]
        public DateTime? FechaEfectiva { get; set; }

        [DataMember]
        [JsonIgnore]
        public long? SimOrdenOrigen { get; set; }

        [DataMember]
        [JsonIgnore]
        public long? OrdenOrigen { get; set; }

        [DataMember]
        [JsonIgnore]
        public string ServidorWin { get; set; }

        [DataMember]
        [JsonIgnore]
        public DateTime? FechaDeEjecucion { get; set; }
        public string Operacion { get; set; }

        public override bool Validar(string idServicio = null, string idFormulario = null)
        {
            base.Validar();

            #region formulario

            EsVacio("Nup", Nup);
            ValidarLargoRango("Nup", Nup, 8);

            if (!EsNumerico(Nup))
            {
                TieneErrores = true;
                Error_tecnico = $"El valor ingresado para NUP es incorrecto. Valor actual {Nup}";
            }

            EsVacio("Segmento", Segmento);
            ValidarLargo("Segmento", Segmento, 3);

            EsVacio("Estado", Estado);
            ValidarContenido("Estado", Estado, TipoEstadoFormulario.Baja, TipoEstadoFormulario.Carga, TipoEstadoFormulario.Confirmacion, TipoEstadoFormulario.Consulta, TipoEstadoFormulario.Disclaimer, TipoEstadoFormulario.Simulacion);
            #endregion

            #region Validar controles
            if (Items != null)
            {
                foreach (var item in Items)
                {
                    if (FormularioId == 4) // ----> Este atributo contiene la info del siguiente formulario, en caso de ser 4, es FONDO.
                    {
                        if (!item.Validar(IdServicio, null)) //Validar los datos que se deben enviar para formulario fondos
                            TieneErrores = true;
                    }
                    else
                    {
                        if (!item.Validar(IdServicio, FormularioId?.ToString()))
                        {

                            TieneErrores = true;
                        }
                    }
                }
            }
            #endregion

            if (TieneErrores)
            {
                Error = (int)TiposDeError.ErrorValidacion;
                Error_desc = Errores;
                LoggingHelper.Instance.Error($"El formulario presenta los siguientes errores de validacion: {Errores}");
            }

            return !TieneErrores;
        }

        public void AsignarDatosBackend(ValorCtrlResponse[] entity, string servicioID = null)
        {
            ConstruirFormularioControlInput(entity, servicioID);
        }

        private void ConstruirFormularioControlInput(ValorCtrlResponse[] list, string servicioID = null)
        {
            var form = this;

            if (list != null && list.Length > 0)
            {
                //recupera los atributos del formulario
                var frmAttr = list.Where(x => !x.ControlPadreId.HasValue && !x.ControlAtributoPadreId.HasValue).ToArray();

                //recupera el id del padre (id de formulario)
                var frmPadreId = frmAttr.Select(x => x.IdComponente).FirstOrDefault();

                //recupera los ids de los diferentes controles del formulario                
                var listFrmCtrlID = list.Where(x => x.ControlPadreId.HasValue)
                                            .Select(x => x.IdComponente)
                                            .Distinct();

                form.IdComponente = frmPadreId;

                for (int i = 0; i < frmAttr.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(frmAttr[i].AtributoDesc))
                    {
                        var propInfo = form.GetType().GetProperty(frmAttr[i].AtributoDesc, bindFlags);

                        if (propInfo != null && !propExcluidas.Contains(propInfo.Name.ToLower().Trim()))
                        {

                            if (string.Compare(frmAttr[i].AtributoDesc, "Ayuda", true) == 0 && !string.IsNullOrWhiteSpace(servicioID)) ////&& servicioID.ToUpper() == Servicio.PoderDeCompra)
                                propInfo.SetValue(form, frmAttr[i].ValorAtributoCompExtendido);
                            else
                                propInfo.SetValue(form, frmAttr[i].AtributoValor);
                        }
                    }
                }

                foreach (decimal frmCtrlID in listFrmCtrlID)
                {
                    //recupera los atributos del control                    
                    var ctrlAtributosControl = list.Where(x => x.IdComponente == frmCtrlID);

                    Type ctrlTipo = null;
                    Type itemGenericType = null;
                    bool tineValor = ctrlAtributosControl.Any(x => x.AtributoDesc.ToLower().Equals("valor"));

                    if (tineValor)
                    {
                        itemGenericType = ctrlAtributosControl.Where(x => x.AtributoDesc.ToLower().Equals("valor")).Select(y => y.AtributoDataType).FirstOrDefault().ToType();
                    }
                    else
                    {
                        itemGenericType = ctrlAtributosControl.Where(x => x.AtributoDesc.ToLower().Equals("tipo")).Select(y => y.AtributoDataType).FirstOrDefault().ToType();
                    }

                    ctrlTipo = ctrlAtributosControl.Where(x => x.AtributoDesc.ToLower().Equals("nombre")).FirstOrDefault().AtributoValor.ToControlMaps(itemGenericType);

                    if (ctrlTipo != null)
                    {
                        var itemControl = Activator.CreateInstance(ctrlTipo);

                        ((ControlSimple)itemControl).IdComponente = ctrlAtributosControl.First().IdComponente;
                        ((ControlSimple)itemControl).IdPadreDB = ctrlAtributosControl.First().ControlPadreId;

                        foreach (var atr in ctrlAtributosControl.ToList())
                        {
                            var propInfo = itemControl.GetType().GetProperty(atr.AtributoDesc, bindFlags);
                            if (propInfo != null && atr.AtributoValor != null)
                            {
                                try
                                {
                                    //List<string> ListaFechas = new List<string>(new string[] { NombreComponente.FechaHasta, NombreComponente.FechaDesde, NombreComponente.Fecha, NombreComponente.FechaBaja });  /*NombreComponente.FechaHastaSafBP, NombreComponente.FechaDesdeSafBP, NombreComponente.FechaSafBP, NombreComponente.FechaVigenciaPDC, NombreComponente.FechaAltaPdcAdhesion, "fecha-desde-pdc" */
                                    /*List<string> ListaFechas = new List<string>(new string[] { NombreComponente.FechaHasta, NombreComponente.FechaDesde, NombreComponente.Fecha, NombreComponente.FechaBaja});
                                    if (ListaFechas.Contains(atr.NombreComponente.Trim()))
                                    {*/
                                    if (atr.AtributoValor.ToLower().Equals("today"))
                                    {
                                        propInfo.SetValue(itemControl, DateTime.Now);
                                    }
                                    else if (atr.AtributoValor.ToLower().Equals("tomorrow"))
                                    {
                                        propInfo.SetValue(itemControl, DateTime.Now.AddDays(1D));
                                    }
                                    else
                                        propInfo.SetValue(itemControl, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);
                                    /*}
                                    else
                                    {
                                        propInfo.SetValue(itemControl, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);
                                    }*/
                                }
                                catch (Exception ex)
                                {
                                    throw new InvalidCastException($"Componente: {atr.NombreComponente}, atributo: {atr.AtributoDesc}: El valor {atr.AtributoValor} no se puede convertir a {atr.AtributoDataType}", ex);
                                }
                            }
                        }

                        //agregar control al listado de items del formulario
                        form.Items.Add(itemControl as ControlSimple);
                    }
                }
            }
        }

        public void AsignarControlHijoAControlPadre(ControlSimple itemPadre, ControlSimple item)
        {
            try
            {
                var propInfo = itemPadre.GetType().GetProperty("items", bindFlags);
                var propList = propInfo.GetValue(itemPadre, null) as IList;
                var pi = propInfo.GetValue(itemPadre, null);

                propInfo.PropertyType.GetMethod("add", bindFlags).Invoke(pi, new[] { item });

            }
            catch (Exception ex)
            {

                throw new BusinessException($"Error en relación de controles.", ex);
            }
        }


    }
}
