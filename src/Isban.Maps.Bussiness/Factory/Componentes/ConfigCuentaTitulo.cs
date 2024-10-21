using Isban.Maps.Business.Factory;
using Isban.Maps.Bussiness;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Constantes.Estructuras;
using Isban.Maps.Entity.Controles;
using Isban.Maps.Entity.Controles.Customizados;
using Isban.Maps.Entity.Response;
using System.Collections.Generic;

namespace Isban.Maps.Business.Componente.Factory
{
    public class ConfigCuentaTitulo : ICrearComponente
    {
        private Lista<ItemCuentaTitulos<string>> _componente;
        private FormularioResponse _entity;
        private DatoFirmaMaps _firma;

        public ConfigCuentaTitulo(FormularioResponse _formulario, Lista<ItemCuentaTitulos<string>> item, DatoFirmaMaps _firma)
        {
            this._firma = _firma;
            _entity = _formulario;
            _componente = item;
        }

        public void Crear()
        {
            #region CuentaTitulo
            var cuentasTitulos = BusinessHelper.ObtenerCuentasPorTipo(_entity, "TI", _firma, true); // Tipo == 8 && CodProducto == 60
            cuentasTitulos = BusinessHelper.ValidarCuentasBloquedas(cuentasTitulos);

            _componente.Items = new List<ItemCuentaTitulos<string>>();

            if (cuentasTitulos.Length == 0) //En caso que no existan cuentas titulo
            {
                _componente.Bloqueado = true;
                _componente.Validado = false;
            }

            for (int j = 0; j < cuentasTitulos.Length; j++)
            {
                if (_entity.IdServicio == Servicio.Rtf && BusinessHelper.ValidarCuentaDisponible(cuentasTitulos[j].NroCta, _entity))
                {
                    var ctaTi = new ItemCuentaTitulos<string>();
                    ctaTi.NumeroCtaTitulo = ctaTi.Valor = cuentasTitulos[j].NroCta.TrimStart(new char[] { '0' });

                    var titularesDesc = _entity.IdServicio != Servicio.SAF ? BusinessHelper.ObtenerTitulares(cuentasTitulos[j].Titulares) : null;

                    ctaTi.Titulares = titularesDesc;
                    ctaTi.Desc = string.IsNullOrWhiteSpace(ctaTi.Desc) ? ctaTi.NumeroCtaTitulo : ctaTi.Desc;
                    _componente.Items.Add(ctaTi);
                }
                else if (_entity.IdServicio != Servicio.Rtf)
                {
                    var ctaTi = new ItemCuentaTitulos<string>();
                    ctaTi.NumeroCtaTitulo = ctaTi.Valor = cuentasTitulos[j].NroCta.TrimStart(new char[] { '0' });

                    var titularesDesc = _entity.IdServicio != Servicio.SAF ? BusinessHelper.ObtenerTitulares(cuentasTitulos[j].Titulares) : null;

                    ctaTi.Titulares = titularesDesc;
                    ctaTi.Desc = string.IsNullOrWhiteSpace(ctaTi.Desc) ? ctaTi.NumeroCtaTitulo : ctaTi.Desc;
                    _componente.Items.Add(ctaTi);
                }
            }
            #endregion
        }
    }
}
