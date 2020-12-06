namespace main_service.RestApi.Requests
{
    public class BranchServicePriceRequest
    {
        public int MaintenanceServiceId { get; set; }
        public int LaborCost { get; set; }
        public int SparePartPrice { get; set; }
    }
}