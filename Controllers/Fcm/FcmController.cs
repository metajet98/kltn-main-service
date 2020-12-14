using System;
using System.Collections.Generic;
using main_service.Constants;
using main_service.Databases;
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
            var userId = User.Identity.GetId();
            return _fcmTokenRepository.InsertToken(data.Token, userId) ? ResponseHelper<dynamic>.OkResponse(null) : ResponseHelper<dynamic>.ErrorResponse(null);
        }

        [HttpPost]
        [Route("unregister")]
        [Authorize(Roles = Role.SystemUser)]
        public JsonResult Unregister([FromBody] FcmTokenRequest data)
        {
            var userId = User.Identity.GetId();
            var resultDelete = _fcmTokenRepository.RemoveToken(data.Token, userId);
            return resultDelete
                ? ResponseHelper<dynamic>.OkResponse(null)
                : ResponseHelper<dynamic>.ErrorResponse(null);
        }
    }
}