using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace OrderManager
{
    public class OrderContext : DbContext
    {

        public OrderContext() : base("OrderDatabase")
        {
            Database.SetInitializer(
            new DropCreateDatabaseIfModelChanges<OrderContext>());
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> GoodItems { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}
