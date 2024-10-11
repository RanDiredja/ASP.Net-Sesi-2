using Microsoft.EntityFrameworkCore;
using MyUniversity.Models;

namespace MyUniversity.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
        }

        public DbSet<Major> Major { get; set; }
        public DbSet<Student> Student { get; set; }
    }
}
