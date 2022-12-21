using lojatcc.authentication.api.Services.Entities;
using Microsoft.EntityFrameworkCore;

namespace lojatcc.authentication.api.Services;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}