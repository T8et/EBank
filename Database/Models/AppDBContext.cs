using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Database.Models;

public partial class AppDBContext : DbContext
{
    public AppDBContext()
    {
    }

    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BtAcc> BtAccs { get; set; }

    public virtual DbSet<BtTran> BtTrans { get; set; }

    public virtual DbSet<BtUser> BtUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=MeBank;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BtAcc>(entity =>
        {
            entity.HasKey(e => e.AccId);

            entity.ToTable("BT_Acc");

            entity.Property(e => e.AccId).HasColumnName("Acc_Id");
            entity.Property(e => e.AccBalance).HasColumnName("Acc_Balance");
            entity.Property(e => e.AccPin)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("Acc_Pin");
            entity.Property(e => e.UserId).HasColumnName("User_Id");
        });

        modelBuilder.Entity<BtTran>(entity =>
        {
            entity.HasKey(e => e.TranId);

            entity.ToTable("BT_Tran");

            entity.Property(e => e.TranId)
                .HasMaxLength(20)
                .HasColumnName("Tran_Id");
            entity.Property(e => e.TranDate)
                .HasColumnType("datetime")
                .HasColumnName("Tran_Date");
            entity.Property(e => e.TranFrAccId).HasColumnName("Tran_Fr_Acc_Id");
            entity.Property(e => e.TranSts).HasColumnName("Tran_Sts");
            entity.Property(e => e.TranToAccId).HasColumnName("Tran_To_Acc_Id");
        });

        modelBuilder.Entity<BtUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("BT_User");

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.UserAddress)
                .HasMaxLength(300)
                .HasColumnName("User_Address");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(20)
                .HasColumnName("User_Email");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .HasColumnName("User_Name");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(15)
                .HasColumnName("User_Phone");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
