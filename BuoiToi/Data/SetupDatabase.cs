using BuoiToi.Models;
using Microsoft.EntityFrameworkCore;

namespace BuoiToi.Data
{
    public class SetupDatabase : DbContext
    {
        public SetupDatabase(DbContextOptions options) :base(options)
        {
        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Bill> Bills { get; set; }

        public DbSet<BillDetail> BillDetails { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
