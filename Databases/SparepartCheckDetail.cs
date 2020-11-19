using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.Databases
{
    [Table("SPAREPART_CHECK_DETAIL")]
    public partial class SparepartCheckDetail
    {
        [Key]
        public int Id { get; set; }
        public int MaintenanceId { get; set; }
        public int StatusId { get; set; }
        public int SparePartItemId { get; set; }

        [ForeignKey(nameof(MaintenanceId))]
        [InverseProperty("SparepartCheckDetail")]
        public virtual Maintenance Maintenance { get; set; }
        [ForeignKey(nameof(SparePartItemId))]
        [InverseProperty(nameof(VehicleGroupSparepartItem.SparepartCheckDetail))]
        public virtual VehicleGroupSparepartItem SparePartItem { get; set; }
        [ForeignKey(nameof(StatusId))]
        [InverseProperty(nameof(SparepartStatus.SparepartCheckDetail))]
        public virtual SparepartStatus Status { get; set; }
    }
}
