using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("VEHICLE_COMPANY")]
    public partial class VehicleCompany
    {
        public VehicleCompany()
        {
            VehicleGroup = new HashSet<VehicleGroup>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Logo { get; set; }

        [InverseProperty("VehicleCompany")]
        public virtual ICollection<VehicleGroup> VehicleGroup { get; set; }
    }
}
