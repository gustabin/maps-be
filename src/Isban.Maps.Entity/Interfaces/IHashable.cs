using System.Collections.Generic;

namespace Isban.Maps.Entity.Interfaces
{
    interface IHashable
    {
        void ValidarHash();
        void GenerarHash();
        List<object> GetValor();
    }
}
