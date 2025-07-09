using APIKlinik.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKlinik.Infrastructure.Data
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<MenuRole> MenuRoles { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<UserShift> UserShifts { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<FaceData> FaceDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                            v => v.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(v, DateTimeKind.Utc) : v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }

            // Konfigurasi tabel users
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(100);
                entity.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.LastLogin).HasColumnName("last_login");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Konfigurasi tabel roles
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Konfigurasi tabel menus
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("menus", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.ParentId).HasColumnName("parent_id");
                entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(50);
                entity.Property(e => e.Path).HasColumnName("path").HasMaxLength(100);
                entity.Property(e => e.OrderNum).HasColumnName("order_num").HasDefaultValue(0);
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(m => m.Parent)
                    .WithMany(m => m.Children)
                    .HasForeignKey(m => m.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Konfigurasi tabel user_roles
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_roles", "public");
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Konfigurasi tabel menu_roles
            modelBuilder.Entity<MenuRole>(entity =>
            {
                entity.ToTable("menu_roles", "public");
                entity.HasKey(e => new { e.MenuId, e.RoleId });
                entity.Property(e => e.MenuId).HasColumnName("menu_id");
                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(mr => mr.Menu)
                    .WithMany(m => m.MenuRoles)
                    .HasForeignKey(mr => mr.MenuId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(mr => mr.Role)
                    .WithMany(r => r.MenuRoles)
                    .HasForeignKey(mr => mr.RoleId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // Konfigurasi tabel attendances
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.ToTable("attendances", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.Date).HasColumnName("date").IsRequired();
                entity.Property(e => e.CheckinTime).HasColumnName("checkin_time");
                entity.Property(e => e.CheckoutTime).HasColumnName("checkout_time");
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Latitude).HasColumnName("latitude").HasColumnType("decimal(9,6)");
                entity.Property(e => e.Longitude).HasColumnName("longitude").HasColumnType("decimal(9,6)");
                entity.Property(e => e.FaceImage).HasColumnName("face_image");
                entity.Property(e => e.IsLate).HasColumnName("is_late").HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(a => a.User)
                    .WithMany()
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.Date }).IsUnique();
            });

            // Konfigurasi tabel attendance_logs
            modelBuilder.Entity<AttendanceLog>(entity =>
            {
                entity.ToTable("attendance_logs", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.Action).HasColumnName("action").HasMaxLength(10).IsRequired();
                entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(10).IsRequired();
                entity.Property(e => e.Reason).HasColumnName("reason");
                entity.Property(e => e.Latitude).HasColumnName("latitude").HasColumnType("decimal(9,6)");
                entity.Property(e => e.Longitude).HasColumnName("longitude").HasColumnType("decimal(9,6)");
                entity.Property(e => e.FaceImage).HasColumnName("face_image");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasOne(al => al.User)
                    .WithMany()
                    .HasForeignKey(al => al.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.ToTable("shifts", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
                entity.Property(e => e.StartTime).HasColumnName("start_time").IsRequired();
                entity.Property(e => e.EndTime).HasColumnName("end_time").IsRequired();
                entity.Property(e => e.LateToleranceMinutes).HasColumnName("late_tolerance_minutes").HasDefaultValue(15);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<UserShift>(entity =>
            {
                entity.ToTable("user_shifts", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.ShiftId).HasColumnName("shift_id").IsRequired();
                entity.Property(e => e.Date).HasColumnName("date");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(us => us.User)
                    .WithMany()
                    .HasForeignKey(us => us.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(us => us.Shift)
                    .WithMany()
                    .HasForeignKey(us => us.ShiftId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.Date }).IsUnique();
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("locations", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100);
                entity.Property(e => e.Latitude).HasColumnName("latitude").HasColumnType("decimal(9,6)").IsRequired();
                entity.Property(e => e.Longitude).HasColumnName("longitude").HasColumnType("decimal(9,6)").IsRequired();
                entity.Property(e => e.RadiusInMeters).HasColumnName("radius_in_meters").HasDefaultValue(50);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<FaceData>(entity =>
            {
                entity.ToTable("face_data", "public");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
                entity.Property(e => e.FaceEmbedding).HasColumnName("face_embedding");
                entity.Property(e => e.FaceImage).HasColumnName("face_image");
                entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                // Relasi dengan tabel users
                entity.HasOne(fd => fd.User)
                    .WithMany()
                    .HasForeignKey(fd => fd.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Constraint UNIQUE untuk user_id
                entity.HasIndex(e => e.UserId).IsUnique();
            });
        }
    }
}