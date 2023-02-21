﻿// <auto-generated />
using System;
using BurzaFirem2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BurzaFirem2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230209084528_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ActivityCompany", b =>
                {
                    b.Property<int>("ActivitiesActivityId")
                        .HasColumnType("int");

                    b.Property<int>("CompaniesCompanyId")
                        .HasColumnType("int");

                    b.HasKey("ActivitiesActivityId", "CompaniesCompanyId");

                    b.HasIndex("CompaniesCompanyId");

                    b.ToTable("ActivityCompany");
                });

            modelBuilder.Entity("BranchCompany", b =>
                {
                    b.Property<int>("BranchesBranchId")
                        .HasColumnType("int");

                    b.Property<int>("CompaniesCompanyId")
                        .HasColumnType("int");

                    b.HasKey("BranchesBranchId", "CompaniesCompanyId");

                    b.HasIndex("CompaniesCompanyId");

                    b.ToTable("BranchCompany");
                });

            modelBuilder.Entity("BurzaFirem2.Models.Activity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ActivityId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Visible")
                        .HasColumnType("bit");

                    b.HasKey("ActivityId");

                    b.ToTable("Activities");

                    b.HasData(
                        new
                        {
                            ActivityId = 1,
                            Name = "Exkurze",
                            Visible = true
                        },
                        new
                        {
                            ActivityId = 2,
                            Name = "Čtrnáctidenní praxe",
                            Visible = true
                        },
                        new
                        {
                            ActivityId = 3,
                            Name = "Dlouhodobá praxe",
                            Visible = true
                        },
                        new
                        {
                            ActivityId = 4,
                            Name = "Brigáda",
                            Visible = true
                        },
                        new
                        {
                            ActivityId = 5,
                            Name = "Zaměstnání",
                            Visible = true
                        });
                });

            modelBuilder.Entity("BurzaFirem2.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "3e20bada-4052-450d-b8df-525decb0b846",
                            Created = new DateTime(2023, 2, 9, 9, 45, 27, 455, DateTimeKind.Local).AddTicks(7198),
                            Email = "jobs@pslib.cz",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "JOBS@PSLIB.CZ",
                            NormalizedUserName = "JOBS@PSLIB.CZ",
                            PasswordHash = "AQAAAAEAACcQAAAAEMe6W2+2AazsiZh/Pyd85UtNem0UqM7AYXAKeSKEwOl+frclVmUwX+vRZLUTHaHMdQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "G56SBMMYFYXDNGIMOS5RMZUDSTQ4BQHI",
                            TwoFactorEnabled = false,
                            Updated = new DateTime(2023, 2, 9, 9, 45, 27, 455, DateTimeKind.Local).AddTicks(7242),
                            UserName = "jobs@pslib.cz"
                        });
                });

            modelBuilder.Entity("BurzaFirem2.Models.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BranchId"), 1L, 1);

                    b.Property<string>("BackgroundColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TextColor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Visible")
                        .HasColumnType("bit");

                    b.HasKey("BranchId");

                    b.ToTable("Branches");

                    b.HasData(
                        new
                        {
                            BranchId = 1,
                            BackgroundColor = "#D90000",
                            Name = "IT",
                            TextColor = "#FFFFFF",
                            Visible = true
                        },
                        new
                        {
                            BranchId = 2,
                            BackgroundColor = "#357BC2",
                            Name = "Strojírenství",
                            TextColor = "#FFFFFF",
                            Visible = true
                        },
                        new
                        {
                            BranchId = 3,
                            BackgroundColor = "#00AA80",
                            Name = "Elektrotechnika",
                            TextColor = "#FFFFFF",
                            Visible = true
                        },
                        new
                        {
                            BranchId = 4,
                            BackgroundColor = "#ECB100",
                            Name = "Lyceum",
                            TextColor = "#FFFFFF",
                            Visible = true
                        });
                });

            modelBuilder.Entity("BurzaFirem2.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"), 1L, 1);

                    b.Property<string>("AddressStreet")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyBranches")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LogoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Municipality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Offer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PresentationUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Wanted")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyId");

                    b.HasIndex("LogoId")
                        .IsUnique()
                        .HasFilter("[LogoId] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("Companies");

                    b.HasData(
                        new
                        {
                            CompanyId = 1,
                            AddressStreet = "U Jezu 525/4",
                            CompanyBranches = "",
                            CompanyUrl = "https://www.hardwario.com/",
                            Created = new DateTime(2023, 2, 9, 9, 45, 27, 458, DateTimeKind.Local).AddTicks(7013),
                            Description = "",
                            Municipality = "Liberec 460 01",
                            Name = "HARDWARIO s.r.o.",
                            Offer = "<p>Naším hlavním benefitem je to, že se vám u nás rozvinou vaše znalosti elektroniky a vývoje software a firmware, a to prací na reálných projektech.</p>",
                            PresentationUrl = "",
                            Updated = new DateTime(2023, 2, 9, 9, 45, 27, 458, DateTimeKind.Local).AddTicks(7018),
                            UserId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Wanted = "<p>Kluky i holky, kteří se chtějí podílet vývoji IoT řešení, která se uplatňují na celém světě. U nás nebudete uklízet a třídit, ale budete pracovat na skutečném vývoji a výrobě elektroniky.</p>"
                        });
                });

            modelBuilder.Entity("BurzaFirem2.Models.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContactId"), 1L, 1);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContactId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("BurzaFirem2.Models.Listing", b =>
                {
                    b.Property<int>("ListingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ListingId"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Visible")
                        .HasColumnType("bit");

                    b.HasKey("ListingId");

                    b.ToTable("Listing");

                    b.HasData(
                        new
                        {
                            ListingId = 1,
                            Created = new DateTime(2023, 2, 9, 9, 45, 27, 458, DateTimeKind.Local).AddTicks(6936),
                            Name = "2022",
                            Visible = true
                        },
                        new
                        {
                            ListingId = 2,
                            Created = new DateTime(2023, 2, 9, 9, 45, 27, 458, DateTimeKind.Local).AddTicks(6955),
                            Name = "2023",
                            Visible = true
                        });
                });

            modelBuilder.Entity("BurzaFirem2.Models.StoredImage", b =>
                {
                    b.Property<Guid>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("CompanyLogoId")
                        .HasColumnType("int");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UploaderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("ImageId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("UploaderId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("BurzaFirem2.Models.Thumbnail", b =>
                {
                    b.Property<Guid>("ImageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<byte[]>("Blob")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("FileId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ImageId", "Type");

                    b.HasIndex("FileId");

                    b.ToTable("Thumbnails");
                });

            modelBuilder.Entity("CompanyListing", b =>
                {
                    b.Property<int>("CompaniesCompanyId")
                        .HasColumnType("int");

                    b.Property<int>("ListingsListingId")
                        .HasColumnType("int");

                    b.HasKey("CompaniesCompanyId", "ListingsListingId");

                    b.HasIndex("ListingsListingId");

                    b.ToTable("CompanyListing");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111110000"),
                            ConcurrencyStamp = "4a83271a-c404-45f1-8323-a1a3e1127011",
                            Name = "Administrátor",
                            NormalizedName = "ADMINISTRÁTOR"
                        },
                        new
                        {
                            Id = new Guid("22222222-2222-2222-2222-222222220000"),
                            ConcurrencyStamp = "008764f3-26eb-43e4-b401-de71757388b5",
                            Name = "Editor",
                            NormalizedName = "EDITOR"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClaimType = "admin",
                            ClaimValue = "1",
                            RoleId = new Guid("11111111-1111-1111-1111-111111110000")
                        },
                        new
                        {
                            Id = 2,
                            ClaimType = "editor",
                            ClaimValue = "1",
                            RoleId = new Guid("22222222-2222-2222-2222-222222220000")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            RoleId = new Guid("11111111-1111-1111-1111-111111110000"),
                            UserId = new Guid("11111111-1111-1111-1111-111111111111")
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ActivityCompany", b =>
                {
                    b.HasOne("BurzaFirem2.Models.Activity", null)
                        .WithMany()
                        .HasForeignKey("ActivitiesActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BurzaFirem2.Models.Company", null)
                        .WithMany()
                        .HasForeignKey("CompaniesCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BranchCompany", b =>
                {
                    b.HasOne("BurzaFirem2.Models.Branch", null)
                        .WithMany()
                        .HasForeignKey("BranchesBranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BurzaFirem2.Models.Company", null)
                        .WithMany()
                        .HasForeignKey("CompaniesCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BurzaFirem2.Models.Company", b =>
                {
                    b.HasOne("BurzaFirem2.Models.StoredImage", "Logo")
                        .WithOne("CompanyLogo")
                        .HasForeignKey("BurzaFirem2.Models.Company", "LogoId");

                    b.HasOne("BurzaFirem2.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Logo");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BurzaFirem2.Models.Contact", b =>
                {
                    b.HasOne("BurzaFirem2.Models.Company", "Company")
                        .WithMany("Contacts")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("BurzaFirem2.Models.StoredImage", b =>
                {
                    b.HasOne("BurzaFirem2.Models.Company", "Company")
                        .WithMany("Images")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BurzaFirem2.Models.ApplicationUser", "Uploader")
                        .WithMany()
                        .HasForeignKey("UploaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("Uploader");
                });

            modelBuilder.Entity("BurzaFirem2.Models.Thumbnail", b =>
                {
                    b.HasOne("BurzaFirem2.Models.StoredImage", "Image")
                        .WithMany("Thumbnails")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");
                });

            modelBuilder.Entity("CompanyListing", b =>
                {
                    b.HasOne("BurzaFirem2.Models.Company", null)
                        .WithMany()
                        .HasForeignKey("CompaniesCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BurzaFirem2.Models.Listing", null)
                        .WithMany()
                        .HasForeignKey("ListingsListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("BurzaFirem2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("BurzaFirem2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BurzaFirem2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("BurzaFirem2.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BurzaFirem2.Models.Company", b =>
                {
                    b.Navigation("Contacts");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("BurzaFirem2.Models.StoredImage", b =>
                {
                    b.Navigation("CompanyLogo");

                    b.Navigation("Thumbnails");
                });
#pragma warning restore 612, 618
        }
    }
}
