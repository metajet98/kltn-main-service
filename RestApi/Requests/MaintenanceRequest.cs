namespace main_service.RestApi.Requests
{
    public class MaintenanceRequest
    {
        public int UserVehicleId { get; set; }
        public string? Notes { get; set; }
        public int Odometer { get; set; }
    }
}