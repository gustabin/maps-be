using Isban.Maps.Entity;
using Isban.Maps.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isban.Maps.IDataAccess
{
    public interface IDdcDA
    {
        string ConnectionString { get; }
        ChequeoAcceso Chequeo(EntityBase entity);
        string GetInfoDB(long id);

    }
}
