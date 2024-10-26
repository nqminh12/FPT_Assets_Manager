using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

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
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string? GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            return configuration["ConnectionStrings:AssetManagerDB"];
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
                    .HasConstraintName("FK__Assets__Category__47DBAE45");

                entity.HasOne(d => d.Cloud)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.CloudId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Assets__CloudID__48CFD27E");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Assets__FolderID__49C3F6B7");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Assets)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__Assets__TypeID__46E78A0C");
            });

            modelBuilder.Entity<AssetLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__AssetLog__5E5499A86B0556E0");

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
                    .HasConstraintName("FK__AssetLogs__Asset__4D94879B");
            });

            modelBuilder.Entity<AssetPermission>(entity =>
            {
                entity.HasKey(e => e.PermissionId)
                    .HasName("PK__AssetPer__EFA6FB0F3AE2B35E");

                entity.Property(e => e.PermissionId).HasColumnName("PermissionID");

                entity.Property(e => e.AssetId).HasColumnName("AssetID");

                entity.Property(e => e.CanDelete).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanEdit).HasDefaultValueSql("((0))");

                entity.Property(e => e.CanView).HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.AssetPermissions)
                    .HasForeignKey(d => d.AssetId)
                    .HasConstraintName("FK__AssetPerm__Asset__5441852A");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AssetPermissions)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__AssetPerm__RoleI__534D60F1");
            });

            modelBuilder.Entity<AssetType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__AssetTyp__516F0395D348E334");

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
                    .HasName("PK__CloudSto__70A7EB8DD16F30B1");

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
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Path).HasMaxLength(255);

                entity.Property(e => e.ProjectName).HasMaxLength(100);

                entity.HasMany(d => d.Folders)
                    .WithMany(p => p.Projects)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProjectFolder",
                        l => l.HasOne<Folder>().WithMany().HasForeignKey("FolderId").HasConstraintName("FK__ProjectFo__Folde__3E52440B"),
                        r => r.HasOne<Project>().WithMany().HasForeignKey("ProjectId").HasConstraintName("FK__ProjectFo__Proje__3D5E1FD2"),
                        j =>
                        {
                            j.HasKey("ProjectId", "FolderId").HasName("PK__ProjectF__9CD7CFD953C8BA0C");

                            j.ToTable("ProjectFolders");

                            j.IndexerProperty<int>("ProjectId").HasColumnName("ProjectID");

                            j.IndexerProperty<int>("FolderId").HasColumnName("FolderID");
                        });
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__UserRole__8AFACE3AB2056921");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
