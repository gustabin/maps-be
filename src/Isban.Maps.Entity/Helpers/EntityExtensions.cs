
namespace Isban.Maps.Entity.Extensiones
{
    using Constantes.Estructuras;
    using Controles;
    using Controles.Compuestos;
    using Controles.Customizados;
    using Controles.Independientes;
    using Mercados;
    using Newtonsoft.Json.Linq;
    using Request;
    using Response;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public static class EntityExtensions
    {
        public static int? GetDecimalPart<T>(this T obj)
        {
            int? result = null;
            char[] separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray();

            if (obj != null && obj.ToString().Split(separator[0]).Count() > 1)
            {

                int frac = 0;

                if (int.TryParse(obj.ToString().Split(separator[0])[1], out frac))
                {
                    result = frac;
                }
                else
                {
                    result = null;
                }
            }

            return result;
        }

        public static int GetCountDecimalDigits(this decimal obj)
        {
            int count = BitConverter.GetBytes(decimal.GetBits(obj)[3])[2];

            return count;
        }

        public static T GetControlMaps<T>(this List<ControlSimple> ctrlM, string nombre)
        {
            T result = default(T);

            var a = ctrlM.Where(x => x.Nombre.ToLower().Equals(nombre));

            if (a.Count() == 1)
            {
                var b = a.Cast<T>();
                result = b.FirstOrDefault();
            }
            else
            {
                foreach (var ctrl in ctrlM)
                {
                    var ctrlComp = ctrl as CuentaCompuesta;

                    if (ctrlComp != null)
                    {
                        foreach (var itemComp in ctrlComp.Items)
                        {
                            if (string.Compare(itemComp.Nombre, nombre, true) == 0)
                            {
                                result = (T)Convert.ChangeType(itemComp, typeof(T));
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static FormularioResponse ToFormularioMaps(this FormularioRequest obj)
        {
            var bindFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

            if (obj.Items == null)
                obj.Items = new List<JObject>();

            var form = obj.MapperClass<FormularioResponse>(TypeMapper.IgnoreCaseSensitive);

            if(!string.IsNullOrEmpty(obj.CodFondo)) form.CodigoDeFondo = obj.CodFondo;
            if (!string.IsNullOrEmpty(obj.Moneda)) form.MonedaFondo = obj.Moneda;
            if (!string.IsNullOrEmpty(obj.CuentaTitulos)) form.CuentaTitulos = obj.CuentaTitulos;
            if (!string.IsNullOrEmpty(obj.CuentaOperativa)) form.CuentaOperativa = obj.CuentaOperativa;

            if (obj != null)
            {
                var items = obj.Items.ToArray();
                try
                {
                

                for (int i = 0; i < items.Length; i++)
                {
                    switch (items[i].Property("Nombre").Value.ToString().ToLower())
                    {
                        case NombreComponente.Frecuencia:
                            form.Items.Add(items[i].ToObject<Lista<Item<string>>>());
                            break;
                        case NombreComponente.Moneda: //"moneda"
                            form.Items.Add(items[i].ToObject<Lista<ItemMoneda<string>>>());
                            break;

                        case NombreComponente.Operacion: //"operacion"
                            form.Items.Add(items[i].ToObject<Lista<Item<string>>>());
                            break;

                        case NombreComponente.CuentaOperativa: //"cuenta-operativa"
                            form.Items.Add(items[i].ToObject<Lista<ItemCuentaOperativa<string>>>());
                            break;

                        case NombreComponente.CuentaTitulo: //"cuenta-titulo"
                            form.Items.Add(items[i].ToObject<Lista<ItemCuentaTitulos<string>>>());
                            break;

                        case NombreComponente.Periodos: //"periodos"
                            form.Items.Add(items[i].ToObject<Lista<Item<string>>>());
                            break;

                        case NombreComponente.Disclaimer: //"disclaimer"
                            form.Items.Add(items[i].ToObject<Lista<ItemDisclaimer<string>>>());
                            break;

                        case NombreComponente.DescripcionDinamica: //"descripcion-dinamica"
                            form.Items.Add(items[i].ToObject<DescripcionDinamica<string>>());
                            break;

                        case NombreComponente.EstadoAdhesion: //"estado-adhesion"
                            form.Items.Add(items[i].ToObject<EstadoAdhesion<string>>());
                            break;

                        case NombreComponente.Email:
                        case NombreComponente.Alias:
                        case NombreComponente.InputText:
                            form.Items.Add(items[i].ToObject<InputText<string>>());
                            break;
                        case NombreComponente.Comprobante:
                        case NombreComponente.SaldoMinimo:
                        case NombreComponente.InputNumber: //"input-number"
                            //decimal num = 0;
                            //string valor = items[i].Property("Valor").Value.ToString();
                            //string incremento = items[i].Property("Incremento").Value.ToString();
                            //string errorDesc = "Valor numérico incorrecto. Ingrese un número válido y use como separador decimal el punto (.)";

                            //if (valor.IndexOf(",") > -1 || incremento.IndexOf(",") > -1 || !decimal.TryParse(valor, out num))
                            //{
                            //    //no se debe permitir numeros con , (coma)
                            //    if (items[i].ContainsKey("Error"))
                            //        items[i].Property("Error").Value = "1";
                            //    else
                            //        items[i].Add("Error", "1");

                            //    if (items[i].ContainsKey("Error_desc"))
                            //        items[i].Property("Error_desc").Value = errorDesc;
                            //    else
                            //        items[i].Add("Error_desc", errorDesc);

                            //    if (items[i].ContainsKey("Error_tecnico"))
                            //        items[i].Property("Error_tecnico").Value = errorDesc;
                            //    else
                            //        items[i].Add("Error_tecnico", errorDesc);

                            //    form.Items.Add(items[i].ToObject<InputNumber<string>>());
                            //}
                            //else
                            form.Items.Add(items[i].ToObject<InputNumber<decimal?>>());

                            break;

                        case NombreComponente.Servicio: //"servicio"
                            form.Items.Add(items[i].ToObject<Lista<ItemServicio<string>>>());
                            break;
                        case NombreComponente.MontoSuscripcion:
                        case NombreComponente.ImporteCompuesto: //"importe-compuesto"://monto-suscripcion
                            var objImpCompuesto = Activator.CreateInstance<ImporteCompuesto>();

                            foreach (var item in items[i].Properties())
                            {
                                var ctrlPropInfo = objImpCompuesto.GetType().GetProperty(item.Name, bindFlags);

                                if (ctrlPropInfo != null && !item.Name.ToLower().Equals("items"))
                                {
                                    ctrlPropInfo.SetValue(objImpCompuesto, item.Value.ParseGenericVal(item.Value.Type.ToString().ToType()));
                                }
                                else if (ctrlPropInfo != null && item.Name.ToLower().Equals("items"))
                                {
                                    foreach (var control in item.Value.AsEnumerable())
                                    {
                                        objImpCompuesto.Items.Add(control.ToObject<InputNumber<decimal?>>());
                                    }
                                }
                            }

                            form.Items.Add(objImpCompuesto);
                            break;
                        case NombreComponente.Vigencia:
                        case NombreComponente.FechaCompuesta:

                            var fecCompuesta = Activator.CreateInstance<FechaCompuesta>();

                            foreach (var item in items[i].Properties())
                            {
                                var ctrlPropInfo = fecCompuesta.GetType().GetProperty(item.Name, bindFlags);

                                if (ctrlPropInfo != null && !item.Name.ToLower().Equals("items"))
                                {
                                    //var val = atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType());
                                    ctrlPropInfo.SetValue(fecCompuesta, item.Value.ParseGenericVal(item.Value.Type.ToString().ToType()));
                                }
                                else if (ctrlPropInfo != null && item.Name.ToLower().Equals("items"))
                                {
                                    var controles = item.Value.ToList();

                                    foreach (var control in controles)
                                    {
                                        var nombre = control.Value<string>("Tipo");

                                        switch (nombre)
                                        {
                                            case TipoControl.Fecha:
                                                var objFecha = control.ToObject<Fecha>();
                                                fecCompuesta.Items.Add(objFecha);
                                                break;
                                            case TipoControl.Lista:
                                                var objLista = control.ToObject<Lista<Item<string>>>();
                                                fecCompuesta.Items.Add(objLista);
                                                break;
                                        }
                                    }
                                }
                            }

                            form.Items.Add(fecCompuesta);
                            break;
                        case NombreComponente.FechaEjecucion:
                        case NombreComponente.Fecha: //"fecha"
                            form.Items.Add(items[i].ToObject<Fecha>());
                            break;
                        case NombreComponente.LegalAgendamiento:
                        case NombreComponente.Legal: //"legal"
                            form.Items.Add(items[i].ToObject<Lista<ItemLegal<string>>>());
                            break;

                        case NombreComponente.ListaFondos:
                        case NombreComponente.Fondo: //"fondo"
                            form.Items.Add(items[i].ToObject<Lista<ItemGrupoAgd>>());
                            break;
                           
                            case NombreComponente.ListadoFondos:
                            case NombreComponente.ListadoAsesoramiento:
                            case NombreComponente.ListaPep:
                            case NombreComponente.Producto:
                            form.Items.Add(items[i].ToObject<Lista<Item<string>>>());
                            break;

                        case NombreComponente.ConsultaAdhesiones:
                            form.Items.Add(items[i].ToObject<ConsultaAdhesiones>());
                            break;
                        case NombreComponente.ListadoGenerico:
                            form.Items.Add(items[i].ToObject<Lista<ItemMoneda<string>>>());
                            break;
                        case NombreComponente.CuentasVinculadas:
                            var ctrl = new CuentaCompuesta();

                            foreach (var item in items[i].Properties())
                            {
                                var ctrlPropInfo = ctrl.GetType().GetProperty(item.Name, bindFlags);

                                if (ctrlPropInfo != null && !item.Name.ToLower().Equals("items"))
                                {
                                    //var val = atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType());
                                    ctrlPropInfo.SetValue(ctrl, item.Value.ParseGenericVal(item.Value.Type.ToString().ToType()));
                                }
                                else if (ctrlPropInfo != null && item.Name.ToLower().Equals("items"))
                                {
                                    foreach (var innerControl in item.Values())
                                    {
                                        var nombre = innerControl.Value<string>("Nombre");

                                        switch (nombre)
                                        {
                                            case NombreComponente.CuentaOperativa:

                                                ctrl.Items.Add(innerControl.ToObject<Lista<ItemCuentaOperativa<string>>>());

                                                break;
                                            case NombreComponente.CuentaTitulo:
                                                ctrl.Items.Add(innerControl.ToObject<Lista<ItemCuentaTitulos<string>>>());
                                                break;
                                        }
                                    }
                                }
                            }

                            form.Items.Add(ctrl);

                            break;

                        case NombreComponente.FondoCompuesto:
                            var fondoCompuesto = new FondoCompuesto();

                            foreach (var item in items[i].Properties())
                            {
                                var ctrlPropInfo = fondoCompuesto.GetType().GetProperty(item.Name, bindFlags);

                                if (ctrlPropInfo != null && !item.Name.ToLower().Equals("items"))
                                {
                                    //var val = atr.AtributoValor.ParseGenericVal(atr.AtributoDataType.ToType());
                                    ctrlPropInfo.SetValue(fondoCompuesto, item.Value.ParseGenericVal(item.Value.Type.ToString().ToType()));
                                }
                                else if (ctrlPropInfo != null && item.Name.ToLower().Equals("items"))
                                {
                                    var controles = item.Value.ToList();

                                    foreach (var control in controles)
                                    {
                                        var nombre = control.Value<string>("Nombre");

                                        switch (nombre)
                                        {
                                            case NombreComponente.LegalAgendamiento:
                                                var objFecha = control.ToObject<Lista<ItemLegal<string>>>();
                                                fondoCompuesto.Items.Add(objFecha);
                                                break;
                                            case NombreComponente.ListaFondos:
                                                var objLista = control.ToObject<Lista<ItemGrupoAgd>>();
                                                fondoCompuesto.Items.Add(objLista);
                                                break;
                                        }
                                    }
                                }
                            }

                            form.Items.Add(fondoCompuesto);
                            break;
                        default:
                            string Nombre = items[i].Property("Nombre").Value.ToString().ToLower();
                            throw new BusinessCodeException((int)TipoExcepcion.Otros, $"Control inexistente: ({items[i].ToString() + Nombre})");

                    }
                }
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

            form.Cabecera = ObtenerCabeceraMock();

            return form;
        }

        private static Cabecera ObtenerCabeceraMock()
        {
            return new Cabecera()
            {
                H_CanalTipo = "04",
                H_SubCanalId = "HTML",
                H_CanalVer = "000",
                H_SubCanalTipo = "99",
                H_CanalId = "0001",
                H_UsuarioTipo = "03",
                H_UsuarioID = "01FRQF31",
                H_UsuarioAttr = "  ",
                H_UsuarioPwd = "@DP33YTO",
                H_IdusConc = "788646",
                H_NumSec = "14",
                //H_Nup = entity.Nup,
                H_IndSincro = "1",
                H_TipoCliente = "I",
                H_TipoIDCliente = "N",
                H_NroIDCliente = "00020956698",
                H_FechaNac = "19690922"
            };

        }

        public static Type ToType(this string obj)
        {
            Type result = null;

            if (!string.IsNullOrWhiteSpace(obj))
            {
                switch (obj.ToLower())
                {
                    case "number":
                        result = typeof(decimal);
                        break;
                    case "number?":
                        result = typeof(decimal?);
                        break;
                    case "string":
                        result = typeof(string);
                        break;
                    case "integer":
                    case "int":
                        result = typeof(int);
                        break;
                    case "integer?":
                    case "int?":
                        result = typeof(int?);
                        break;
                    case "boolean":
                        result = typeof(bool);
                        break;
                    case "boolean?":
                        result = typeof(bool?);
                        break;
                    case "date":
                        result = typeof(DateTime);
                        break;
                    case "date?":
                        result = typeof(DateTime?);
                        break;
                    default:
                        throw new InvalidCastException($"El tipo de dato {obj} no posee mapeo.");
                }
            }

            return result;
        }

        public static object ParseGenericVal(this object obj, Type objType)
        {
            object result = null;

            if (obj != null && obj.GetType().Equals(objType.GetType()))
            {
                result = Convert.ChangeType(obj, objType);
            }
            else
            {
                try
                {
                    if (obj == null)
                    {
                        obj = objType.GetType().GetMethod("GetDefaultGeneric")?.Invoke(null, null);
                    }
                    else if (obj != null && Nullable.GetUnderlyingType(objType) != null)
                    {
                        var typeCon = Nullable.GetUnderlyingType(objType);
                        result = Convert.ChangeType(obj, typeCon);
                    }
                    else if (objType == typeof(bool))
                    {
                        switch (obj.ToString().ToLower())
                        {
                            case "1":
                                result = Convert.ToBoolean(Convert.ToInt16(obj));
                                break;

                            case "0":
                                result = Convert.ToBoolean(Convert.ToInt16(obj));
                                break;

                            default:
                                result = Convert.ToBoolean(obj);
                                break;
                        }

                    }
                    else
                    {
                        result = Convert.ChangeType(obj, objType, CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception)
                {
                    //throw new Exception(string.Format("No se puede convertir el valor {0} del tipo {1} a {2}", obj, obj.GetType(), objType));
                }
            }
            return result;
        }

        public static T ParseGenericVal<T>(this object obj)
        {
            T result = default(T);
            var isString = obj as string;

            if (obj is T)
            {
                result = (T)obj;
            }
            else
            {
                try
                {
                    if (obj == null)
                    {
                        obj = default(T);
                    }
                    else if (obj != null && Nullable.GetUnderlyingType(typeof(T)) != null)
                    {
                        //TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                        var typeCon = Nullable.GetUnderlyingType(typeof(T));
                        result = (T)Convert.ChangeType(obj, typeCon);
                    }
                    else
                    {
                        result = (T)Convert.ChangeType(obj, typeof(T));
                    }
                }
                catch (Exception)
                {
                    throw new Exception(string.Format("No se puede convertir el valor {0} del tipo {1} a {2}", obj, obj.GetType(), typeof(T)));
                }
            }

            return result;
        }

        public static Type ToControlMaps(this string obj, Type dataType = null)
        {
            Type result = null;

            try
            {
                switch (obj.ToLower())
                {
                    case NombreComponente.Moneda: //"moneda"
                        result = typeof(Lista<>).MakeGenericType(typeof(ItemMoneda<>).MakeGenericType(dataType));

                        break;

                    case NombreComponente.Operacion: //"operacion"
                        result = typeof(Lista<>).MakeGenericType(typeof(Item<>).MakeGenericType(dataType));

                        break;

                    case NombreComponente.CuentaOperativa: //"cuenta-operativa"
                        result = typeof(Lista<>).MakeGenericType(typeof(ItemCuentaOperativa<>).MakeGenericType(dataType));

                        break;

                    case NombreComponente.CuentaTitulo: //"cuenta-titulo"
                        result = typeof(Lista<>).MakeGenericType(typeof(ItemCuentaTitulos<>).MakeGenericType(dataType));

                        break;

                    case NombreComponente.Periodos: //"periodos"
                        //result = typeof(Lista<>).MakeGenericType(typeof(Item<>).MakeGenericType(dataType));
                        result = typeof(Lista<Item<decimal>>);

                        break;

                    case NombreComponente.Disclaimer: //"disclaimer"
                        result = typeof(Lista<>).MakeGenericType(typeof(ItemDisclaimer<>).MakeGenericType(dataType));

                        break;

                    case NombreComponente.DescripcionDinamica: //"descripcion-dinamica"
                        result = typeof(DescripcionDinamica<>).MakeGenericType(dataType);

                        break;

                    case NombreComponente.EstadoAdhesion: //"estado-adhesion"
                        result = typeof(EstadoAdhesion<>).MakeGenericType(dataType);

                        break;

                    case NombreComponente.Email:
                    case NombreComponente.Alias:
                    case NombreComponente.InputText:
                        result = typeof(InputText<>).MakeGenericType(dataType);

                        break;
                    case NombreComponente.MontoSuscripcionMinimo:
                    case NombreComponente.MontoSuscripcionMaximo:
                    case NombreComponente.Comprobante:
                    case NombreComponente.SaldoMinimo:
                    case NombreComponente.InputNumber: //"input-number"
                        result = typeof(InputNumber<>).MakeGenericType(dataType);

                        break;

                    case NombreComponente.Servicio: //"servicio"
                        result = typeof(Lista<>).MakeGenericType(typeof(ItemServicio<>).MakeGenericType(dataType));

                        break;
                    case NombreComponente.MontoSuscripcion:
                    case NombreComponente.ImporteCompuesto: //"importe-compuesto"://monto-suscripcion
                        result = typeof(ImporteCompuesto);
                        break;

                    case NombreComponente.Vigencia:
                    case NombreComponente.FechaCompuesta: //"fecha-compuesta"://vigencia
                        result = typeof(FechaCompuesta);

                        break;
                    case NombreComponente.FechaEjecucion:
                    case NombreComponente.FechaDesde:
                    case NombreComponente.FechaHasta:
                    case NombreComponente.Fecha: //"fecha"
                        result = typeof(Fecha);

                        break;
                    case NombreComponente.LegalAgendamiento:
                    case NombreComponente.Legal: //"legal"
                        result = typeof(Lista<ItemLegal<string>>);

                        break;
                    case NombreComponente.ListaFondos:
                    case NombreComponente.Fondo: //"fondo"
                        result = typeof(Lista<ItemGrupoAgd>);

                        break;

                    case NombreComponente.ListadoGenerico:
                    case NombreComponente.Frecuencia:
                    case NombreComponente.ListadoFondos:
                    case NombreComponente.ListaPep:
                    case NombreComponente.ListadoAsesoramiento:
                    case NombreComponente.Producto:
                        result = typeof(Lista<>).MakeGenericType(typeof(Item<>).MakeGenericType(dataType));

                        break;

                    case
                    NombreComponente.ConsultaAdhesiones:
                        result = typeof(ConsultaAdhesiones);
                        break;

                    case NombreComponente.CuentasVinculadas:
                        result = typeof(CuentaCompuesta);
                        break;

                    case NombreComponente.FondoCompuesto:
                        result = typeof(FondoCompuesto);
                        break;
                    default:
                        throw new BusinessCodeException((int)TipoExcepcion.Otros, $"Control inexistente: ({obj})");

                }

                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"No se pudo crear el innerControl ({obj} del tipo ({dataType})", ex);
            }
        }

        public static bool Is<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }
    }
}
