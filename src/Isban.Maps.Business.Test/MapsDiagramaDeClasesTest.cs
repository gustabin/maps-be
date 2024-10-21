

namespace Isban.Maps.Business.Test
{
    using Entity.Base;
    using Entity.Controles;
    using Entity.Controles.Compuestos;
    using Entity.Controles.Customizados;
    using Entity.Controles.Independientes;
    using Entity.Interfaces;
    using Entity.Request;
    using Entity.Response;
    using NUnit.Framework;

    [TestFixture]
    public class MapsDiagramaDeClasesTest
    {
        #region Test OK

        #region Items Ok
        [Test]
        public void ItemBaseOK()
        {
            Assert.True(typeof(IItem).IsAssignableFrom(typeof(ItemBase<>)));
            Assert.True(typeof(IValor<string>).IsAssignableFrom(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemFondoGrupoOK()
        {
            Assert.True(typeof(ItemFondoGrupo<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemLegalOK()
        {
            Assert.True(typeof(ItemLegal<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemPeriodoOK()
        {
            Assert.True(typeof(Item<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemGrupoOK()
        {
            Assert.True(typeof(ItemGrupo<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemMonedaOK()
        {
            Assert.True(typeof(ItemMoneda<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemOperacionOK()
        {
            Assert.True(typeof(Item<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemCuentaOperativaOK()
        {
            Assert.True(typeof(ItemCuentaOperativa<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }
        [Test]
        public void ItemServicioOK()
        {
            Assert.True(typeof(ItemServicio<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemCuentaTituloOK()
        {
            Assert.True(typeof(ItemCuentaTitulos<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemOK()
        {
            Assert.True(typeof(Item<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }      

        [Test]
        public void ItemDisclaimberOK()
        {
            Assert.True(typeof(ItemDisclaimer<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }
        #endregion

        [Test]
        public void ListaEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(Lista<>).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void FormularioResponseEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(FormularioResponse).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void FormularioRequestEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(FormularioRequest).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void ImporteCompuestoEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(ImporteCompuesto).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void ConsultaAdhesionesEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(ConsultaAdhesiones).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void InputNumberEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(InputNumber<>).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void InputTextEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(InputText<>).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void FechaCompuestaEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(FechaCompuesta).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void FechaaEscontrolSimpleOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(Fecha).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void ControlSimpleEsMapsControlOk()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.True(typeof(ControlSimple).IsSubclassOf(typeof(MapsControlBase)));
        }
        #endregion

        #region Test Error
        #region Items Error
        [Test]
        public void ItemFondoGrupoError()
        {
            Assert.False(!typeof(ItemFondoGrupo<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemLegalError()
        {
            Assert.False(!typeof(ItemLegal<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemPeriodoError()
        {
            Assert.False(!typeof(Item<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemGrupoError()
        {
            Assert.False(!typeof(ItemGrupo<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemMonedaError()
        {
            Assert.False(!typeof(ItemMoneda<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemOperacionError()
        {
            Assert.False(!typeof(Item<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemCuentaOperativaError()
        {
            Assert.False(!typeof(ItemCuentaOperativa<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }
        [Test]
        public void ItemServicioError()
        {
            Assert.False(!typeof(ItemServicio<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemCuentaTituloError()
        {
            Assert.False(!typeof(ItemCuentaTitulos<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemError()
        {
            Assert.False(!typeof(Item<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }

        [Test]
        public void ItemDisclaimberError()
        {
            Assert.False(!typeof(ItemDisclaimer<string>).IsSubclassOf(typeof(ItemBase<string>)));
        }
        #endregion

        [Test]
        public void ListaEscontrolSimpleError()
        {
            Assert.False(!typeof(Lista<>).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void FormularioResponseEscontrolSimpleError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(FormularioResponse).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void FormularioRequestEscontrolSimpleError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(FormularioRequest).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void ImporteCompuestoEscontrolSimpleError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(ImporteCompuesto).IsSubclassOf(typeof(ControlSimple)));
        }


        [Test]
        public void ConsultaAdhesionesEscontrolSimpleError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(ConsultaAdhesiones).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void InputNumberEscontrolSimpleError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(InputNumber<>).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void InputTextEscontrolSimpleError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(InputText<>).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void FechaCompuestaEscontrolSimpleError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(FechaCompuesta).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void FechaEscontrolSimpleError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(Fecha).IsSubclassOf(typeof(ControlSimple)));
        }

        [Test]
        public void ControlSimpleEsMapsControlError()
        {
            //verifico que se mantenga la herencia pactada.       
            Assert.False(!typeof(ControlSimple).IsSubclassOf(typeof(MapsControlBase)));
        }
        #endregion


    }
}

