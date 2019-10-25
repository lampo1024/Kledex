﻿using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.EventSourcing.Reporting.Data
{
    public class ReportingDbContext : DbContext
    {
        public ReportingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductEntity>().ToTable("Products");
        }

        public DbSet<ProductEntity> Products { get; set; }
    }
}
