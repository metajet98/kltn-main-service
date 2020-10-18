using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Maintenances
{
    [Table("MaintenanceImage")]
    public class MaintenanceImage : BaseEntity
    {
        public int MaintenanceId { get; set; }
        public string ImageUrl { get; set; }

        [ForeignKey("MaintenanceId")]
        public virtual Maintenance Maintenance { get; set; }
    }
}