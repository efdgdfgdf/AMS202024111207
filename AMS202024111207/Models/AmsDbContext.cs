using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AMS202024111207.Models;

public partial class AmsDbContext : DbContext
{
    public AmsDbContext()
    {
    }

    public AmsDbContext(DbContextOptions<AmsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename=D:\\Lab_ASP_NET\\AMS202024111207\\AMS202024111207\\App_Data\\AmsDB.mdf;Integrated Security=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.AssetId).HasName("PK_dbo.Assets");

            entity.Property(e => e.AssetId).HasColumnName("AssetID");
            entity.Property(e => e.AssetName).HasMaxLength(30);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CustodianId).HasColumnName("CustodianID");
            entity.Property(e => e.ImgName).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PurchaseDate).HasColumnType("date");
            entity.Property(e => e.Specification).HasMaxLength(100);

            entity.HasOne(d => d.Category).WithMany(p => p.Assets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_dbo.Assets_dbo.Categories_CategoryID");

            entity.HasOne(d => d.Custodian).WithMany(p => p.Assets)
                .HasForeignKey(d => d.CustodianId)
                .HasConstraintName("FK_dbo.Assets_dbo.Employee_EmployeeID");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_dbo.Categories");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(30);
            entity.Property(e => e.Description).HasMaxLength(200);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK_dbo.Department");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName).HasMaxLength(50);
            entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.Departments)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("FK_dbo.Department_dbo.Employee_EmployeeID");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK_dbo.Employee");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.UserName, "UK_dbo.Employee_UserName").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.JoinDate).HasColumnType("date");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Qqemail)
                .HasMaxLength(25)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("QQemail");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsFixedLength();

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_dbo.Employee_dbo.Department_DepartmentID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
