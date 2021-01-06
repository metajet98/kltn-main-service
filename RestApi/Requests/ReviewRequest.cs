namespace main_service.RestApi.Requests
{
    public class ReviewRequest
    {
        public double Star { get; set; }
        public string? Comment { get; set; }
    }
}