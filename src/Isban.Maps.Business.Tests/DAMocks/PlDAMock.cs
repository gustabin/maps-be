using System;
using Isban.Maps.Entity;
using Isban.Maps.Entity.Base;
using Isban.Maps.IDataAccess;

namespace Isban.Maps.Business.Tests.Tests
{
    public class PlDAMock : IPlDA
    {
        public string ConnectionString
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ChequeoAcceso Chequeo(EntityBase entity)
        {
            throw new NotImplementedException();
        }

        public string GetInfoDB(long id)
        {
            throw new NotImplementedException();
        }
    }
}