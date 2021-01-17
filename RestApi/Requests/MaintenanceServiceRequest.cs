namespace main_service.RestApi.Requests
{
    public class MaintenanceServiceRequest
    {
        public int VehicleGroupId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? WarrantyPeriod { get; set; }
    }
}