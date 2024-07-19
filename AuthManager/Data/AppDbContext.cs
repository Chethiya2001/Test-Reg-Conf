using AuthManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
  
    public DbSet<UserAccount> Users { get; set; }
}
