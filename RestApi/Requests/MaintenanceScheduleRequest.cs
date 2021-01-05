using System;

namespace main_service.RestApi.Requests
{
    public class MaintenanceScheduleRequest
    {
        public DateTime? Date { get; set; }
        public int? Odometer { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
    }
}