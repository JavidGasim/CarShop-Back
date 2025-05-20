using CarShop.Entities.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarShop.DataAccess.Data
{
    public class CarShopDbContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
    {
        public CarShopDbContext(DbContextOptions<CarShopDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Favourite> Favourites { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = CarShopDB; Integrated Security = True;", b => b.MigrationsAssembly("CarShop.WepApi"));
        }
    }
}