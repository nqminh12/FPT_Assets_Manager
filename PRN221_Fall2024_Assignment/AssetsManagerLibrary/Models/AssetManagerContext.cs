using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AssetsManagerLibrary.Models
{
    public partial class AssetManagerContext : DbContext
    {
        public AssetManagerContext()
        {
        }

        public AssetManagerContext(DbContextOptions<AssetManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asset> Assets { get; set; } = null!;
        public virtual DbSet<AssetLog> AssetLogs { get; set; } = null!;
        public virtual DbSet<AssetPermission> AssetPermissions { get; set; } = null!;
        public virtual DbSet<AssetType> AssetTypes { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CloudStorage> CloudStorages { get; set; } = null!;
        public virtual DbSet<Folder> Folders { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;database=AssetManager;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.AssetName).HasMaxLength(100);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CloudId).HasColumnName("CloudID");

                entity.Property(e => e.FilePath).HasMaxLength(255);

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.ImportedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SizeKb).HasColumnName("SizeKB");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Assets__Category__45F365D3");

                entity.HasOne(d => d.Cloud)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.CloudId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Assets__CloudID__46E78A0C");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Assets__FolderID__440B1D61");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__Assets__TypeID__44FF419A");
            });

            modelBuilder.Entity<AssetLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__AssetLog__5E5499A84A1D2905");

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.ActionDetails).HasMaxLength(255);

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.NewPath).HasMaxLength(255);

                entity.Property(e => e.OldPath).HasMaxLength(255);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.AssetLogs)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__AssetLogs__Asset__4AB81AF0");
            });

            modelBuilder.Entity<AssetPermission>(entity =>
            {
                entity.HasKey(e => e.PermissionId)
                    .HasName("PK__AssetPer__EFA6FB0FB311F562");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.CanDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanEdit).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanView).HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.AssetPermissions)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("FK__AssetPerm__Asset__5165187F");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AssetPermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__AssetPerm__RoleI__5070F446");
            });

            modelBuilder.Entity<AssetType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__AssetTyp__516F0395775471F8");

                entity.ToTable("AssetType");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.TypeDescription).HasMaxLength(10);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<CloudStorage>(entity =>
            {
                entity.HasKey(e => e.CloudId)
                    .HasName("PK__CloudSto__70A7EB8DA77AA8CD");

                entity.ToTable("CloudStorage");

                entity.Property(e => e.CloudId).HasColumnName("CloudID");

                entity.Property(e => e.AccessToken).HasMaxLength(255);

                entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

                entity.Property(e => e.Provider).HasMaxLength(50);

                entity.Property(e => e.RefreshToken).HasMaxLength(255);
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.AssetType).HasMaxLength(50);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FolderName).HasMaxLength(100);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Folders__Project__3A81B327");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Path).HasMaxLength(255);

                entity.Property(e => e.ProjectName).HasMaxLength(100);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__UserRole__8AFACE3AE0C46649");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
