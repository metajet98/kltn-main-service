using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using main_service.Databases;
using main_service.RestApi.Response;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace main_service.Utils.EncryptionHelper
{
    public class EncryptionHelper : IEncryptionHelper
    {
        const string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private readonly string _passwordPepper;
        private readonly string _jwtSecretCode;
        private readonly int _accessTokenExpireTime;
        private readonly int _refreshTokenExpireTime;
        
        public EncryptionHelper(IConfiguration configuration)
        {
            _passwordPepper = configuration.GetSection("Auth:PasswordPepper").Value;
            _jwtSecretCode = configuration.GetSection("Auth:JwtSecretCode").Value;
            _accessTokenExpireTime = int.Parse(configuration.GetSection("Auth:AccessTokenExpireTime").Value);
            _refreshTokenExpireTime = int.Parse(configuration.GetSection("Auth:RefreshTokenExpireTime").Value);
        }

        public UserAuth HashPassword(string password, int userId)
        {
            var salt = GetRandomString(16);
            byte[] paper = Encoding.ASCII.GetBytes(_passwordPepper + salt);
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
                Hash = hash,
                UserId = userId,
                CreatedDate = DateTime.Now
            };
        }

        public bool ValidatePassword(string password, string hash, string salt)
        {
            byte[] paper = Encoding.ASCII.GetBytes(_passwordPepper + salt);
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
        
        public AuthResponse GenerateToken(int userId, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecretCode);
            var listAccessTokenClaim = new List<Claim> {new Claim(ClaimTypes.Name, userId.ToString()), new Claim(ClaimTypes.Role, role)};
            var listRefreshTokenClaim = new List<Claim> {new Claim(ClaimTypes.Name, userId.ToString())};
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(listAccessTokenClaim),
                Expires = DateTime.UtcNow.AddHours(_accessTokenExpireTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(listRefreshTokenClaim),
                Expires = DateTime.UtcNow.AddHours(_refreshTokenExpireTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);
            return new AuthResponse
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = tokenHandler.WriteToken(refreshToken),
                ExpiredIn = ((DateTimeOffset)accessTokenDescriptor.Expires).ToUnixTimeSeconds()
            };
        }
        public IDictionary<string, object>? VerifyToken(string token)
        {
            try
            {
                var payload = new JwtBuilder()
                    .WithAlgorithm(new HMACSHA256Algorithm())
                    .WithSecret(_jwtSecretCode)
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                return payload;
            }
            catch (TokenExpiredException)
            {
                return null;
            }
            catch (SignatureVerificationException)
            {
                return null;
            }
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