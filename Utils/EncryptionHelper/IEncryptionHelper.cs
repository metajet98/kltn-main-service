using System;
using System.Collections.Generic;
using main_service.Databases;
using main_service.RestApi.Response;

namespace main_service.Utils.EncryptionHelper
{
    public interface IEncryptionHelper
    {
        public UserAuth HashPassword(string password, int userId);
        public bool ValidatePassword(string password, string hash, string salt);
        public AuthResponse GenerateToken(int userId, string role);
        public IDictionary<string, object>? VerifyToken(string token);
    }
}