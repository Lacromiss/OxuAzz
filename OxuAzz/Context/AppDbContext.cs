using Microsoft.EntityFrameworkCore;
using OxuAzz.Models;

namespace OxuAzz.Context
{
    public class AppDbContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("server =DESKTOP-LUJ8MVB; database=OxuAzz1; integrated security=true;TrustServerCertificate=True");
        }
        public DbSet<New> News { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
