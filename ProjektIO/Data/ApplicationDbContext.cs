using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektIO.Entities;

namespace ProjektIO.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Hedgefund> Hedgefunds { get; set; }
        public DbSet<HedgefundHistory> HedgefundsHistory { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}