using System;
using Isban.Maps.Entity;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.IDataAccess;

namespace Isban.Maps.Business.Tests
{
    public class SmcDAMock : ISmcDA
    {
        public SaldoConcertadoNoLiquidadoResponse EjecutarSaldoConcertadoNoLiquidado(SaldoConcertadoNoLiquidadoRequest entity)
        {
            return new SaldoConcertadoNoLiquidadoResponse
            {
                Saldo = entity.SucCtaOper == "0" ? 789632145 : (decimal?)null
            };
        }

        public void InsertOrder(InsertOrderRequest entity)
        {
            
        }

        public void UpdateOrder(UpdateOrderRequest entity)
        {

        }

        public string ConnectionString
        {
            get
            {
                return "Data Source=GSAD3;enlist=true;User Id={0};Password={1};credentialId=68002";
            }
        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            return new ChequeoAcceso()
            {
                BasedeDatos = "TEST",
                ConnectionString = "Data Source = GSAD3; enlist = true; User Id = { 0 }; Password ={ 1}; credentialId = 68002",
                Hash = "",
                Ok = true,
                ServidorDB = "Test",
                ServidorWin = "unit test",
                UsuarioDB = "usuarioTest",
                UsuarioWin = "usuarioTestWin"
            };
        }

        public string GetInfoDB(long id)
        {
            return "TEST";
        }
    }
}