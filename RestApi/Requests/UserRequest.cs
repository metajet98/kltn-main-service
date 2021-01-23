using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace main_service.RestApi.Requests
{
    public class UserRequest
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string Password { get; set; }
        public int? BranchId { get; set; }
        public string Role { get; set; }
    }
}