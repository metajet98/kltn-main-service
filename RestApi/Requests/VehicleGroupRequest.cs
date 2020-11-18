namespace main_service.RestApi.Requests
{
    public class VehicleGroupRequest
    {
        public string Name { get; set; }
        public int VehicleTypeId { get; set; }
        public int VehicleCompanyId { get; set; }
        public int Capacity { get; set; }
        public string Image { get; set; }
    }
}