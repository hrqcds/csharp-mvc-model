using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Param> Params { get; set; } = null!;

    // Seed data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new User()
        {
            ID = Guid.NewGuid().ToString(),
            Name = "ti",
            Register = "ti",
            Email = "ti@ti.com",
            Role = Roles.TI
        });
        modelBuilder.Entity<User>().HasData(new User()
        {
            ID = Guid.NewGuid().ToString(),
            Name = "adm",
            Register = "adm",
            Email = "adm@adm.com",
            Role = Roles.ADM
        });
    }

}