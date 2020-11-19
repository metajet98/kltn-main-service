using System;

namespace main_service.RestApi.Requests
{
    public class SparePartCheckingStatusRequest
    {
        public string Name { get; set; }
        public string Acronym { get; set; }
    }
}