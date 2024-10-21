using Isban.Maps.IDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;
using Isban.Maps.Entity;
using Isban.Maps.Entity.Base;

namespace Isban.Maps.Business.Tests
{
    public class OpicsDAMock : IOpicsDA
    {
        public string ConnectionString
        {
            get
            {
                return "Data Source=GSAD3;enlist=true;User Id={0};Password={1};credentialId=61000";
            }
        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            return new ChequeoAcceso()
            {
                BasedeDatos = "TEST",
                ConnectionString = "Data Source = GSAD3; enlist = true; User Id = { 0 }; Password ={ 1}; credentialId = 61000",
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

        public long? AltaCuentaTiluloOpics(AltaCuentaOpicsReq entity)
        {
            return 0;
        }

        public LoadSaldosResponse EjecutarLoadSaldos(LoadSaldosRequest entity)
        {
            return new LoadSaldosResponse
            {
                ListaSaldos = new List<Saldos> {
                          new Saldos {
                            Asesor = 99,
                            CAhorroDolares=987654321,
                            CAhorroPesos=999000000,
                            Cuenta=9911223366,
                            Descripcion="Descripcion TEst",
                            Fecha=DateTime.Now,
                            Sucursal=0
                          }
                }
            };
        }

        public List<ConsultaLoadAtisResponse> ObtenerAtis(ConsultaLoadAtisRequest consultaLoadAtisRequest)
        {
            var consultaAtis = new List<ConsultaLoadAtisResponse>();
            consultaAtis.Add(
                new ConsultaLoadAtisResponse
                {
                    CuentaAtit = 11111111,
                    CuentaBp = 1119911223366
                }
                );

            return consultaAtis;
        }
    }
}
