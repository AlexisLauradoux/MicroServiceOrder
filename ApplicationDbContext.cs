using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServiceOrder
{
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite($"Data Source={nameof(MicroServiceOrder)}.db");

        public DbSet<Order> Orders { get; set; }

        public DbSet<ItemOrder> ItemOrders { get; set; }
    }
}
