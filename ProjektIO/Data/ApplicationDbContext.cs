using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektIO.Entities;
using ProjektIO.Models;

namespace ProjektIO.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRolesIndex>().ToView("UserRolesIndex").HasNoKey();
            base.OnModelCreating(builder);
        }
        public DbSet<Hedgefund> Hedgefunds { get; set; }
        public DbSet<HedgefundHistory> HedgefundsHistory { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<UserRolesIndex> UserRolesIndex { get; set; } 
        public DbSet<Salaries> Salaries { get; set; }
    }
}