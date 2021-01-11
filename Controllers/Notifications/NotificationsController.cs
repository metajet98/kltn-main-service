using System.Collections.Generic;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Notifications
{
    [ApiController]
    [Route("/api/")]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationsRepository _notificationsRepository;

        public NotificationsController(NotificationsRepository notificationsRepository)
        {
            _notificationsRepository = notificationsRepository;
        }

        [HttpGet]
        [Route("notifications")]
        [Authorize(Roles = Constants.Role.User)]
        public JsonResult GetNotification([FromQuery] NotificationQuery query)
        {
            var notifications = _notificationsRepository.QueryNotifications(query);
            return ResponseHelper<IEnumerable<Notification>>.OkResponse(notifications);
        }
        
        [HttpGet]
        [Route("banners")]
        [Authorize(Roles = Constants.Role.User)]
        public JsonResult GetBanner()
        {
            var banners = _notificationsRepository.QueryBanners();
            return ResponseHelper<IEnumerable<Banner>>.OkResponse(banners);
        }
    }
}