using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("VEHICLE_TYPE")]
    public partial class VehicleType
    {
        public VehicleType()
        {
            VehicleGroup = new HashSet<VehicleGroup>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string TypeName { get; set; }

        [InverseProperty("VehicleType")]
        public virtual ICollection<VehicleGroup> VehicleGroup { get; set; }
    }
}
