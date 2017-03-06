using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Repositories
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/core/data/entity-framework-6
    /// this is only for the command line (think migrations etc)
    /// </summary>
    public class ReservationContextFactory : IDbContextFactory<ReservationContext>
    {

        public ReservationContext Create()
        {
            return new ReservationContext(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Demo.Core.Repositories.OrderDemoContext;Integrated Security=True;MultipleActiveResultSets=true");
        }
    }
}
