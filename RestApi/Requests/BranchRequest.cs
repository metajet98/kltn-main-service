namespace main_service.RestApi.Requests
{
    public class BranchRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Logo { get; set; }
    }
}