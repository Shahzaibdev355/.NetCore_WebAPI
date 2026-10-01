using Microsoft.EntityFrameworkCore;
using netcore_webapi.Entities;

namespace netcore_webapi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
    {

        public DbSet<User> AccountUsers { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

    }
}
