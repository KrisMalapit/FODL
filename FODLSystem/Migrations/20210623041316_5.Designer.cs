// <auto-generated />
using System;
using FODLSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FODLSystem.Migrations
{
    [DbContext(typeof(FODLSystemContext))]
    [Migration("20210623041316_5")]
    partial class _5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FODLSystem.Models.Area", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("List")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("No")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("OfficeCode")
                        .IsRequired()
                        .HasColumnType("VARCHAR(10)");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("FODLSystem.Models.Company", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Status");

                    b.HasKey("ID");

                    b.ToTable("Companies");

                    b.HasData(
                        new { ID = 1, Code = "SMPC", Name = "Semirara Mining and Power Corporation", Status = "Active" }
                    );
                });

            modelBuilder.Entity("FODLSystem.Models.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.HasKey("Id");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("FODLSystem.Models.Department", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("CompanyId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Status");

                    b.HasKey("ID");

                    b.HasIndex("CompanyId");

                    b.ToTable("Departments");

                    b.HasData(
                        new { ID = 1, Code = "NA", CompanyId = 1, Name = "NOTSET", Status = "Deleted" }
                    );
                });

            modelBuilder.Entity("FODLSystem.Models.Dispenser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("No")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Dispensers");
                });

            modelBuilder.Entity("FODLSystem.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ModelNo");

                    b.Property<string>("Name");

                    b.Property<string>("No")
                        .IsRequired()
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("FODLSystem.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Description2");

                    b.Property<string>("DescriptionLiquidation");

                    b.Property<string>("No")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("Status");

                    b.Property<string>("TypeFuel");

                    b.HasKey("Id");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("FODLSystem.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Action");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Descriptions");

                    b.Property<string>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("FODLSystem.Models.LubeTruck", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("No")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("OldId");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("LubeTrucks");
                });

            modelBuilder.Entity("FODLSystem.Models.NoSeries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateUpdated");

                    b.Property<string>("LastNoUsed");

                    b.HasKey("Id");

                    b.ToTable("NoSeries");
                });

            modelBuilder.Entity("FODLSystem.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new { Id = 1, Name = "Admin", Status = "Active" },
                        new { Id = 2, Name = "User", Status = "Active" }
                    );
                });

            modelBuilder.Entity("FODLSystem.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CompanyAccess");

                    b.Property<int>("DepartmentId");

                    b.Property<string>("Domain");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("RoleId");

                    b.Property<string>("Status");

                    b.Property<string>("UserType");

                    b.Property<string>("Username")
                        .HasColumnType("VARCHAR(50)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("RoleId");

                    b.HasIndex("Username", "Status")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL AND [Status] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new { Id = 1, CompanyAccess = "1", DepartmentId = 1, Domain = "SMCDACON", Email = "kcmalapit@semirarampc.com", FirstName = "Kristoffer", LastName = "Malapit", Name = "Kristoffer Malapit", Password = "", RoleId = 1, Status = "1", Username = "kcmalapit" }
                    );
                });

            modelBuilder.Entity("FODLSystem.Models.Department", b =>
                {
                    b.HasOne("FODLSystem.Models.Company", "Companies")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FODLSystem.Models.User", b =>
                {
                    b.HasOne("FODLSystem.Models.Department", "Departments")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FODLSystem.Models.Role", "Roles")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
