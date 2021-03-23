using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JewelryStore.Model
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserDetails>().HasKey(ud => ud.Id);
            modelBuilder.Entity<UserDetails>().HasKey(ud => new { ud.UserName, ud.Password });
            base.OnModelCreating(modelBuilder);
        }
    }
}
