using System;
using System.Collections.Generic;
using main_service.Databases;
using main_service.Extensions;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using main_service.RestApi.Response;
using main_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Topics
{
    [Route("/api/topic")]
    public class TopicController : ControllerBase
    {
        private readonly TopicRepository _topicRepository;
        private readonly NotificationsRepository _notificationsRepository;
        private readonly FcmService _fcmService;

        public TopicController(TopicRepository topicRepository, FcmService fcmService,
            NotificationsRepository notificationsRepository)
        {
            _topicRepository = topicRepository;
            _notificationsRepository = notificationsRepository;
            _fcmService = fcmService;
        }

        [HttpPost]
        [Authorize(Roles = Constants.Role.User)]
        public JsonResult Create([FromBody] TopicRequest topicRequest)
        {
            try
            {
                var userId = User.Identity.GetId();
                var result = _topicRepository.CreateTopic(topicRequest, userId);
                return result
                    ? ResponseHelper<dynamic>.OkResponse(null, "Tạo hỏi đáp thành công!")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpGet]
        [Route("/api/topic")]
        [Authorize(Roles = Constants.Role.All)]
        public JsonResult GetAll([FromQuery] TopicQuery query)
        {
            var topics = _topicRepository.QueryTopics(query);
            return ResponseHelper<IEnumerable<Topic>>.OkResponse(topics);
        }

        [HttpGet]
        [Route("/api/topic/{id}")]
        [Authorize(Roles = Constants.Role.All)]
        public JsonResult Get(int id)
        {
            var topic = _topicRepository.GetTopic(id);
            return ResponseHelper<Topic>.OkResponse(topic);
        }

        [HttpPost]
        [Route("/api/topic/{id}/reply")]
        [Authorize(Roles = Constants.Role.All)]
        public JsonResult PostReply(int id, [FromBody] TopicReplyRequest topicReplyRequest)
        {
            try
            {
                var userId = User.Identity.GetId();
                var topic = _topicRepository.GetById(id);
                var result = _topicRepository.PostReply(topicReplyRequest, id, userId);

                if (userId != topic.UserId)
                {
                    var data = FcmData.CreateFcmData("topic_reply", null);
                    var notify = FcmData.CreateFcmNotification(
                        "Vừa có người trả lời topic của bạn",
                        "Topic của bạn vừa có một lượt trả lời, xem ngay tại trang hỏi đáp",
                        null);
                    _fcmService.SendMessage(topic.UserId, data, notify);
                    _notificationsRepository.Insert(new Notification
                    {
                        UserId = userId,
                        Description = "Topic của bạn vừa có một lượt trả lời, xem ngay tại trang hỏi đáp",
                        Title = "Vừa có người trả lời topic của bạn",
                        Activity = "topic_reply",
                        CreatedDate = DateTime.Now,
                    });
                    _notificationsRepository.Save();
                }

                return result
                    ? ResponseHelper<dynamic>.OkResponse(null, "Trả lời thành công!")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}