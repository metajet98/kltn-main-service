using System;

namespace main_service.RestApi.Requests
{
    public class UserCalenderRequest
    {
        public string Notes { get; set; }
        public int BranchId { get; set; }
        public DateTime Time { get; set; }
    }
    
    public class UserCalenderQuery
    {
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public DateTime? Date { get; set; } 
    }
    
    public class UserCalenderReview
    {
        public bool IsApprove { get; set; }
        public string? Review { get; set; }
    }
}