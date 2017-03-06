using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Core.Models;
using Microsoft.Extensions.Logging;

namespace Demo.Core.Repositories
{
    public class ReservationContext : DbContext
    {

        //public OrderDemoContext(string connectionString) : base(connectionString)
        //{
        //    Configuration.LazyLoadingEnabled = false;
        //    Configuration.AutoDetectChangesEnabled = false;
        //}

        //public OrderDemoContext() : base("name=")
        //{
        //    Configuration.LazyLoadingEnabled = false;
        //}

        public ReservationContext()
        {

        }

        public ReservationContext(string connString) : base(connString)
        {
            //_logger = logger;
            //if (logger != null)
            //{
            //    Database.Log = msg => _logger.LogInformation(2, msg);
            //}

            Configuration.AutoDetectChangesEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        //public ReservationContext()
        //{
        //    Database.Log = msg => Trace.Write(msg);
        //    Configuration.AutoDetectChangesEnabled = false;
        //    Configuration.LazyLoadingEnabled = false;
        //}

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //todo : reflection to do for all entities? maybe implement interface
            modelBuilder.Entity<Order>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Customer>()
               .Property(p => p.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // modelBuilder.Entity<Customer>().Ignore(t => t.OrdersCount);


            modelBuilder.Entity<Address>()
               .Property(p => p.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Order>()
                .Ignore(e => e.Events);

            modelBuilder.Entity<OrderLine>()
               .Property(p => p.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Product>()
              .Property(p => p.Id)
              .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);





            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            //var domainEventEntities = ChangeTracker.Entries<IEntity>()
            //    .Select(po => po.Entity)
            //    .Where(po => po.Events.Any())
            //    .ToArray();

            //foreach (var entity in domainEventEntities)
            //{
            //    var events = entity.Events.ToArray();
            //    entity.Events.Clear();
            //    foreach (var domainEvent in events)
            //    {
            //       // _dispatcher.Dispatch(domainEvent);
            //    }
            //}

            return base.SaveChanges();
        }
    }

}
