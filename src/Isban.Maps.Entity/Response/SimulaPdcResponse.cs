
namespace Isban.Maps.Entity.Request
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class SimulaPdcResponse
    {
        #region privates
        private long? _idsimcuentapdc = null;
        private string _nup = string.Empty;
        private long? _cuentatitulos = null;
        private string _nroctaoperativa = string.Empty;
        private string _tipoctaoperativa = string.Empty;
        private string _sucursalctaoperativa = string.Empty;
        private string _codigomoneda = string.Empty;
        private DateTime? _fechapedido = null;
        private DateTime? _fechaefectiva = null;
        private DateTime? _fechavigenciadesde = null;
        private string _canal = string.Empty;
        private string _subcanal = string.Empty;
        private string _segmento = string.Empty;
        private string _operacion = string.Empty;
        private string _producto = string.Empty;
        private string _subproducto = string.Empty;
        private long? _idcuentapdc = null;
        #endregion

        [DataMember]
        public long? IDSimCuentaPDC
        {
            get { return _idsimcuentapdc; }
            set
            {
                _idsimcuentapdc = value ?? 0;
            }
        }

        [DataMember]
        public long? IDCuentaPDC
        {
            get { return _idcuentapdc; }
            set
            {
                _idcuentapdc = value ?? 0;
            }
        }

        [DataMember]
        public string NUP
        {
            get { return _nup; }
            set
            {
                _nup = !string.IsNullOrEmpty(value) ? string.Format("{0:D8}", int.Parse(value)) : string.Empty;
            }
        }

        [DataMember]
        public long? CuentaTitulos
        {
            get { return _cuentatitulos; }
            set
            {
                _cuentatitulos = value ?? 0;
            }
        }

        [DataMember]
        public string NroCtaOperativa
        {
            get { return _nroctaoperativa; }
            set
            {
                _nroctaoperativa = !string.IsNullOrEmpty(value) ? string.Format("{0:D12}", int.Parse(value)) : string.Empty;
            }
        }

        [DataMember]
        public string TipoCtaOperativa
        {
            get { return _tipoctaoperativa; }
            set
            {
                _tipoctaoperativa = !string.IsNullOrEmpty(value) ? string.Format("{0:D2}", int.Parse(value)) : string.Empty;
            }
        }

        [DataMember]
        public string SucursalCtaOperativa
        {
            get { return _sucursalctaoperativa; }
            set
            {
                _sucursalctaoperativa = !string.IsNullOrEmpty(value) ? string.Format("{0:D4}", int.Parse(value)) : string.Empty;
            }
        }

        [DataMember]
        public string CodigoMoneda
        {
            get { return _codigomoneda; }
            set
            {
                _codigomoneda = !string.IsNullOrEmpty(value) ? value : string.Empty;
            }
        }

        [DataMember]
        public DateTime? FechaPedido
        {
            get { return _fechapedido; }
            set
            {
                _fechapedido = value ?? DateTime.MinValue;
            }
        }

        [DataMember]
        public DateTime? FechaEfectiva
        {
            get { return _fechaefectiva; }
            set
            {
                _fechaefectiva = value ?? DateTime.MinValue;
            }
        }

        [DataMember]
        public DateTime? FechaVigenciaDesde
        {
            get { return _fechavigenciadesde; }
            set
            {
                _fechavigenciadesde = value ?? DateTime.MinValue;
            }
        }

        [DataMember]
        public string Canal
        {
            get { return _canal; }
            set
            {
                _canal = !string.IsNullOrEmpty(value) ? string.Format("{0:D2}", int.Parse(value)) : string.Empty;
            }
        }

        [DataMember]
        public string Subcanal
        {
            get { return _subcanal; }
            set
            {
                _subcanal = !string.IsNullOrEmpty(value) ? string.Format("{0:D4}", int.Parse(value)) : string.Empty;
            }
        }

        [DataMember]
        public string Segmento
        {
            get { return _segmento; }
            set
            {
                _segmento = value;
            }
        }

        [DataMember]
        public string Operacion
        {
            get { return _operacion; }
            set
            {
                //if () { throw new Exception("Se está intentando asignar un valor nulo al atributo Operacion"); }
                _operacion = !string.IsNullOrEmpty(value) ? value.ToUpper() : string.Empty;
            }
        }

        [DataMember]
        public string Producto
        {
            get { return _producto; }
            set
            {
                _producto = !string.IsNullOrEmpty(value) ? string.Format("{0:D2}", int.Parse(value)) : string.Empty;
            }
        }

        [DataMember]
        public string Subproducto
        {
            get { return _subproducto; }
            set
            {
                _subproducto = !string.IsNullOrEmpty(value) ? string.Format("{0:D4}", int.Parse(value)) : string.Empty;
            }
        }
    }
}
