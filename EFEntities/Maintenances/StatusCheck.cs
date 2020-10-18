using System.Collections.Generic;
using main_service.EFEntities.Base;

namespace main_service.EFEntities.Maintenances
{
    public class StatusCheck : BaseEntity
    {
        public string Description { get; set; }
        
        public virtual IEnumerable<MaintenanceChecking> MaintenanceCheckings { get; set; }
    }
}