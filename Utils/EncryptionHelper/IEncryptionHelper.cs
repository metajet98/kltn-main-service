using System;
using System.Collections.Generic;
using main_service.Databases;

namespace main_service.Utils.EncryptionHelper
{
    public interface IEncryptionHelper
    {
        public UserAuth HashPassword(string password, int userId);
        public bool ValidatePassword(string password, string hash, string salt);
        public string GenerateAccessToken(int userId, string role);
        public string GenerateRefreshToken(int userId);
        public IDictionary<string, object>? VerifyToken(string token);
    }
}