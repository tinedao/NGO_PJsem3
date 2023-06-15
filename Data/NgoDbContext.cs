using Microsoft.EntityFrameworkCore;
using NGO_PJsem3.Models;
using System.Collections.Generic;

namespace NGO_PJsem3.Data
{
    public class NgoDbContext : DbContext
    {
        public NgoDbContext(DbContextOptions<NgoDbContext> options)
        : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Causes> Causes { get; set; }
        public DbSet<Donations> Donations { get; set; }
        public DbSet<NGOs> NGOs { get; set; }
        public DbSet<Programs> Programs { get; set; }
        public DbSet<Querys> Querys { get; set; }
    }
}
