using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("SPAREPART_STATUS")]
    public partial class SparepartStatus
    {
        public SparepartStatus()
        {
            MaintenanceCheck = new HashSet<MaintenanceCheck>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        [StringLength(1)]
        public string Acronym { get; set; }

        [InverseProperty("Status")]
        public virtual ICollection<MaintenanceCheck> MaintenanceCheck { get; set; }
    }
}
