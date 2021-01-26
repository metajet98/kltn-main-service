using System;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using main_service.RestApi.Response;
using main_service.Utils.EncryptionHelper;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Auth
{
    public class AuthController : ControllerBase
    {
        private readonly IEncryptionHelper _encryptionHelper;
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository, IEncryptionHelper encryptionHelper)
        {
            _encryptionHelper = encryptionHelper;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("/api/login")]
        public JsonResult Login([FromBody] UserRequest authRequest)
        {
            try
            {
                var user = _userRepository.FindByPhoneNumber(authRequest.PhoneNumber);
                if (user == null)
                {
                    return ResponseHelper<string>.ErrorResponse(null,
                        "Số điện thoại hoặc mật khẩu không đúng, vui lòng thử lại!");
                }

                var isPasswordCorrect =
                    _encryptionHelper.ValidatePassword(authRequest.Password, user.UserAuth.Hash, user.UserAuth.Salt);
                if (isPasswordCorrect)
                {
                    return new JsonResult(_encryptionHelper.GenerateToken(user.Id, user.Role));
                }

                return ResponseHelper<string>.ErrorResponse(null,
                    "Số điện thoại hoặc mật khẩu không đúng, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
            
        }
        
        [HttpPost]
        [Route("/api/login/staff_maintenance")]
        public JsonResult StaffLogin([FromBody] UserRequest authRequest)
        {
            try
            {
                var user = _userRepository.FindByPhoneNumber(authRequest.PhoneNumber);
                if (user == null)
                {
                    return ResponseHelper<string>.ErrorResponse(null,  "Số điện thoại hoặc mật khẩu không đúng, vui lòng thử lại!");
                }
            
                if (user.Role != Constants.Role.StaffMaintenance)
                {
                    return ResponseHelper<string>.ErrorResponse(null,  "Tài khoản không phải là nhân viên bảo dưỡng");
                }

                var isPasswordCorrect = _encryptionHelper.ValidatePassword(authRequest.Password, user.UserAuth.Hash, user.UserAuth.Salt);
                if (isPasswordCorrect)
                {
                    return new JsonResult(_encryptionHelper.GenerateToken(user.Id, user.Role));
                }

                return ResponseHelper<string>.ErrorResponse(null, "Số điện thoại hoặc mật khẩu không đúng, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
        
        [HttpPost]
        [Route("/api/token/refresh")]
        public JsonResult RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var payLoad = _encryptionHelper.VerifyToken(request.RefreshToken);
                if (payLoad == null)
                {
                    return ResponseHelper<string>.UnauthorizedResponse(null,
                        "Refresh Token không hợp lệ hoặc đã hết hạn");
                }

                int.TryParse(payLoad["unique_name"].ToString(), out var userId);
                var user = _userRepository.GetById(userId);

                if (user == null)
                {
                    return ResponseHelper<string>.UnauthorizedResponse(null,
                        "Refresh Token không hợp lệ hoặc đã hết hạn");
                }

                return new JsonResult(_encryptionHelper.GenerateToken(user.Id, user.Role));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
            
        }
        
        // [HttpPatch]
        // [Route("/password")]
        // public async Task<JsonResult> ChangePassword([FromBody] ChangePasswordRequest requestData)
        // {
        //     var userId = User.Identity.GetId();
        //     var user = await _authUserRepo.FindUserAuthByUserId(userId);
        //
        //     if (_authService.HashPassword(requestData.OldPassword) == user.HashPassword)
        //     {
        //         var newHashPassword = _authService.HashPassword(requestData.NewPassword);
        //         var result = await _authUserRepo.UpdateUserPassword(userId, newHashPassword);
        //         
        //         if (result)
        //         {
        //             return new OkResponse(new
        //             {
        //                 Message = "Thay đổi mật khẩu thành công"
        //             });
        //         }
        //         else
        //         {
        //             return new BadRequestResponse(new ErrorData
        //             {
        //                 Message = "Thay đổi mật khẩu thất bại!"
        //             });
        //         }
        //     }
        //     else
        //     {
        //         return new BadRequestResponse(new ErrorData
        //         {
        //             Message = "Mật khẩu cũ không đúng!"
        //         });
        //     }
        // }
    }
}