using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WOT_API;

public partial class WOTCOREContext : DbContext
{
    public WOTCOREContext()
    {
    }

    public WOTCOREContext(DbContextOptions<WOTCOREContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DbUser> DbUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WOTConnection> WotConnections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("dbUSERS");

            entity.Property(e => e.ConnId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("connId");
            entity.Property(e => e.DbName)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("dbName");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("dob");
            entity.Property(e => e.EmailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("emailID");
            entity.Property(e => e.EmpId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("empID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.LoginEnable).HasColumnName("loginEnable");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("middleName");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mobileNo");
            entity.Property(e => e.Passcode)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("passcode");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.UserName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__CB9A1CDF5F3F2825");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("dob");
            entity.Property(e => e.EmailId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("emailID");
            entity.Property(e => e.EmpId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("empID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.LoginEnable)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("loginEnable");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("middleName");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mobileNo");
            entity.Property(e => e.Passcode)
                .HasMaxLength(32)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("passcode");
            entity.Property(e => e.UserName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<WOTConnection>(entity =>
        {
            entity.HasKey(e => e.ConnId).HasName("PK__WOT_Conn__35BC1D8B27EF948E");

            entity.ToTable("WOT_Connection");

            entity.Property(e => e.ConnId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("connID");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasColumnName("active");
            entity.Property(e => e.DbName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dbName");
            entity.Property(e => e.Passcode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("passcode");
            entity.Property(e => e.ServerName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("serverName");
            entity.Property(e => e.UserId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("userId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
