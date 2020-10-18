using System;
using System.Security.Cryptography;
using System.Text;
using main_service.EFEntities.Users;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;

namespace main_service.Utils.EncryptionHelper
{
    public class EncryptionHelper : IEncryptionHelper
    {
        const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private readonly string _secretCode;
        
        public EncryptionHelper(IConfiguration configuration)
        {
            _secretCode = configuration.GetSection("Auth:PasswordSalt").Value;
        }

        public UserAuth HashPassword(string password)
        {
            var salt = GetRandomString(16);
            byte[] paper = Encoding.ASCII.GetBytes(_secretCode + salt);
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: paper,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
            return new UserAuth
            {
                Salt = salt,
                HashPassword = hash
            };
        }

        public bool ValidatePassword(string password, string hash, string salt)
        {
            byte[] paper = Encoding.ASCII.GetBytes(_secretCode + salt);
            var checkingHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: paper,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
            return hash.Equals(checkingHash);
        }
        
        private string GetRandomString(int length)
        {
            string s = "";
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                while (s.Length != length)
                {
                    byte[] oneByte = new byte[1];
                    provider.GetBytes(oneByte);
                    char character = (char) oneByte[0];
                    if (Valid.Contains(character))
                    {
                        s += character;
                    }
                }
            }

            return s;
        }
    }
}