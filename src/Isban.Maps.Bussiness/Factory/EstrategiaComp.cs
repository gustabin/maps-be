namespace Isban.Maps.Business.Factory
{
    public class EstrategiaComp
    {
        private readonly ICrearComponente _configControl;

        //Seria como primero configuro el control en si, luego sus configItems.
        public EstrategiaComp(ICrearComponente configComponente)
        {
            _configControl = configComponente;
        }

        public void Crear()
        {
            _configControl.Crear();
        }       
    }
}
