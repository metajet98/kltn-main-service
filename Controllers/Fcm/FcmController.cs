using System;
using main_service.Constants;
using main_service.Extensions;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Fcm
{
    [ApiController]
    [Route("/api/fcm/")]
    public class FcmController : ControllerBase
    {
        private readonly FcmTokenRepository _fcmTokenRepository;

        public FcmController(FcmTokenRepository fcmTokenRepository)
        {
            _fcmTokenRepository = fcmTokenRepository;
        }

        [HttpPost]
        [Route("register")]
        [Authorize(Roles = Role.SystemUser)]
        public JsonResult Register([FromBody] FcmTokenRequest data)
        {
            try
            {
                var userId = User.Identity.GetId();
                return _fcmTokenRepository.InsertToken(data.Token, userId) ? ResponseHelper<dynamic>.OkResponse(null) : ResponseHelper<dynamic>.ErrorResponse(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra khi đăng kí token FCM!");
            }
        }

        [HttpPost]
        [Route("unregister")]
        [Authorize(Roles = Role.SystemUser)]
        public JsonResult Unregister([FromBody] FcmTokenRequest data)
        {
            try
            {
                var userId = User.Identity.GetId();
                _fcmTokenRepository.RemoveToken(data.Token, userId);
                return ResponseHelper<dynamic>.OkResponse(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra khi huỷ token FCM!");
            }
        }
    }
}