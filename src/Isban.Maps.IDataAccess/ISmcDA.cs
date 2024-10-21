using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isban.Maps.Entity.Base;
using Isban.Maps.Entity;
using Isban.Maps.Entity.Request;
using Isban.Maps.Entity.Response;

namespace Isban.Maps.IDataAccess
{
    public interface ISmcDA
    {
        string ConnectionString { get; }
        ChequeoAcceso Chequeo(EntityBase entity);
        string GetInfoDB(long id);

        SaldoConcertadoNoLiquidadoResponse EjecutarSaldoConcertadoNoLiquidado(SaldoConcertadoNoLiquidadoRequest entity);

        void InsertOrder(InsertOrderRequest entity);

        void UpdateOrder(UpdateOrderRequest entity);
    }
}
