﻿using BurzaFirem2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;

namespace BurzaFirem2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        private ILogger<ApplicationDbContext> _logger;

        public DbSet<Company> Companies { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Models.Activity> Activities { get; set; }
        public DbSet<StoredImage> Images { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Listing> Listings { get; set; }

        public ApplicationDbContext(DbContextOptions options, ILogger<ApplicationDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasOne(c => c.Logo).WithOne(i => i.CompanyLogo).HasForeignKey<Company>(c => c.LogoId);
                entity.HasMany(c => c.Images).WithOne(i => i.Company).HasForeignKey(i => i.CompanyId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Branch>().HasData(new Branch { BranchId = 1, Name = "IT", BackgroundColor = "#3cab68", TextColor="#FFFFFF", Visible = true });
            modelBuilder.Entity<Branch>().HasData(new Branch { BranchId = 2, Name = "Strojírenství", BackgroundColor = "#429fe3", TextColor = "#FFFFFF", Visible = true });
            modelBuilder.Entity<Branch>().HasData(new Branch { BranchId = 3, Name = "Elektrotechnika", BackgroundColor = "#e34242", TextColor = "#FFFFFF", Visible = true });
            modelBuilder.Entity<Branch>().HasData(new Branch { BranchId = 4, Name = "Lyceum", BackgroundColor = "#e3a342", TextColor = "#FFFFFF", Visible = true });
            modelBuilder.Entity<Branch>().HasData(new Branch { BranchId = 5, Name = "Oděvnictví", BackgroundColor = "#9c42e3", TextColor = "#FFFFFF", Visible = true });
            modelBuilder.Entity<Branch>().HasData(new Branch { BranchId = 6, Name = "Textilnictví", BackgroundColor = "#e3428f", TextColor = "#FFFFFF", Visible = true });
            modelBuilder.Entity<Models.Activity>().HasData(new Models.Activity { ActivityId = 1, Name = "Exkurze", Visible = true });
            modelBuilder.Entity<Models.Activity>().HasData(new Models.Activity { ActivityId = 2, Name = "Čtrnáctidenní praxe", Visible = true });
            modelBuilder.Entity<Models.Activity>().HasData(new Models.Activity { ActivityId = 3, Name = "Dlouhodobá praxe", Visible = true });
            modelBuilder.Entity<Models.Activity>().HasData(new Models.Activity { ActivityId = 4, Name = "Brigáda", Visible = true });
            modelBuilder.Entity<Models.Activity>().HasData(new Models.Activity { ActivityId = 5, Name = "Zaměstnání", Visible = true });
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.HasData(new IdentityRole<Guid>
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111110000"),
                    Name = "Administrátor",
                    NormalizedName = "ADMINISTRÁTOR"
                });
                entity.HasData(new IdentityRole<Guid>
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222220000"),
                    Name = "Editor",
                    NormalizedName = "EDITOR"
                });
            });
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasData(new ApplicationUser
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Email = "jobs@pslib.cz",
                    NormalizedEmail = "JOBS@PSLIB.CZ",
                    UserName = "jobs@pslib.cz",
                    PasswordHash = hasher.HashPassword(new ApplicationUser(), "Admin_1234"),
                    SecurityStamp = "G56SBMMYFYXDNGIMOS5RMZUDSTQ4BQHI",
                    NormalizedUserName = "JOBS@PSLIB.CZ",
                    EmailConfirmed = true,
                });
            });
            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.HasData(new IdentityRoleClaim<Guid>
                {
                    Id = 1,
                    RoleId = Guid.Parse("11111111-1111-1111-1111-111111110000"),
                    ClaimType = "admin",
                    ClaimValue = "1"
                });
                entity.HasData(new IdentityRoleClaim<Guid>
                {
                    Id = 2,
                    RoleId = Guid.Parse("22222222-2222-2222-2222-222222220000"),
                    ClaimType = "editor",
                    ClaimValue = "1"
                });
                modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
                {
                    entity.HasKey(x => new { x.RoleId, x.UserId });
                    entity.HasData(new IdentityUserRole<Guid>
                    {
                        UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        RoleId = Guid.Parse("11111111-1111-1111-1111-111111110000")
                    });
                });
                modelBuilder.Entity<Listing>(entity =>
                {
                    entity.HasData(new Listing
                    {
                        ListingId = 1,
                        Name = "Virtuálně",
                        Created = DateTime.Now
                    });
                    entity.HasData(new Listing
                    {
                        ListingId = 2,
                        Name = "Prezenčně 2023",
                        Created = DateTime.Now
                    });
                    entity.HasData(new Listing
                    {
                        ListingId = 3,
                        Name = "Prezenčně 2024",
                        Created = DateTime.Now
                    });
                });
                modelBuilder.Entity<Company>(entity =>
                {
                    entity.HasData(new Company
                    {
                        CompanyId = 1,
                        Name = "HARDWARIO s.r.o.",
                        Offer = "<p>Naším hlavním benefitem je to, že se vám u nás rozvinou vaše znalosti elektroniky a vývoje software a firmware, a to prací na reálných projektech.</p>",
                        Wanted = "<p>Kluky i holky, kteří se chtějí podílet vývoji IoT řešení, která se uplatňují na celém světě. U nás nebudete uklízet a třídit, ale budete pracovat na skutečném vývoji a výrobě elektroniky.</p>",
                        AddressStreet = "U Jezu 525/4",
                        Municipality = "Liberec 460 01",
                        CompanyUrl = "https://www.hardwario.com/",
                        UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    });
                });
                modelBuilder.Entity<Thumbnail>(entity =>
                {
                    entity.HasKey(t => new { t.ImageId, t.Type });
                });
            });
        }

        public DbSet<BurzaFirem2.Models.Listing> Listing { get; set; }
    }
}
