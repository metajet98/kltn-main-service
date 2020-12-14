namespace main_service.RestApi.Requests
{
    public class UserVehicleRequest
    {
        public int? UserId { get; set; }
        public int VehicleGroupId { get; set; }
        public string? ChassisNumber { get; set; }
        public string? Name { get; set; }
        public string? EngineNumber { get; set; }
        public string PlateNumber { get; set; }
        public string Color { get; set; }
    }
}