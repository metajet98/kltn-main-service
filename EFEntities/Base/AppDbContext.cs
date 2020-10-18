using main_service.EFEntities.Branches;
using main_service.EFEntities.Maintenances;
using main_service.EFEntities.Users;
using main_service.EFEntities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace main_service.EFEntities.Base
{
    public class AppDbContext : DbContext
    {
        //User
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAuth> UserAuths { get; set; }
        public virtual DbSet<UserVehicle> UserVehicles { get; set; }
        
        //Vehicles
        public virtual DbSet<VehicleCompany> VehicleCompanies { get; set; }
        public virtual DbSet<VehicleType> VehicleTypes { get; set; }
        public virtual DbSet<VehicleGroup> VehicleGroups { get; set; }
        public virtual DbSet<MaintenanceItem> MaintenanceItems { get; set; }
        public virtual DbSet<CheckingItem> CheckingItems { get; set; }

        //Branches
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<BrandItemPrice> BrandItemPrices { get; set; }
        
        //Maintenance
        public virtual DbSet<Maintenance> Maintenances { get; set; }
        public virtual DbSet<MaintenanceChecking> MaintenanceCheckings { get; set; }
        public virtual DbSet<MaintenanceSchedule> MaintenanceSchedules { get; set; }
        public virtual DbSet<StatusCheck> StatusChecks { get; set; }


        public virtual DbSet<Role> Roles { get; set; }


        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=db-maintenance-system.czxyq53fjurd.ap-southeast-1.rds.amazonaws.com,1433;database=db-maintenance-system;User ID=admin;password=MYKMFiMEnyGkChNGE9Cr;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.RoleName).IsUnique();
                entity.HasData(
                    new Role {Id = 1, RoleName = Constants.Role.Admin},
                    new Role {Id = 2, RoleName = Constants.Role.CenterManager},
                    new Role {Id = 3, RoleName = Constants.Role.StaffDesk},
                    new Role {Id = 4, RoleName = Constants.Role.StaffMaintenance},
                    new Role {Id = 5, RoleName = Constants.Role.User}
                );
            });
            modelBuilder.Entity<UserVehicle>(entity => { entity.HasIndex(e => e.PlateNumber); });
        }
    }
}