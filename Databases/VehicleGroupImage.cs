using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("VEHICLE_GROUP_IMAGE")]
    public partial class VehicleGroupImage
    {
        [Key]
        public int Id { get; set; }
        public int VehicleGroupId { get; set; }
        [Required]
        public string Image { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(VehicleGroupId))]
        [InverseProperty("VehicleGroupImage")]
        public virtual VehicleGroup VehicleGroup { get; set; }
    }
}
