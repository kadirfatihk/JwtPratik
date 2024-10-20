using JwtPratik.Entities;
using Microsoft.EntityFrameworkCore;

namespace JwtPratik.Context
{
    public class PratikJwtDbContext : DbContext
    {
       public PratikJwtDbContext(DbContextOptions<PratikJwtDbContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
    }
}
