using System;
using main_service.Repositories;
using main_service.RestApi.Response;
using main_service.Utils.EncryptionHelper;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers
{
    [ApiController]
    [Route("api/initial")]
    public class InitialController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly UserAuthRepository _userAuthRepository;
        private readonly IEncryptionHelper _encryptionHelper;

        public InitialController(UserRepository userRepository, UserAuthRepository userAuthRepository,
            IEncryptionHelper encryptionHelper)
        {
            _userRepository = userRepository;
            _userAuthRepository = userAuthRepository;
            _encryptionHelper = encryptionHelper;
        }

        [HttpPost]
        [Route("")]
        public JsonResult Post()
        {
            var newUser = new Databases.User
            {
                Role = Constants.Role.CenterManager,
                PhoneNumber = "0123456789",
                CreatedDate = DateTime.Now,
                Address = "",
                Birthday = DateTime.Now,
                FullName = "CenterManager",
                Email = "centermanager@centermanager.com"
            };
            _userRepository.Insert(newUser);
            _userRepository.Save();
            var newUserAuth = _encryptionHelper.HashPassword("123456", newUser.Id);
            _userAuthRepository.Insert(newUserAuth);
            _userAuthRepository.Save();
            return new JsonResult(new OkResponse<string>("OK"));
        }
    }
}