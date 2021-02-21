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

namespace main_service.Controllers.UserCalender
{
    [Route("/api/calender/")]
    public class UserCalenderController : ControllerBase
    {
        private readonly UserCalenderRepository _userCalenderRepository;
        private readonly FcmService _fcmService;

        public UserCalenderController(UserCalenderRepository userCalenderRepository, FcmService fcmService)
        {
            _userCalenderRepository = userCalenderRepository;
            _fcmService = fcmService;
        }

        [HttpPost]
        [Authorize(Roles = Constants.Role.User)]
        public JsonResult Create([FromBody] UserCalenderRequest request)
        {
            try
            {
                var userId = User.Identity.GetId();
                var result = _userCalenderRepository.Create(request, userId);
                return result
                    ? ResponseHelper<object>.OkResponse(null, "Đăng kí lịch thành công")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
        
        [HttpDelete]
        [Authorize(Roles = Constants.Role.SystemUser)]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                _userCalenderRepository.Delete(id);
                _userCalenderRepository.Save();
                return ResponseHelper<object>.OkResponse(null, "Xoá thành công");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Role.StaffDesk)]
        [Route("{id}")]
        public JsonResult Review(int id, [FromBody] UserCalenderReview review)
        {
            try
            {
                var userCalender = _userCalenderRepository.Review(id, review);
                if(userCalender == null) return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
                var data = FcmData.CreateFcmData("calender_review", null);
                if (review.IsApprove)
                {
                    var notify = FcmData.CreateFcmNotification(
                        "Lịch hẹn vừa được duyệt",
                        $"Lịch hẹn vào {userCalender.Time:dd/MM/yyyy HH:mm} vừa được duyệt",
                        null);
                    _fcmService.SendMessage(userCalender.UserId, data, notify);
                }
                else
                {
                    var notify = FcmData.CreateFcmNotification(
                        "Lịch hẹn đã bị từ chối",
                        $"Lịch hẹn vào {userCalender.Time:dd/MM/yyyy HH:mm} đã bị từ chối. Lý do: {review.Review}",
                        null);
                    _fcmService.SendMessage(userCalender.UserId, data, notify);
                }
                return ResponseHelper<object>.OkResponse(null, "Cập nhật thành công");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
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