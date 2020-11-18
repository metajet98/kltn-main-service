using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("USER")]
    public partial class User
    {
        public User()
        {
            BranchStaff = new HashSet<BranchStaff>();
            FcmToken = new HashSet<FcmToken>();
            MaintenanceMaintenanceStaff = new HashSet<Maintenance>();
            MaintenanceReceptionStaff = new HashSet<Maintenance>();
            Notification = new HashSet<Notification>();
            UserVehicle = new HashSet<UserVehicle>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        public string? Address { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Birthday { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [InverseProperty("User")]
        public virtual UserAuth UserAuth { get; set; }
        [InverseProperty("Staff")]
        public virtual ICollection<BranchStaff> BranchStaff { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<FcmToken> FcmToken { get; set; }
        [InverseProperty(nameof(Maintenance.MaintenanceStaff))]
        public virtual ICollection<Maintenance> MaintenanceMaintenanceStaff { get; set; }
        [InverseProperty(nameof(Maintenance.ReceptionStaff))]
        public virtual ICollection<Maintenance> MaintenanceReceptionStaff { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Notification> Notification { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserVehicle> UserVehicle { get; set; }
    }
}
