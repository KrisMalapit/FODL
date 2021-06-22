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
        public DbSet<Area> Areas { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemLog> ItemLogs { get; set; }
        public DbSet<NoSeries> NoSeries { get; set; }
       
        public DbSet<Department> Departments { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<BicycleEntryHeader> BicycleEntryHeaders { get; set; }
        public DbSet<BicycleEntryDetail> BicycleEntryDetails { get; set; }



        public DbSet<FireExtinguisherHeader> FireExtinguisherHeaders { get; set; }
        public DbSet<FireExtinguisherDetail> FireExtinguisherDetails { get; set; }

        public DbSet<EmergencyLightHeader> EmergencyLightHeaders { get; set; }
        public DbSet<EmergencyLightDetail> EmergencyLightDetails { get; set; }

        public DbSet<FireHydrantHeader> FireHydrantHeaders { get; set; }
        public DbSet<FireHydrantDetail> FireHydrantDetails { get; set; }



        public DbSet<InergenTankHeader> InergenTankHeaders { get; set; }
        public DbSet<InergenTankDetail> InergenTankDetails { get; set; }


        public DbSet<LocationFireExtinguisher> LocationFireExtinguishers { get; set; }
       
        public DbSet<LocationItemDetail> LocationItemDetails { get; set; }
        
        public DbSet<LocationEmergencyLight> LocationEmergencyLights { get; set; }
        public DbSet<LocationInergenTank> LocationInergenTanks { get; set; }

        public DbSet<LocationFireHydrant> LocationFireHydrants { get; set; }
        
        public DbSet<Bicycle> Bicycles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasIndex(p => new { p.Username, p.Status })
               .IsUnique();

            modelBuilder.Entity<LocationFireExtinguisher>()
              .HasIndex(p => new { p.AreaId, p.Code, p.Status })
              .IsUnique();

            modelBuilder.Entity<LocationFireHydrant>()
             .HasIndex(p => new { p.AreaId, p.Code, p.Status })
             .IsUnique();

            modelBuilder.Entity<LocationEmergencyLight>()
             .HasIndex(p => new { p.AreaId, p.Code, p.Status })
             .IsUnique();

            modelBuilder.Entity<Bicycle>()
              .HasIndex(p => new { p.IdentificationNo, p.Status })
              .IsUnique();

            modelBuilder.Entity<FireExtinguisherHeader>()
             .HasIndex(p => new { p.AreaId, p.CreatedAt, p.Status })
             .IsUnique();

            modelBuilder.Entity<EmergencyLightHeader>()
             .HasIndex(p => new { p.AreaId, p.CreatedAt, p.Status })
             .IsUnique();

            modelBuilder.Entity<InergenTankHeader>()
            .HasIndex(p => new { p.AreaId, p.CreatedAt, p.Status })
            .IsUnique();

            modelBuilder.Entity<FireHydrantHeader>()
           .HasIndex(p => new { p.AreaId, p.CreatedAt, p.Status })
           .IsUnique();

            modelBuilder.Entity<LocationItemDetail>()
             .HasIndex(p => new { p.ItemId, p.Status })
             .IsUnique();



          

            modelBuilder.Entity<Company>().HasData(
               new { ID = 1, Code = "SLPGC", Name = "Southwest Luzon Power Gen Corporation", Status = "Active" },
               new { ID = 2,Code= "SCPC", Name = "Sem-Calaca Power Corporation", Status = "Active" }
           );

            modelBuilder.Entity<Department>().HasData(
               new { ID = 1, Code = "NA", Name = "NOTSET", Status = "Deleted", CompanyId = 1 }
           );
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
