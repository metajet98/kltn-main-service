using System.ComponentModel.DataAnnotations.Schema;
using main_service.EFEntities.Base;
using main_service.EFEntities.Vehicles;

namespace main_service.EFEntities.Maintenances
{
    public class MaintenanceChecking : BaseEntity
    {
        public int CheckingItemId { get; set; }
        public int StatusId { get; set; }
        public int MaintenanceId { get; set; }
        
        [ForeignKey("CheckingItemId")] 
        public virtual CheckingItem CheckingItem { get; set; }
        [ForeignKey("StatusId")] 
        public virtual StatusCheck StatusCheck { get; set; }
        [ForeignKey("MaintenanceId")] 
        public virtual Maintenance Maintenance { get; set; }
    }
}