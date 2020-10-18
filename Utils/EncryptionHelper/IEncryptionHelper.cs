using main_service.EFEntities;
using main_service.EFEntities.Users;

namespace main_service.Utils.EncryptionHelper
{
    public interface IEncryptionHelper
    {
        public UserAuth HashPassword(string password);
        public bool ValidatePassword(string password, string hash, string salt);
    }
}