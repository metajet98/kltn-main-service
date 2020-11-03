using System;

namespace main_service.RestApi.Response
{
    public class AuthResponse
    {
        public String AccessToken { get; set; }
        public String RefreshToken { get; set; }
    }
}