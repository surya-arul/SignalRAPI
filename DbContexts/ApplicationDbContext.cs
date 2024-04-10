using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SignalRAPI.Models;

namespace SignalRAPI.DbContexts;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblClaysysEmployee> TblClaysysEmployees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblClaysysEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tblClays__3214EC074DF4A253");

            entity.ToTable("tblClaysysEmployees", tb => tb.HasTrigger("tr_dbo_tblClaysysEmployees_9aee31c0-dbaa-4219-8b66-7c9f658b43ae_Sender"));

            entity.Property(e => e.Department)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
