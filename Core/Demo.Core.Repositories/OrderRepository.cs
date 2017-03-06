using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Interfaces;
using Demo.Core.Models;
using Microsoft.Extensions.Logging;

namespace Demo.Core.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ReservationContext context, ILogger<Order> logger) : base(context, logger)
        {
           
        }

        public override Task<List<Order>> Get(Expression<Func<Order, bool>> filter = null, Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = null, string includeProperties = "")
        {
            Context.Database.Log = message => Logger?.LogInformation(message);
            return base.Get(filter, orderBy, includeProperties);
        }

        public override Task<Order> GetById(object id)
        {
            return DbSet.Include("OrderLines").AsNoTracking().SingleOrDefaultAsync(i => i.Id == (int) id);
            
        }

        public override Order Update(Order entityToUpdate)
        {
            Context.Database.Log = msg => Logger?.LogInformation(2, msg);
            var entity = DbSet.Attach(entityToUpdate);
            foreach (OrderLine orderLine in entityToUpdate.OrderLines)
            {
                if (orderLine.Id == 0)
                {
                    ((ReservationContext) Context).OrderLines.Add(orderLine);
                }
            }

            Context.Entry(entityToUpdate).State = EntityState.Modified;



            return entity;
            //return base.Update(entityToUpdate);
        }
    }
}
