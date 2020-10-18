using main_service.Databases;

namespace main_service.Utils.EncryptionHelper
{
    public interface IEncryptionHelper
    {
        public UserAuth HashPassword(string password);
        public bool ValidatePassword(string password, string hash, string salt);
    }
}