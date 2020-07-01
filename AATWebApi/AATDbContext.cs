using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AATWebApi;
using Microsoft.EntityFrameworkCore;

namespace AATWebApp
{
    public class AATDbContext : DbContext
    {

        public DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:pocattdb.database.windows.net,1433;Initial Catalog=pocattdb;Persist Security Info=False;User ID=pocattdb;Password=I*ecQF@aK28uH*W^5*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
