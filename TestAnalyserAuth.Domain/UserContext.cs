using Microsoft.EntityFrameworkCore;
using TestAnalyserAuth.Domain.Entity;

namespace TestAnalyserAuth.Domain;

public class UserContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public UserContext(DbContextOptions<UserContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}