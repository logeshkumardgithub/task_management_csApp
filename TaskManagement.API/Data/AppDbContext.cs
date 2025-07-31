
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Models;
using TaskManagement.API.Data;

namespace TaskManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<AppUser> Users { get; set; }
    }
}
