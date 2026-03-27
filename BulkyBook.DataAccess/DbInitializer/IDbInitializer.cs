using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
