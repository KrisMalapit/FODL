using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FODLSystem.Models;

namespace FODLSystem.Models
{
    public class SEMSystemContext : DbContext
    {
        public SEMSystemContext(DbContextOptions<SEMSystemContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
       
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }

    
        public DbSet<NoSeries> NoSeries { get; set; }
       
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasIndex(p => new { p.Username, p.Status })
               .IsUnique();
           
            modelBuilder.Entity<Role>().HasData(
                new { Id = 1, Name = "Admin", Status = "Active" },
                new { Id = 2, Name = "User", Status = "Active" }

           );

            modelBuilder.Entity<User>().HasData(
               new { Id = 1,Username = "kcmalapit",RoleId = 1,Password = "",FirstName = "Kristoffer", LastName = "Malapit",Status = "1", Email = "kcmalapit@semirarampc.com", DepartmentId = 1, Name = "Kristoffer Malapit", Domain = "SMCDACON", CompanyAccess = "1"}
           );

        }

        internal object Include(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
