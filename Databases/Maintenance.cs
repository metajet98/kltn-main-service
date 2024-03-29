﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("MAINTENANCE")]
    public partial class Maintenance
    {
        public Maintenance()
        {
            MaintenanceBillDetail = new HashSet<MaintenanceBillDetail>();
            MaintenanceImage = new HashSet<MaintenanceImage>();
            MaintenanceSchedule = new HashSet<MaintenanceSchedule>();
            SparepartCheckDetail = new HashSet<SparepartCheckDetail>();
        }

        [Key]
        public int Id { get; set; }
        public int? UserVehicleId { get; set; }
        public string? Notes { get; set; }
        public int? MaintenanceStaffId { get; set; }
        public int? ReceptionStaffId { get; set; }
        public int? Odometer { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
        public int? BranchId { get; set; }
        public string? Title { get; set; }
        public int Status { get; set; }
        public int MotorWash { get; set; }
        public bool? SparepartBack { get; set; }
        public int? ReviewId { get; set; }

        [ForeignKey(nameof(BranchId))]
        [InverseProperty("Maintenance")]
        public virtual Branch Branch { get; set; }
        [ForeignKey(nameof(MaintenanceStaffId))]
        [InverseProperty(nameof(User.MaintenanceMaintenanceStaff))]
        public virtual User MaintenanceStaff { get; set; }
        [ForeignKey(nameof(ReceptionStaffId))]
        [InverseProperty(nameof(User.MaintenanceReceptionStaff))]
        public virtual User ReceptionStaff { get; set; }
        [ForeignKey(nameof(ReviewId))]
        [InverseProperty("Maintenance")]
        public virtual Review Review { get; set; }
        [ForeignKey(nameof(UserVehicleId))]
        [InverseProperty("Maintenance")]
        public virtual UserVehicle UserVehicle { get; set; }
        [InverseProperty("Maintenance")]
        public virtual ICollection<MaintenanceBillDetail> MaintenanceBillDetail { get; set; }
        [InverseProperty("Maintenance")]
        public virtual ICollection<MaintenanceImage> MaintenanceImage { get; set; }
        [InverseProperty("Maintenance")]
        public virtual ICollection<MaintenanceSchedule> MaintenanceSchedule { get; set; }
        [InverseProperty("Maintenance")]
        public virtual ICollection<SparepartCheckDetail> SparepartCheckDetail { get; set; }
    }
}
