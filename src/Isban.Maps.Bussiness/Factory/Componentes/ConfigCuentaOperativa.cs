using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Response;
using System.Collections.Generic;

namespace Isban.Maps.Business.Componente.Factory
{
    public class ConfigCuentaOperativa : ICrearComponente
    {
        private Lista<ItemCuentaOperativa<string>> _componente;
        private FormularioResponse _entity;
        private DatoFirmaMaps _firma;

        public ConfigCuentaOperativa(FormularioResponse _formulario, Lista<ItemCuentaOperativa<string>> item, DatoFirmaMaps firma)
        {
            _entity = _formulario;
            _componente = item;
            _firma = firma;
        }

        public void Crear()
        {
            #region CuentaOperativa            

            var cuentasOperativas = BusinessHelper.ObtenerCuentasPorTipo(_entity, "OP", _firma, true); // Tipo != 8 && CodProducto != 60
                                                                                                       //Se filtra por las cuentas que se puede operar
            var cuentasValidas = BusinessHelper.ValidarCuentas(cuentasOperativas, _entity);
            _componente.Items = new List<ItemCuentaOperativa<string>>();

            if (cuentasValidas.Length == 0) //En caso que no existan cuentas para asociar
            {
                _componente.Bloqueado = true;
                _componente.Validado = false;
            }

            for (int l = 0; l < cuentasValidas.Length; l++)
            {
                var ctaOp = new ItemCuentaOperativa<string>();
                ctaOp.TipoCtaOperativa = cuentasValidas[l].TipoCta.ToString().PadLeft(2, '0');
                ctaOp.NumeroCtaOperativa = ctaOp.Valor = cuentasValidas[l].NroCta.TrimStart(new char[] { '0' });
                ctaOp.Producto = cuentasValidas[l].CodProducto;
                ctaOp.SubProducto = cuentasValidas[l].CodSubproducto;
                ctaOp.SucursalCtaOperativa = cuentasValidas[l].SucursalCta;
                ctaOp.CodigoMoneda = cuentasValidas[l].CodigoMoneda;
                ctaOp.Desc = cuentasValidas[l].DescripcionTipoCta;
                ctaOp.Titulares = BusinessHelper.ObtenerTitulares(cuentasValidas[l].Titulares);
                ctaOp.ValorPadre = cuentasValidas[l].CodigoMoneda;
                var saldo = BusinessHelper.ObtenerSaldoCuenta(cuentasValidas[l], _entity.Segmento, _entity.Nup, _entity.Canal, _entity.SubCanal, _entity.Ip, _entity.Usuario,_firma);
                if (saldo != null)
                    ctaOp.SaldoCta = saldo;
                _componente.Items.Add(ctaOp);
            }
            #endregion
        }

    }
}
