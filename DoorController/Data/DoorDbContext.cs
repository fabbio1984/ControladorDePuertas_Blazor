using DoorController.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorController.Data
{
    public class DoorDbContext : DbContext
    {
        public DoorDbContext(DbContextOptions<DoorDbContext> options) : base(options) {}

        public DbSet<Door> Doors => Set<Door>();
        public DbSet<DoorEvent> DoorEvents => Set<DoorEvent>();
    }
}
