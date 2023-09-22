using Challenge.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Data
{
    public class AppDbContext : DbContext
    {
        /// <summary>Initializes a new instance of the <see cref="T:Challenge.Data.AppDbContext" /> class.</summary>
        /// <param name="options">The options.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>Gets or sets the customers.</summary>
        /// <value>The customers.</value>
        public DbSet<Customer> Customers { get; set; }
    }
}
