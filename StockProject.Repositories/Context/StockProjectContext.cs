using Microsoft.EntityFrameworkCore;
using StockProject.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.Repositories.Context
{
    public class StockProjectContext: DbContext
    {
        public StockProjectContext(DbContextOptions<StockProjectContext> options):base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-AAEQRV0;Database=DB_StockProject;Trusted_Connection=True;");
        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
    }
}
