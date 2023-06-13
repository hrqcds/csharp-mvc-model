using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) { }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Param> Params { get; set; } = null!;


}