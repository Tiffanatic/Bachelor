﻿using System;
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

        public DbSet<Price>Prices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<AssignmentType> AssignmentTypes { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<AddressAggregate> AddressAggregates { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<City>()
                .HasOne<Country>(c => c.Country)
                .WithMany(c => c.Cities)
                .HasForeignKey(c => c.CountryId);

            builder.Entity<AddressAggregate>()
                .HasOne<Country>(a => a.Country)
                .WithMany(c => c.AddressAggregates)
                .HasForeignKey(c => c.CountryId);
            
            builder.Entity<AddressAggregate>()
                .HasOne<City>(a => a.City)
                .WithMany(c => c.AddressAggregates)
                .HasForeignKey(c => c.CityId);

            builder.Entity<Customer>()
                .HasOne<AddressAggregate>(c => c.Address)
                .WithOne(c => c.Customer)
                .HasForeignKey<AddressAggregate>(a => a.CustomerId);

            builder.Entity<Customer>()
                .HasOne<CompanyType>(c => c.CompanyType)
                .WithMany(ct => ct.Customers)
                .HasForeignKey(c => c.CompanyTypeId);

            builder.Entity<CustomerContact>().HasKey(cc => new {cc.ContactId, cc.CustomerId});

            builder.Entity<AssignmentType>()
                .HasMany<Assignment>(at => at.Assignments)
                .WithOne(a => a.AssignmentType)
                .HasForeignKey(a => a.AssignmentTypeId);
            
            builder.Entity<Customer>()
                .HasMany<Assignment>(a => a.Assignments)
                .WithOne(c => c.Customer)
                .HasForeignKey(a => a.CustomerId);


            builder.Entity<Price>()
                .HasOne<AssignmentType>()
                .WithMany(at => at.Prices)
                .HasForeignKey(a => a.AssignmentId);

            builder.Entity<User>()
                .HasMany<Price>(a => a.Prices)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            builder.Entity<User>()
                .HasMany<Assignment>(a => a.Assignments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            builder.Entity<TimeRecord>()
                .HasOne<Assignment>(a => a.Assignment)
                .WithMany(a => a.TimeRecords)
                .HasForeignKey(a => a.AssignmentId);

            base.OnModelCreating(builder);
        }
    }
}