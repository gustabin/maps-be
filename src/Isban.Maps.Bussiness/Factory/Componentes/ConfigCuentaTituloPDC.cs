using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Response;
using System;
using System.Collections.Generic;

namespace Isban.Maps.Business.Componente.Factory
{
    public class ConfigCuentaTituloPDC : ICrearComponente
    {
        private Lista<ItemCuentaTitulos<string>> _componente;
        private FormularioResponse _entity;
        private DatoFirmaMaps _firma;
        public ConfigCuentaTituloPDC(FormularioResponse _formulario, Lista<ItemCuentaTitulos<string>> item, DatoFirmaMaps _firma)
        {
            this._firma = _firma;
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            //var cuentasTitulo = BusinessHelper.ObtenerCuentasPorTipo(entity, "TI", firma, true);
            var ListaCtasTit = BusinessHelper.ObtenerCuentasPDC(_entity, _firma).ToArray();
            ClienteCuentaDDC[] cuentasValidas = BusinessHelper.ValidarCuentas(ListaCtasTit, _entity);

            if (cuentasValidas == null || cuentasValidas.Length == 0) //En caso que no existan cuentas titulo
            {
                _componente.Bloqueado = true;
                _componente.Validado = false;
            }
            else
            {
                foreach (var cuenta in cuentasValidas)
                {
                    var ctaTi = new ItemCuentaTitulos<string>();
                    ctaTi.NumeroCtaTitulo = ctaTi.Valor = cuenta.NroCta.TrimStart(new char[] { '0' }); //cuenta.NroCta;
                    ctaTi.Titulares = BusinessHelper.ObtenerTitulares(cuenta.Titulares);
                    ctaTi.Desc = "Cuenta custodia";
                    ctaTi.ValorPadre = null;
                    _componente.Items.Add(ctaTi);
                }
            }
        }
    }
}
