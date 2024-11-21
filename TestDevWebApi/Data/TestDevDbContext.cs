using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestDevWebApi.Models;

namespace TestDevWebApi.Data;

public partial class TestDevDbContext : DbContext
{
    public TestDevDbContext()
    {
    }

    public TestDevDbContext(DbContextOptions<TestDevDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.Property(e => e.AccNumber).IsFixedLength();
            entity.Property(e => e.AccParent).IsFixedLength();

            entity.HasOne(d => d.AccParentNavigation).WithMany(p => p.InverseAccParentNavigation).HasConstraintName("FK_Accounts_Accounts");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
