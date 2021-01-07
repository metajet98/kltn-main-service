using System.Collections.Generic;

namespace main_service.RestApi.Requests
{
    public class TopicQuery
    {
        public int? CreatedUserId { get; set; }
    }
    public class TopicRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string>? Images { get; set; }
    }
    
    public class TopicReplyRequest
    {
        public string Content { get; set; }
        public string? Image { get; set; }
    }
}