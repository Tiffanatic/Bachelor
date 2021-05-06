using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Models.Auth;

namespace RapidTime.Data
{
    public class RapidTimeDbContext : IdentityDbContext<User, Role, Guid>
    {
        public RapidTimeDbContext(DbContextOptions<RapidTimeDbContext> options) : base(options)
        {
            
        }

        public DbSet<PriceEntity>Prices { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ContactEntity> Contacts { get; set; }
        public DbSet<CompanyTypeEntity> CompanyTypes { get; set; }
        public DbSet<AssignmentTypeEntity> AssignmentTypes { get; set; }
        public DbSet<AssignmentEntity> Assignments { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<AddressAggregateEntity> AddressAggregates { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CityEntity>()
                .HasOne<CountryEntity>(c => c.CountryEntity)
                .WithMany(c => c.Cities)
                .HasForeignKey(c => c.CountryId);

            builder.Entity<AddressAggregateEntity>()
                .HasOne<CountryEntity>(a => a.CountryEntity)
                .WithMany(c => c.AddressAggregates)
                .HasForeignKey(c => c.CountryId);
            
            builder.Entity<AddressAggregateEntity>()
                .HasOne<CityEntity>(a => a.CityEntity)
                .WithMany(c => c.AddressAggregates)
                .HasForeignKey(c => c.CityId);

            builder.Entity<CustomerEntity>()
                .HasOne<AddressAggregateEntity>(c => c.Address)
                .WithOne(c => c.CustomerEntity)
                .HasForeignKey<AddressAggregateEntity>(a => a.CustomerId);

            builder.Entity<CustomerEntity>()
                .HasOne<CompanyTypeEntity>(c => c.CompanyTypeEntity)
                .WithMany(ct => ct.Customers)
                .HasForeignKey(c => c.CompanyTypeId);

            builder.Entity<CustomerContact>().HasKey(cc => new {cc.ContactId, cc.CustomerId});

            builder.Entity<AssignmentTypeEntity>()
                .HasMany<AssignmentEntity>(at => at.Assignments)
                .WithOne(a => a.AssignmentTypeEntity)
                .HasForeignKey(a => a.AssignmentTypeId);
            
            builder.Entity<CustomerEntity>()
                .HasMany<AssignmentEntity>(a => a.Assignments)
                .WithOne(c => c.CustomerEntity)
                .HasForeignKey(a => a.CustomerId);


            builder.Entity<PriceEntity>()
                .HasOne<AssignmentTypeEntity>()
                .WithMany(at => at.Prices)
                .HasForeignKey(a => a.AssignmentId);

            builder.Entity<User>()
                .HasMany<PriceEntity>(a => a.Prices)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder.Entity<User>()
                .HasMany<AssignmentEntity>(a => a.Assignments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            builder.Entity<TimeRecordEntity>()
                .HasOne<AssignmentEntity>(a => a.AssignmentEntity)
                .WithMany(a => a.TimeRecords)
                .HasForeignKey(a => a.AssignmentId);

            base.OnModelCreating(builder);
        }
    }
}