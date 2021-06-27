﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FODLSystem.Models;

namespace FODLSystem.Models
{
    public class FODLSystemContext : DbContext
    {
        public FODLSystemContext(DbContextOptions<FODLSystemContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<NoSeries> NoSeries { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<LubeTruck> LubeTrucks { get; set; }
        public DbSet<Dispenser> Dispensers { get; set; }
        public DbSet<Location> Locations { get; set; }


        public DbSet<FuelOil> FuelOils { get; set; }
        public DbSet<FuelOilDetail> FuelOilDetails { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasIndex(p => new { p.Username, p.Status })
               .IsUnique();

            modelBuilder.Entity<Company>().HasData(
               new { ID = 1, Code = "SMPC", Name = "Semirara Mining and Power Corporation", Status = "Active" }
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
