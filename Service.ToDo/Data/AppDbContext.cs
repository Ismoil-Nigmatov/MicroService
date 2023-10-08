using Microsoft.EntityFrameworkCore;
using Service.ToDo.Entity;
using Task = Service.ToDo.Entity.Task;

namespace Service.ToDo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

    }
}
