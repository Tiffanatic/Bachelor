using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
            builder
                .Entity<TimeRecord>()
                .Property(p => p.TimeRecorded)
                .HasConversion(new TimeSpanToTicksConverter()); // Suggestions to use TimeSpanToTicksConverter taken from: https://stackoverflow.com/questions/17129795/storing-timespan-with-entity-framework-codefirst-sqldbtype-time-overflow and https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations
            
            
            base.OnModelCreating(builder);
        }
    }
}