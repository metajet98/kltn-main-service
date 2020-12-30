using System.Collections.Generic;

namespace main_service.RestApi.Requests
{
    public class SparePartMaintenanceCheckRequest
    {
        public List<SparePartMaintenanceCheck> SparePartMaintenanceChecks { get; set; }
    }

    public class SparePartMaintenanceCheck
    {
        public int StatusId { get; set; }
        public int VehicleSparePartId { get; set; }
    }
}