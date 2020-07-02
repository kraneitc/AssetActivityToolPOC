using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AATWebApi.Models
{
    public sealed class AATDbContext : DbContext
    {

        public AATDbContext(DbContextOptions<AATDbContext> options) : base(options)
        {
            if (Database.GetDbConnection() is SqlConnection conn)
                conn.AccessToken = new AzureServiceTokenProvider().GetAccessTokenAsync("https://database.windows.net/")
                    .Result;
        }

        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity => { entity.ToTable("Product", "SalesLT"); });
        }
    }
}
