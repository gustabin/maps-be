using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.Business.Componente.Factory
{
    public class ConfigCuentataOperPDC : ICrearComponente
    {
        private Lista<ItemCuentaOperativa<string>> _componente;
        private FormularioResponse _entity;
        private DatoFirmaMaps _firma;

        public ConfigCuentataOperPDC(FormularioResponse _formulario, Lista<ItemCuentaOperativa<string>> item, DatoFirmaMaps firma)
        {
            _entity = _formulario;
            _componente = item;
            _firma = firma;
        }

        public void Crear()
        {
            var cuentasOperativas = BusinessHelper.ObtenerCuentasPorTipo(_entity, "OP", _firma, true);
            //Se filtra por las cuentas que se puede operar
            var cuentasValidas = BusinessHelper.ValidarCuentas(cuentasOperativas, _entity);
            _componente.Items = new List<ItemCuentaOperativa<string>>();

            if (cuentasOperativas == null || cuentasOperativas.Length == 0)
            {
                _componente.Bloqueado = true;
                _componente.Validado = false;
            }
            else
            {
                foreach (var cuenta in cuentasOperativas)
                {
                    var ctaOp = new ItemCuentaOperativa<string>();
                    ctaOp.TipoCtaOperativa = cuenta.TipoCta.ToString().PadLeft(2, '0');
                    ctaOp.NumeroCtaOperativa = ctaOp.Valor = cuenta.NroCta.TrimStart(new char[] { '0' });
                    ctaOp.Producto = cuenta.CodProducto;
                    ctaOp.SubProducto = cuenta.CodSubproducto;
                    ctaOp.SucursalCtaOperativa = cuenta.SucursalCta;
                    ctaOp.CodigoMoneda = cuenta.CodigoMoneda;
                    ctaOp.Desc = cuenta.DescripcionTipoCta;
                    ctaOp.Titulares = BusinessHelper.ObtenerTitulares(cuenta.Titulares);
                    ctaOp.ValorPadre = cuenta.CodigoMoneda;
                    var saldo = BusinessHelper.ObtenerSaldoCuenta(cuenta, _entity.Segmento, _entity.Nup, _entity.Canal, _entity.SubCanal, _entity.Ip, _entity.Usuario);
                    if (saldo != null)
                        ctaOp.SaldoCta = saldo;
                    _componente.Items.Add(ctaOp);
                }
            }
        }
    }
}

