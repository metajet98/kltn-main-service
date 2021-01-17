using System;
using System.Collections.Generic;
using main_service.Constants;
using main_service.Databases;
using main_service.Extensions;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using main_service.Utils.EncryptionHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Users
{
    [Route("/api/user")]
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
        
        [HttpPost]
        public JsonResult Create([FromBody] UserRequest userRequest)
        {
            var newUser = new User
            {
                Address = userRequest.Address,
                Birthday = userRequest.Birthday, 
                Email = userRequest.Email,
                FullName = userRequest.FullName,
                PhoneNumber = userRequest.PhoneNumber,
                Role = Role.User,
                CreatedDate = DateTime.Now
            };
            _userRepository.Insert(newUser);
            _userRepository.Save();
            var newUserAuth = _encryptionHelper.HashPassword(userRequest.Password, newUser.Id);
            _userAuthRepository.Insert(newUserAuth);
            _userAuthRepository.Save();
            
            return ResponseHelper<string>.OkResponse(null, "Tạo tài khoản thành công!");
        }

        [HttpGet]
        [Authorize(Roles = Constants.Role.CenterManager)]
        public JsonResult GetAll()
        {
            var users = _userRepository.Get(x => x.Role.Equals(Role.User));
            return ResponseHelper<IEnumerable<User>>.OkResponse(users);
        }
        
        [HttpGet]
        [Route("self")]
        [Authorize(Roles = Role.All)]
        public JsonResult Self()
        {
            var userId = User.Identity.GetId();
            return ResponseHelper<User>.OkResponse(_userRepository.GetUserInfo(userId));
        }
        
        [HttpGet]
        [Route("{userId}")]
        [Authorize(Roles = Constants.Role.CenterManager)]
        public JsonResult Get(int userId)
        {
            var user = _userRepository.GetById(userId);
            return ResponseHelper<User>.OkResponse(user);
        }
    }
}