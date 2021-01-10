using System.Collections.Generic;
using main_service.Databases;
using main_service.Extensions;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.UserCalender
{
    [Route("/api/calender/")]
    public class UserCalenderController : ControllerBase
    {
        private readonly UserCalenderRepository _userCalenderRepository;

        public UserCalenderController(UserCalenderRepository userCalenderRepository)
        {
            _userCalenderRepository = userCalenderRepository;
        }

        [HttpPost]
        [Authorize(Roles = Constants.Role.User)]
        public JsonResult Create([FromBody] UserCalenderRequest request)
        {
            var userId = User.Identity.GetId();
            var result = _userCalenderRepository.Create(request, userId);
            return result
                ? ResponseHelper<object>.OkResponse(null, "Đăng kí lịch thành công")
                : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
        }
        
        [HttpDelete]
        [Authorize(Roles = Constants.Role.SystemUser)]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            _userCalenderRepository.Delete(id);
            _userCalenderRepository.Save();
            return ResponseHelper<object>.OkResponse(null, "Xoá thành công");
        }
        
        [HttpGet]
        [Authorize(Roles = Constants.Role.SystemUser)]
        public JsonResult GetAll([FromQuery] UserCalenderQuery query)
        {
            var result = _userCalenderRepository.Query(query);
            return ResponseHelper<IEnumerable<CustomerCalender>>.OkResponse(result);
            
        }
    }
}