using System.Collections.Generic;

namespace main_service.RestApi.Requests
{
    public class MaintenanceRequest
    {
        public int UserVehicleId { get; set; }
        public string? Notes { get; set; }
        public string? Title { get; set; }
        public int Odometer { get; set; }
        public List<string>? Images { get; set; }
        public int MotorWash { get; set; }
        public bool SparepartBack { get; set; }
    }
}