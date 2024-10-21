using System;
using Isban.Maps.Business.Factory;
using Isban.Maps.Business.Formularios.Factory;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados.UnityInject;
using System.Linq;
using Isban.Maps.Bussiness.Factory.Formularios;
using System.Collections.Generic;
using Isban.Mercados;
using System.Threading.Tasks;
using Isban.Maps.Entity.Request;
using Isban.Mercados.Service.InOut;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Bussiness.Factory;
using Isban.Maps.Entity.Controles;
using System.Configuration;
using Isban.Maps.Entity.Controles.Independientes;
using Isban.Mercados.LogTrace;
using Newtonsoft.Json;

namespace Isban.Maps.Bussiness.Wizard
{
    /// <summary>
    /// Esta clase debería pasar a reemplazar MapsServiciosBusiness
    /// </summary>
    public class WizardBusiness : IWizardBusiness
    {
        /// <summary>
        /// Genera el flujo para poder realizar un alta de una Adhesión.
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="entity"></param>
        /// <returns>Formulario a completar</returns>
        public virtual FormularioResponse AltaAdhesion(DatoFirmaMaps firma, FormularioResponse entity)
        {
            string _paso = "siguiente";
            FormularioResponse result = null;
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

            //TODO: Validar si es necesario llamar a esto que esta demorando un segundo casi.

            if (!ValidacionMEP(ref entity, ref result))
            {
                return result;
            }

            var wizard = new WizardMaps(entity, firma);

            var data = ValidacionMEP(ref entity, firma);

            daMapsControles.LogFormulario(entity);
            wizard.RegistrarPasoWizard();

            switch (_paso?.ToLower())
            {
                case AccionWizard.Siguiente:
                    result = wizard.SiguientePaso();
                    break;
                case AccionWizard.Anterior:
                    result = wizard.PasoAnterior();
                    break;
                default:
                    break;
            }

            if (string.Compare(entity.Estado, TipoEstadoFormulario.Confirmacion, true) == 0)
                wizard.RegistrarPasoWizard();

            FormularioHelper.SetearRelaciones(entity);
            FormularioHelper.DepurarControles(entity);
            ValidarEjecucionFeriadosAGD(entity, result, firma);
            PrecargaDatosMEP(entity, result, data);
            ValidacionDMEP(result, firma);
            daMapsControles.LogFormulario(entity);

            return result;
        }

        /// <summary>
        /// Realiza la baja de la adhesión
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual FormularioResponse BajaAdhesion(DatoFirmaMaps firma, FormularioResponse entity)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            FormularioResponse result = new FormularioResponse();

            daMapsControles.LogFormulario(entity);

            var wizard = new WizardMapsBaja(entity, firma);
            result = wizard.SiguientePaso();

            daMapsControles.LogFormulario(entity);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual FormularioResponse ConsultaAdhesion(DatoFirmaMaps firma, FormularioResponse entity)
        {
            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

            daMapsControles.LogFormulario(entity);

            var consulta = new EstrategiaComp(new ConfigFormularioConsulta(entity, firma));
            consulta.Crear();

            daMapsControles.LogFormulario(entity);
            return entity;
        }

        public FormularioResponse ObtenerListaDeServicios(FormularioResponse entity)
        {
            return FormularioHelper.ObtenerListaDeServicios(entity);
        }

        public List<FormularioResponse> ObtenerTodosLosPasos(MapsBase datos, DatoFirmaMaps firma)
        {

            IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
            var datosResp = daMapsControles.ObtenerTodosLosPasos(datos);
            var result = new List<FormularioResponse>();
            var forms = datosResp.GroupBy(x => new { x.FormularioId, x.BranchFrmId }).ToArray();

            Array.ForEach(forms, formDatos =>
            {
                EstrategiaComp fac = null;
                var form = datos.MapperClass<FormularioResponse>(TypeMapper.IgnoreCaseSensitive);

                switch (datos.IdServicio)
                {
                    case Servicio.SAF:
                        fac = new EstrategiaComp(new ConfigFormularioSAF(form, firma, formDatos.ToArray()));

                        break;
                    case Servicio.PoderDeCompra:
                        fac = new EstrategiaComp(new ConfigFormularioPDC(form, firma, formDatos.ToArray()));

                        break;

                    case Servicio.Agendamiento:
                        fac = new EstrategiaComp(new ConfigFomularioAGD(form, firma, formDatos.ToArray()));
                        break;

                    case Servicio.AgendamientoFH:
                        fac = new EstrategiaComp(new ConfigFomularioAGD(form, firma, formDatos.ToArray()));
                        break;
                }

                fac.Crear();
                result.Add(form);
            });

            return result;
        }

        /// <summary>
        /// Valida las restricciones del cliente.
        /// Si el cliente posee alguna restriccion, carga un formulario de restriccion con el mensaje correspondiente.
        /// </summary>
        /// <param name="_entitybase"></param>
        /// <param name="firma"></param>
        /// <returns></returns>
        private ValidaRestriccionMEPResponse ValidacionMEP(ref FormularioResponse _entitybase, DatoFirmaMaps firma) {
            if (_entitybase.IdServicio == Servicio.DolarMEPReverso || _entitybase.IdServicio == Servicio.DolarMEP || _entitybase.IdServicio == Servicio.DolarMEPGD30)
            {
                IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
                ValidaRestriccionMEPResponse data;
                #region Consulta restricciones
                
                if (_entitybase.IdServicio == Servicio.DolarMEP || _entitybase.IdServicio == Servicio.DolarMEPGD30)
                {
                    var req = firma.MapperClass<RequestSecurity<ValidaRestriccionCMEPRequest>>(TypeMapper.IgnoreCaseSensitive);
                    req.Datos = new ValidaRestriccionCMEPRequest()
                    {
                        Nup = _entitybase.Nup,
                        Segmento = _entitybase.Segmento,
                        TipoBono = _entitybase.IdServicio == Servicio.DolarMEP ? TipoBono.Local : TipoBono.Extranjero,
                        TipoControlPrestamosCheques = "3",
                        Encabezado = new Cabecera()
                        {
                            H_CanalTipo = "89",
                            H_SubCanalId = "HTML",
                            H_CanalVer = "000",
                            H_SubCanalTipo = "0000",
                            H_CanalId= "0001",
                            H_UsuarioTipo= "03",
                            H_UsuarioID= " ",
                            H_UsuarioAttr= "  ",
                            H_UsuarioPwd= " ",
                            H_IdusConc= " ",
                            H_NumSec= " ",
                            H_Nup= " ",
                            H_IndSincro= " ",
                            H_TipoCliente= " ",
                            H_TipoIDCliente= " ",
                            H_NroIDCliente= " ",
                            H_FechaNac= " "
                        }
                    };
                    req.Canal = "89";
                    req.SubCanal = "0000";
                    data = DependencyFactory.Resolve<IServiceWebApiClient>().ValidarRestriccionCMEP(req);
                } else
                {
                    var req = firma.MapperClass<RequestSecurity<ValidaRestriccionMEPRequest>>(TypeMapper.IgnoreCaseSensitive);
                    req.Datos = new ValidaRestriccionMEPRequest()
                    {
                        Ip = _entitybase.Ip,
                        Usuario = _entitybase.Usuario,
                        Nup = _entitybase.Nup,
                        Segmento = _entitybase.Segmento
                    };
                    data = DependencyFactory.Resolve<IServiceWebApiClient>().ValidarRestriccionMEP(req);
                }
                #endregion
                if (data != null)
                {
                    if (_entitybase.IdServicio == Servicio.DolarMEP || _entitybase.IdServicio == Servicio.DolarMEPGD30)
                    {
                        var req = firma.MapperClass<RequestSecurity<ValidaRestriccionMEPRequest>>(TypeMapper.IgnoreCaseSensitive);
                        req.Datos = new ValidaRestriccionMEPRequest()
                        {
                            Ip = _entitybase.Ip,
                            Usuario = _entitybase.Usuario,
                            Nup = _entitybase.Nup,
                            Segmento = _entitybase.Segmento
                        };
                        var unicaOp = daMapsControles.ObtenerRestriccionAdhesion(req.Datos);
                        data.CodigoOperacionSimultanea = unicaOp.Estado == null ? 0 : 2;
                        data.NotificacionOperacionSimultanea = unicaOp.Mensaje;
                        if (data.CodigoRestriccion == 0)
                        {
                            if (data.CodigoOperacionSimultanea != 0)
                            {
                                data.CodigoRestriccion = 2;
                                data.NotificacionRestriccion = data.NotificacionOperacionSimultanea;
                            }
                        }
                    }
                    if (data.CodigoRestriccion == 2)
                    {
                        _entitybase.IdServicio = Servicio.DolarMEPRestringido;
                        _entitybase.Segmento = "ALL";
                    }
                } else
                {
                    LoggingHelper.Instance.Error($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} | Error en servicio de validación de restricciones MEP", "WizardBusiness", "ValidacionMEP");
                    throw new BusinessException("Error en servicio de validación de restricciones MEP");
                }                

                return data;
            }
            return null;

        }

        /// <summary>
        ///  Validacion de limite para compra dolar mep (AL30)
        /// </summary>
        /// <param name="_entitybase"></param>
        /// <param name="firma"></param>
        private void ValidacionDMEP(FormularioResponse _entitybase, DatoFirmaMaps firma)
        {
            if ( _entitybase.IdServicio == Servicio.DolarMEP && _entitybase.Id == IdForm.CargaDatosCMEP)
            {
                IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();
                               

                // Seteo minimo DMEP
                var req = firma.MapperClass<RequestSecurity<ValidarCNV907Request>>(TypeMapper.IgnoreCaseSensitive);
                req.Datos = new ValidarCNV907Request()
                {
                    Ip = _entitybase.Ip,
                    Usuario = _entitybase.Usuario,
                    Nup = _entitybase.Nup
                };
                var cantidadNominales = DependencyFactory.Resolve<IServiceWebApiClient>().ValidarCNV907(req);
                var precioMEP = Convert.ToDecimal(daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq { NomParametro = Keys.PrecioMEP }));
                var limiteNominales = Convert.ToInt32(daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq { NomParametro = Keys.LimiteNominales }));
                var saldoMaximoNominales = (limiteNominales - cantidadNominales.CantidadNominales) * precioMEP;

                _entitybase.Items.ForEach(x =>
                {
                    if (x.Id == NombreComponente.SaldoMinimoMEP)
                    {
                        ((InputNumber<decimal?>)x).MaxValor = saldoMaximoNominales;
                    }

                });
                LoggingHelper.Instance.Information($"formulario validacion nominales DMEP: {JsonConvert.SerializeObject(_entitybase)}", "WizardBusiness", "ValidacionDMEP L225");

            }

        }


        /// <summary>
        /// Valida el valor del saldo máximo cargado.
        /// Si excede el saldo de la cuenta, devuelve el mismo formulario.
        /// </summary>
        /// <param name="_entitybase"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool ValidacionMEP(ref FormularioResponse _entitybase, ref FormularioResponse result)
        {
            bool valido = true;
            if (_entitybase.Id == IdForm.CargaDatosDMEP || _entitybase.Id == IdForm.CargaDatosDMEPBP || _entitybase.Id == IdForm.CargaDatosCMEP || _entitybase.Id == IdForm.CargaDatosCMEPG)
            {
                var cuentasOperativas = _entitybase.Items.Where(x => x.Id == NombreComponente.CuentaOperativaPesos).FirstOrDefault() as Lista<ItemCuentaOperativa<string>>;
                var ctaOperativa = cuentasOperativas.Items.Where(x => x.Seleccionado).FirstOrDefault();
                var saldoMinimo = _entitybase.Items.Where(x => x.Id == NombreComponente.SaldoMinimoMEP).FirstOrDefault() as InputNumber<decimal?>;
                if (ctaOperativa.SaldoCta == null || (saldoMinimo.Valor > ctaOperativa.SaldoCta))
                {
                    _entitybase.Items.ForEach(x =>
                    {
                        if (x.Id == NombreComponente.SaldoMinimoMEP)
                        {
                            ((InputNumber<decimal?>)x).Valor = null;
                            if (((InputNumber<decimal?>)x).Informacion != null)
                            {
                                if (!((InputNumber<decimal?>)x).Informacion.Contains("Recordá que el valor máximo"))
                                {
                                    ((InputNumber<decimal?>)x).Informacion = $"<br>{((InputNumber<decimal?>)x).Informacion}<br>" +
                                                                             $"Recordá que el valor máximo no puede superar el saldo de la cuenta operativa seleccionada.";
                                }
                            } else
                            {
                                ((InputNumber<decimal?>)x).Informacion = $"Recordá que el valor máximo no puede superar el saldo de la cuenta operativa seleccionada.";
                            }
                        }

                        if (x.Id.Contains(NombreComponente.CuentaOperativa) || x.Id.Contains(NombreComponente.Legal) || x.Id.Contains(NombreComponente.CuentaTitulo) || x.Id.Contains(NombreComponente.SaldoMinimo))
                        {
                            x.Validado = false;
                            x.Bloqueado = false;
                        }

                    });
                    result = _entitybase.ShallowCopy();
                    valido = false;
                }
            }

            return valido;
        }
        private void PrecargaDatosMEP(FormularioResponse _entitybase, FormularioResponse _entity, ValidaRestriccionMEPResponse data)
        {                      
            #region Filtro cuentas VMEP
            if (_entitybase.IdServicio == Servicio.DolarMEPReverso || _entitybase.IdServicio == Servicio.DolarMEP || _entitybase.IdServicio == Servicio.DolarMEPGD30)
            {
                var cuentaTit = _entity.Items.Where(x => x.Id == "cuenta-titulo-1").FirstOrDefault() as Lista<ItemCuentaTitulos<string>>;
                var cuentaOper = _entity.Items.Where(x => x.Id == "cuenta-operativa-1").FirstOrDefault() as Lista<ItemCuentaOperativa<string>>;
                if (cuentaTit != null && cuentaOper != null)
                {
                    _entity.Items.ForEach(control =>
                    {

                        if (control.Id == "cuenta-operativa-1")
                        {
                            ((Lista<ItemCuentaOperativa<string>>)control).Items.RemoveAll(item => item.Producto != Producto.CuentaUnica);
                            
                        }

                        if (control.Id == "cuenta-titulo-1" && data.ListaCuentas.Count > 0)
                        {
                            ((Lista<ItemCuentaTitulos<string>>)control).Items.RemoveAll(x => !data.ListaCuentas.Contains(x.Valor));
                        }
                    });
                }
            }
            #endregion
            #region Filtro Cuentas ACT
            if (_entitybase.IdServicio == Servicio.AltaCuenta)
            {
                var cuentaOper = _entity.Items.Where(x => x.Id == "cuenta-operativa-1").FirstOrDefault() as Lista<ItemCuentaOperativa<string>>;
                _entity.Items.ForEach(control =>
                {

                    if (control.Id == "cuenta-operativa-1")
                    {
                        ((Lista<ItemCuentaOperativa<string>>)control).Items.RemoveAll(item => (item.Producto == Producto.ProductoACT && item.SubProducto == SubProducto.CuentaAUH  ) || (item.Producto == Producto.ProductoACT && item.SubProducto == SubProducto.CuentaUniversal ));
                        
                    }                       
                });
                
            }
            #endregion
            #region Restriccion VMEP
            if (_entitybase.IdServicio == Servicio.DolarMEPRestringido)
            {
                _entity.Items.ForEach(control =>
                {

                    if (control.Id == "legal-1")
                    {
                        if (data != null)
                        {
                            ((Lista<ItemLegal<string>>)control).Items.Add(new ItemLegal<string>()
                            {
                                Desc = "Información de Restricción",
                                Valor = data.NotificacionRestriccion
                            });

                        }

                    }
                });
            }
            #endregion
        }

        private void ValidarEjecucionFeriadosAGD(FormularioResponse _entitybase, FormularioResponse _entity, DatoFirmaMaps _firma)
        {
            if (_entitybase.IdServicio == Servicio.Agendamiento || _entitybase.IdServicio == Servicio.AgendamientoFH)
            {
                try
                {

                    var fechaEjecucion = _entity.Items.Where(x => x.Id == "fecha-2").FirstOrDefault() as Fecha;
                    if (fechaEjecucion != null)
                    {
                        var valor = fechaEjecucion?.FechaMin;

                        var reqCtas = _entity.MapperClass<DiasNoHabilesRequest>(TypeMapper.IgnoreCaseSensitive);
                        reqCtas.FiltroPais = "AR";

                        var reqSecurityGetCuenta = _firma.MapperClass<RequestSecurity<DiasNoHabilesRequest>>(TypeMapper.IgnoreCaseSensitive);

                        reqSecurityGetCuenta.Datos = reqCtas;
                        reqSecurityGetCuenta.Canal = "04";
                        reqSecurityGetCuenta.SubCanal = "0099";

                        var dias = DependencyFactory.Resolve<IServiceWebApiClient>().ConsultaDiasNoHabiles(reqSecurityGetCuenta);
                        if (dias != null && dias.ListaFeriados.Count() > 0 && valor != null)
                        {
                            foreach (var dia in dias.ListaFeriados)
                            {

                                var feriado = Convert.ToDateTime(dia.Fecha);

                                if (feriado.Date == valor.GetValueOrDefault().Date)
                                {
                                    valor = valor.GetValueOrDefault().AddBusinessDays(1);
                                }

                            }
                        }
                        _entity.Items.ForEach(x =>
                        {
                            if (x.Id == "fecha-2")
                            {
                                ((Fecha)x).FechaMin = (DateTime)valor;
                            }
                        });

                    }

                }
                catch (Exception)
                {


                }
            }
        }

    }
}
