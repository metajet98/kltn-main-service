using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace main_service.Databases
{
    public partial class AppDBContext : DbContext
    {
        public AppDBContext()
        {
        }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<BranchServicePrice> BranchServicePrice { get; set; }
        public virtual DbSet<BranchStaff> BranchStaff { get; set; }
        public virtual DbSet<FcmToken> FcmToken { get; set; }
        public virtual DbSet<Maintenance> Maintenance { get; set; }
        public virtual DbSet<MaintenanceBillDetail> MaintenanceBillDetail { get; set; }
        public virtual DbSet<MaintenanceSchedule> MaintenanceSchedule { get; set; }
        public virtual DbSet<MaintenanceService> MaintenanceService { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<SparepartCheckDetail> SparepartCheckDetail { get; set; }
        public virtual DbSet<SparepartStatus> SparepartStatus { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAuth> UserAuth { get; set; }
        public virtual DbSet<UserVehicle> UserVehicle { get; set; }
        public virtual DbSet<VehicleCompany> VehicleCompany { get; set; }
        public virtual DbSet<VehicleGroup> VehicleGroup { get; set; }
        public virtual DbSet<VehicleGroupSparepartItem> VehicleGroupSparepartItem { get; set; }
        public virtual DbSet<VehicleType> VehicleType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=db-maintenance-system.czxyq53fjurd.ap-southeast-1.rds.amazonaws.com,1433;database=db-maintenance-system;User ID=admin;password=MYKMFiMEnyGkChNGE9Cr;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("BRANCH_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("BRANCH_Id_uindex")
                    .IsUnique();
            });

            modelBuilder.Entity<BranchServicePrice>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("BRANCH_MAINTENANCE_ITEM_PRICE_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("BRANCH_MAINTENANCE_ITEM_PRICE_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.BranchServicePrice)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BRANCH_MAINTENANCE_ITEM_PRICE_BRANCH_Id_fk");

                entity.HasOne(d => d.MaintenanceService)
                    .WithMany(p => p.BranchServicePrice)
                    .HasForeignKey(d => d.MaintenanceServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BRANCH_MAINTENANCE_ITEM_PRICE_MAINTENANCE_ITEM_Id_fk");
            });

            modelBuilder.Entity<BranchStaff>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("BRANCH_STAFF_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("BRANCH_STAFF_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.BranchStaff)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BRANCH_STAFF_BRANCH_Id_fk");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.BranchStaff)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("BRANCH_STAFF_USER_Id_fk");
            });

            modelBuilder.Entity<FcmToken>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("FCM_TOKEN_pk")
                    .IsClustered(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FcmToken)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FCM_TOKEN_USER_Id_fk");
            });

            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("MAINTENANCE_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("MAINTENANCE_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Maintenance)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_BRANCH_Id_fk");

                entity.HasOne(d => d.MaintenanceStaff)
                    .WithMany(p => p.MaintenanceMaintenanceStaff)
                    .HasForeignKey(d => d.MaintenanceStaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_USER_Id_fk");

                entity.HasOne(d => d.ReceptionStaff)
                    .WithMany(p => p.MaintenanceReceptionStaff)
                    .HasForeignKey(d => d.ReceptionStaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_USER_Id_fk_2");

                entity.HasOne(d => d.UserVehicle)
                    .WithMany(p => p.Maintenance)
                    .HasForeignKey(d => d.UserVehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_USER_VEHICLE_Id_fk");
            });

            modelBuilder.Entity<MaintenanceBillDetail>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("MAINTENANCE_BILL_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("MAINTENANCE_BILL_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.BranchServicePrice)
                    .WithMany(p => p.MaintenanceBillDetail)
                    .HasForeignKey(d => d.BranchServicePriceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_BILL_BRANCH_MAINTENANCE_ITEM_PRICE_Id_fk");

                entity.HasOne(d => d.Maintenance)
                    .WithMany(p => p.MaintenanceBillDetail)
                    .HasForeignKey(d => d.MaintenanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_BILL_MAINTENANCE_Id_fk");
            });

            modelBuilder.Entity<MaintenanceSchedule>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("MAINTENANCE_SCHEDULE_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("MAINTENANCE_SCHEDULE_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.UserVehicle)
                    .WithMany(p => p.MaintenanceSchedule)
                    .HasForeignKey(d => d.UserVehicleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_SCHEDULE_USER_VEHICLE_Id_fk");
            });

            modelBuilder.Entity<MaintenanceService>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("MAINTENANCE_ITEM_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("MAINTENANCE_ITEM_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.VehicleGroup)
                    .WithMany(p => p.MaintenanceService)
                    .HasForeignKey(d => d.VehicleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_ITEM_VEHICLE_GROUP_Id_fk");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("NOTIFICATION_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("NOTIFICATION_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("NOTIFICATION_USER_Id_fk");
            });

            modelBuilder.Entity<SparepartCheckDetail>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("MAINTENANCE_CHECK_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("MAINTENANCE_CHECK_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.Maintenance)
                    .WithMany(p => p.SparepartCheckDetail)
                    .HasForeignKey(d => d.MaintenanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_CHECK_MAINTENANCE_Id_fk");

                entity.HasOne(d => d.SparePartItem)
                    .WithMany(p => p.SparepartCheckDetail)
                    .HasForeignKey(d => d.SparePartItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_CHECK_VEHICLE_GROUP_SPAREPART_ITEM_Id_fk");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.SparepartCheckDetail)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MAINTENANCE_CHECK_SPAREPART_STATUS_Id_fk");
            });

            modelBuilder.Entity<SparepartStatus>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("SPAREPART_STATUS_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("SPAREPART_STATUS_Id_uindex")
                    .IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("USER_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Email)
                    .HasName("USER_Email_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.PhoneNumber)
                    .HasName("USER_PhoneNumber_uindex")
                    .IsUnique();
            });

            modelBuilder.Entity<UserAuth>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("USER_AUTH_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.UserId)
                    .HasName("USER_AUTH_UserId_uindex")
                    .IsUnique();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserAuth)
                    .HasForeignKey<UserAuth>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USER_AUTH_USER_Id_fk");
            });

            modelBuilder.Entity<UserVehicle>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("USER_VEHICLE_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("USER_VEHICLE_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserVehicle)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USER_VEHICLE_USER_Id_fk");

                entity.HasOne(d => d.VehicleGroup)
                    .WithMany(p => p.UserVehicle)
                    .HasForeignKey(d => d.VehicleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("USER_VEHICLE_VEHICLE_GROUP_Id_fk");
            });

            modelBuilder.Entity<VehicleCompany>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("VEHICLE_COMPANY_pk")
                    .IsClustered(false);
            });

            modelBuilder.Entity<VehicleGroup>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("VEHICLE_GROUP_pk")
                    .IsClustered(false);

                entity.HasOne(d => d.VehicleCompany)
                    .WithMany(p => p.VehicleGroup)
                    .HasForeignKey(d => d.VehicleCompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VEHICLE_GROUP_VEHICLE_COMPANY_Id_fk");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.VehicleGroup)
                    .HasForeignKey(d => d.VehicleTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("VEHICLE_GROUP_VEHICLE_TYPE_Id_fk");
            });

            modelBuilder.Entity<VehicleGroupSparepartItem>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("CHECKING_ITEM_pk")
                    .IsClustered(false);

                entity.HasIndex(e => e.Id)
                    .HasName("CHECKING_ITEM_Id_uindex")
                    .IsUnique();

                entity.HasOne(d => d.VehicleGroup)
                    .WithMany(p => p.VehicleGroupSparepartItem)
                    .HasForeignKey(d => d.VehicleGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("CHECKING_ITEM_VEHICLE_GROUP_Id_fk");
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("VEHICLE_TYPE_pk")
                    .IsClustered(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
