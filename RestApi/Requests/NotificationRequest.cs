namespace main_service.RestApi.Requests
{
    public class NotificationQuery
    {
        public int? UserId { get; set; }
        public int? Page { get; set; }
        public int? Limit { get; set; }
    }
}