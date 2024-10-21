using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Extensiones;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IBussiness;
using Isban.Maps.IDataAccess;
using Isban.Mercados;
using Isban.Mercados.Service.InOut;
using Isban.Mercados.UnityInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Isban.Maps.Business.Formularios.Factory
{
    public class ConfigFormularioConsulta : ICrearComponente
    {
        private static BindingFlags bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
        private static List<string> propExcluidas = new List<string> { "items", "nup", "segmento", "canal", "subcanal", "idsimulacion", "cabecera" };

        private FormularioResponse _entity;
        private FormularioResponse formulario;
        private DatoFirmaMaps _firma;
        private ValorConsDeAdhesionesResp[] adhesiones;
        private ValorConsDeAdhesionesResp adhesion;
        private long idDescripcionDinamica;
        private long idEstadoAdhesion;
        private long idVigencia;
        private long idPeriodos;
        private long idFechaDesde;
        private long idFechaHasta;
        private string tituloSAFAlta;
        private string tituloPDCAlta;
        private string tituloDefaultAlta;
        private string tituloSAFBaja;
        private string tituloPDCBaja;
        private string tituloDefaultBaja;
        private string ayuda;

        IMapsControlesDA daMapsControles = DependencyFactory.Resolve<IMapsControlesDA>();

        public ConfigFormularioConsulta(FormularioResponse formulario)
        {
            _entity = formulario;
        }

        public ConfigFormularioConsulta(FormularioResponse formulario, DatoFirmaMaps firma)
        {
            _entity = formulario;
            _firma = firma;
        }

        public void Crear()
        {
            _entity.Estado = TipoEstado.ConsultaAdhesion; // -----> ver si es necesario al finalizar el paso a wizard

            // 1 - Crear Formulario y setear valores
            CrearFormulario();
            
            // 2 - Obtener Ids Componentes Dinamicos
            ObtenerIdComponentes();

            // 3 - Obtener Titulos de formulario por servicio
            ObtenerTitulosDescripcion();

            // 4 - Obtener ayuda
            ObtenerAyuda();

            // 5 - Armado
            if (!_entity.IdAdhesion.Equals(null)) // -----> Viene por un unico formulario
            {
                // 6 - ObtenerAdhesion 
                ObtenerAdhesiones(false);
                // 7 - Armar adhesion
                ArmarAdhesion();
                // 8 - Agregar al formulario
                AgregarAFormularioConsulta();
            }
            else
            {
                // 6 - ObtenerAdhesiones
                ObtenerAdhesiones(true);
                // 7/8 - Armar/AgregarFormulario las adhesiones
                ArmarAdhesionesYAgregarAFormulario();
            }

            // 9 - Obtengo el perfil inversor actual
            PerfilDeInversor();
        }

        private void AgregarAFormularioConsulta()
        {
            if (char.ToUpper(adhesion.Estado) == 'A')
            {
                (_entity.Items.Where(c => (c as ControlSimple).Nombre.ToLower() == NombreComponente.ConsultaAdhesiones).FirstOrDefault() as ConsultaAdhesiones).Activas.Add(formulario);
            }
            else if (char.ToUpper(adhesion.Estado) == 'B')
            {
                (_entity.Items.Where(c => (c as ControlSimple).Nombre.ToLower() == NombreComponente.ConsultaAdhesiones).FirstOrDefault() as ConsultaAdhesiones).Inactivas.Add(formulario);
            }
        }

        private void ObtenerTitulosDescripcion()
        {
            tituloSAFAlta = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
            {
                NomParametro = Keys.ConsultaSAFAlta
            });

            tituloPDCAlta = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
            {
                NomParametro = Keys.ConsultaPDCAlta
            });

            tituloDefaultAlta = daMapsControles.ObtenerValorParametrizado(new ConsultaParametrizacionReq
            {
                NomParametro = Keys.ConsultaDefaultAlta
            });
            
            var req = new ConsultaParametrizacionReq()
            {
                CodigoSistema = "MAPS",
                NomParametro = "TituloFormBajaSAF"
            };
            tituloSAFBaja = daMapsControles.ObtenerValorParametrizado(req);

            req = new ConsultaParametrizacionReq()
            {
                CodigoSistema = "MAPS",
                NomParametro = "TituloFormBajaPDC"
            };
            tituloPDCBaja = daMapsControles.ObtenerValorParametrizado(req);

            req = new ConsultaParametrizacionReq()
            {
                CodigoSistema = "MAPS",
                NomParametro = "TituloFormDefault"
            };
            tituloDefaultBaja = daMapsControles.ObtenerValorParametrizado(req);
        }

        private void ObtenerAyuda()
        {
            ayuda = daMapsControles.ObtenerValorParametrizado(
                                        new ConsultaParametrizacionReq
                                        {
                                            NomParametro = Keys.AyudaMail
                                        });
        }

        private void ArmarAdhesion()
        {
            string Estado = (char.ToUpper(adhesion.Estado) == 'A') ? TipoEstado.Activo : TipoEstado.Inactivo;
            formulario = adhesion.TextoJson.DeserializarToJson<FormularioRequest>().ToFormularioMaps();
            switch (formulario.IdServicio.ToUpper())
            {
                case Servicio.SAF:
                    if (Estado == "A")
                    {
                        formulario.Titulo = tituloSAFAlta;
                    }
                    else if (Estado == "B")
                    {
                        formulario.Titulo = tituloSAFBaja;
                    }
                    break;
                case Servicio.PoderDeCompra:
                    if (Estado == "A")
                    {
                        formulario.Titulo = tituloPDCAlta;
                    }
                    else if (Estado == "B")
                    {
                        formulario.Titulo = tituloPDCBaja;
                    }
                    break;
                default:
                    if (Estado == "A")
                    {
                        formulario.Titulo = tituloDefaultAlta;
                    }
                    else if (Estado == "B")
                    {
                        formulario.Titulo = tituloDefaultBaja;
                    }
                    break;
            }

            if (BusinessHelper.ValidarExistencia(NombreComponente.Email, formulario))
            {
                formulario.Items
                .Where(x => string.Compare((x as ControlSimple).Nombre, NombreComponente.Email, true) == 0)
                .FirstOrDefault()
                .Ayuda = ayuda;
            }

            if (!BusinessHelper.ValidarExistencia(NombreComponente.DescripcionDinamica, formulario))
            {
                formulario.Items.Add(BusinessHelper.AgregarComponenteDescripcion(_entity, idDescripcionDinamica, formulario.IdServicio));
            }

            if (!BusinessHelper.ValidarExistencia(NombreComponente.EstadoAdhesion, formulario))
            {
                formulario.Items.Add(BusinessHelper.AgregarComponenteEstado(_entity, Estado, idEstadoAdhesion));
            }

            if (formulario.IdServicio.ToUpper() == Servicio.AgendamientoFH && !BusinessHelper.ValidarExistencia(NombreComponente.Vigencia, formulario))
            {
                formulario.Items.Add(BusinessHelper.AgregarComponenteVigencia(_entity, idVigencia,idPeriodos,idFechaDesde,idFechaHasta,_entity ));
            }

            formulario.Items = formulario.Items.OrderBy(x => (x as ControlSimple).Posicion).ToList();
        }

        private void ArmarAdhesionesYAgregarAFormulario()
        {
            _entity.Items.ForEach(c =>
            {
                var componente = c as ControlSimple;

                if (componente.Nombre.ToLower() == NombreComponente.ConsultaAdhesiones)
                {
                    var adh = c as ConsultaAdhesiones;

                    Array.ForEach(adhesiones, t =>
                    {
                        string Estado = (char.ToUpper(t.Estado) == 'A') ? TipoEstado.Activo : TipoEstado.Inactivo;
                        var fa = t.TextoJson.DeserializarToJson<FormularioRequest>().ToFormularioMaps();
                        switch (fa.IdServicio.ToUpper())
                        {
                            case Servicio.SAF:
                                if (char.ToUpper(t.Estado) == 'A')
                                {
                                    fa.Titulo = tituloSAFAlta;
                                }
                                else if (char.ToUpper(t.Estado) == 'B')
                                {
                                    fa.Titulo = tituloSAFBaja;
                                }
                                break;
                            case Servicio.PoderDeCompra:
                                if (char.ToUpper(t.Estado) == 'A')
                                {
                                    fa.Titulo = tituloPDCAlta;
                                }
                                else if (char.ToUpper(t.Estado) == 'B')
                                {
                                    fa.Titulo = tituloPDCBaja;
                                }
                                break;
                            default:
                                if (char.ToUpper(t.Estado) == 'A')
                                {
                                    fa.Titulo = tituloDefaultAlta;
                                }
                                else if (char.ToUpper(t.Estado) == 'B')
                                {
                                    fa.Titulo = tituloDefaultBaja;
                                }
                                break;
                        }
                        
                        if (BusinessHelper.ValidarExistencia(NombreComponente.Email, fa))
                        {
                            fa.Items
                            .Where(x => string.Compare((x as ControlSimple).Nombre, NombreComponente.Email, true) == 0)
                            .FirstOrDefault()
                            .Ayuda = ayuda;
                        }

                        if (!BusinessHelper.ValidarExistencia(NombreComponente.DescripcionDinamica, fa))
                        {
                            fa.Items.Add(BusinessHelper.AgregarComponenteDescripcion(_entity, idDescripcionDinamica, fa.IdServicio));
                        }

                        if (!BusinessHelper.ValidarExistencia(NombreComponente.EstadoAdhesion, fa))
                        {
                            fa.Items.Add(BusinessHelper.AgregarComponenteEstado(_entity, Estado, idEstadoAdhesion));
                        }

                        if (fa.IdServicio.ToUpper() == Servicio.AgendamientoFH && !BusinessHelper.ValidarExistencia(NombreComponente.Vigencia, formulario))
                        {
                            formulario.Items.Add(BusinessHelper.AgregarComponenteVigencia(_entity, idVigencia, idPeriodos, idFechaDesde, idFechaHasta,fa));
                        }

                        fa.Items = fa.Items.OrderBy(x => (x as ControlSimple).Posicion).ToList();

                        if (char.ToUpper(t.Estado) == 'A')
                        {
                            adh.Activas.Add(fa);
                        }
                        else if (char.ToUpper(t.Estado) == 'B')
                        {
                            adh.Inactivas.Add(fa);
                        }
                    });
                }
            });
        }

        private void ObtenerIdComponentes()
        {
            idDescripcionDinamica = daMapsControles.ObtenerIdComponente(NombreComponente.DescripcionDinamica, _entity.Usuario, _entity.Ip);
            idEstadoAdhesion = daMapsControles.ObtenerIdComponente(NombreComponente.EstadoAdhesion, _entity.Usuario, _entity.Ip);
            idVigencia =  daMapsControles.ObtenerIdComponente(NombreComponente.Vigencia, _entity.Usuario, _entity.Ip);
            idPeriodos =  daMapsControles.ObtenerIdComponente(NombreComponente.Periodos, _entity.Usuario, _entity.Ip);
            idFechaDesde =  daMapsControles.ObtenerIdComponente(NombreComponente.FechaDesde, _entity.Usuario, _entity.Ip);
            idFechaHasta =  daMapsControles.ObtenerIdComponente(NombreComponente.FechaHasta, _entity.Usuario, _entity.Ip);
        }

        private void ObtenerAdhesiones(bool isLista)
        {
            //Obtencion de cuentas operativas relacionadas al nup
            var cuentasOperativasLista = BusinessHelper.ObtenerCuentasPorTipo(_entity, "OP", _firma, false);
            var cuentasOperativas = BusinessHelper.ConcatenarCuentas(cuentasOperativasLista, true);

            //Obtencion de cuentas titulos relacionadas al nup
            var cuentasTitulosLista = BusinessHelper.ObtenerCuentasPorTipo(_entity, "TI", _firma, false);
            var cuentasTitulos = BusinessHelper.ConcatenarCuentas(cuentasTitulosLista, false);

            if (isLista)
            {
                adhesiones = daMapsControles.ObtenerConsultaDeAdhesiones(_entity, cuentasOperativas, cuentasTitulos);
            }
            else
            {
                adhesion = daMapsControles.ObtenerConsultaDeAdhesiones(_entity).FirstOrDefault();
            }
        }

        private void PerfilDeInversor()
        {
            //IServicesClient srvClient = DependencyFactory.Resolve<IServicesClient>();
            //_entity.PerfilInversor = srvClient.GetPerfil(_entity.Nup, _firma); //frm.PerfilInversor = srvClient.GetPerfil(frm.Nup, firma);



            var serviceWebApi = DependencyFactory.Resolve<IServiceWebApiClient>();

            var consultaPerfilReq = new ConsultaPerfilInversorRequest()
            {
                Nup = _entity.Nup,
                Encabezado = BusinessHelper.GenerarCabecera(_entity.Canal, _entity.SubCanal)
            };
            var reqSecurity = _firma.MapperClass<RequestSecurity<ConsultaPerfilInversorRequest>>(TypeMapper.IgnoreCaseSensitive);
            reqSecurity.Datos = consultaPerfilReq;
            _entity.PerfilInversor = serviceWebApi.ConsultaPerfilInversor(reqSecurity).Descripcion;
        }

        private void CrearFormulario()
        {
            var valores = daMapsControles.ObtenerConfigDeFormulario(_entity);

            if (valores != null && valores.Length > 0)
            {
                //recupera los atributos del formulario
                var frmAttr = valores.Where(x => !x.ControlPadreId.HasValue && !x.ControlAtributoPadreId.HasValue).ToArray();

                //recupera el id del padre (id de formulario)
                var frmPadreId = frmAttr.Select(x => x.IdComponente).FirstOrDefault();

                //recupera los ids de los diferentes controles del formulario                
                var listFrmCtrlID = valores.Where(x => x.ControlPadreId.HasValue)
                                            .Select(x => x.IdComponente)
                                            .Distinct();

                _entity.IdComponente = frmPadreId;

                #region Seteo propiedades del formulario
                for (int i = 0; i < frmAttr.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(frmAttr[i].AtributoDesc))
                    {
                        var propInfo = _entity.GetType().GetProperty(frmAttr[i].AtributoDesc, bindFlags);

                        if (propInfo != null && !propExcluidas.Contains(propInfo.Name.ToLower().Trim()))
                            propInfo.SetValue(_entity, frmAttr[i].AtributoValor);
                    }
                }
                #endregion

                foreach (decimal frmCtrlID in listFrmCtrlID)
                {
                    //recupera los atributos del control                    
                    var ctrlAtributosControl = valores.Where(x => x.IdComponente == frmCtrlID);

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
                                    List<string> ListaFechas = new List<string>(new string[] { NombreComponente.FechaHasta, NombreComponente.FechaDesde, NombreComponente.FechaHastaSafBP, NombreComponente.FechaDesdeSafBP, NombreComponente.FechaVigenciaPDC, NombreComponente.FechaAltaPdcAdhesion, NombreComponente.Fecha, NombreComponente.FechaSafBP, NombreComponente.FechaBaja });
                                    if (ListaFechas.Contains(atr.NombreComponente.Trim()))
                                    {
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
                                    }
                                    else
                                    {
                                        propInfo.SetValue(itemControl, atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType()), null);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new InvalidCastException($"Componente: {atr.NombreComponente}, atributo: {atr.AtributoDesc}: El valor {atr.AtributoValor} no se puede convertir a {atr.AtributoDataType}", ex);
                                }
                            }
                        }

                        //agregar control al listado de items del formulario
                        _entity.Items.Add(itemControl as ControlSimple);
                    }
                }
            }
        }
    }
}
