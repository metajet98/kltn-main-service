using System.Collections.Generic;
using System.Linq;
using main_service.Databases;
using main_service.Extensions;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Topics
{
    [Route("/api/topic")]
    public class TopicController : ControllerBase
    {
        private readonly TopicRepository _topicRepository;

        public TopicController(TopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        [HttpPost]
        [Authorize(Roles = Constants.Role.User)]
        public JsonResult Create([FromBody] TopicRequest topicRequest)
        {
            var userId = User.Identity.GetId();
            var result = _topicRepository.CreateTopic(topicRequest, userId);
            return result
                ? ResponseHelper<dynamic>.OkResponse(null, "Tạo hỏi đáp thành công!")
                : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
        }
        
        [HttpGet]
        [Route("/api/topic")]
        [Authorize(Roles = Constants.Role.SystemUser)]
        public JsonResult GetAll([FromQuery] TopicQuery query)
        {
            var topics = _topicRepository.QueryTopic(query);
            return ResponseHelper<IEnumerable<Topic>>.OkResponse(topics);
        }

        [HttpGet]
        [Route("/api/topic/{id}")]
        [Authorize(Roles = Constants.Role.User)]
        public JsonResult Get(int id)
        {
            var topic = _topicRepository.Get(x => x.Id.Equals(id), includeProperties: "TopicImage,TopicReply,User").FirstOrDefault();
            return ResponseHelper<Topic>.OkResponse(topic);
        }

        [HttpPost]
        [Route("/api/topic/{id}/reply")]
        [Authorize(Roles = Constants.Role.SystemUser)]
        public JsonResult PostReply(int id, [FromBody] TopicReplyRequest topicReplyRequest)
        {
            var userId = User.Identity.GetId();
            var result = _topicRepository.PostReply(topicReplyRequest, id, userId);
            return result
                ? ResponseHelper<dynamic>.OkResponse(null, "Trả lời thành công!")
                : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
        }
    }
}