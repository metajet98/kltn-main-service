using System;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories.User;
using main_service.RestApi.Requests;
using main_service.Utils.EncryptionHelper;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers
{
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly UserAuthRepository _userAuthRepository;
        private readonly IEncryptionHelper _encryptionHelper;

        public UserController(UserRepository userRepository, UserAuthRepository userAuthRepository, IEncryptionHelper encryptionHelper)
        {
            _userRepository = userRepository;
            _userAuthRepository = userAuthRepository;
            _encryptionHelper = encryptionHelper;
        }
        public JsonResult Create([FromBody] UserRequest userRequest)
        {
            var newUser = new User
            {
                Address = userRequest.Address,
                Birthday = userRequest.Birthday, 
                Email = userRequest.Email,
                FullName = userRequest.FullName,
                PhoneNumber = userRequest.PhoneNumber,
                Role = Constants.Role.User,
                CreatedDate = DateTime.Now
            };
            _userRepository.Insert(newUser);
            _userRepository.Save();
            var newUserAuth = _encryptionHelper.HashPassword(userRequest.Password, newUser.Id);
            _userAuthRepository.Insert(newUserAuth);
            _userAuthRepository.Save();
            
            return ResponseHelper<string>.OkResponse(null, "Tạo tài khoản thành công!");
        }
    }
}