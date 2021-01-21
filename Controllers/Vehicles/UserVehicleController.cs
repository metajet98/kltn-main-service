using System;
using System.Collections.Generic;
using System.Linq;
using main_service.Constants;
using main_service.Databases;
using main_service.Extensions;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using main_service.RestApi.Response;
using main_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Vehicles
{
    [ApiController]
    [Route("api/user-vehicle/")]
    public class UserVehicleController : ControllerBase
    {
        private readonly UserVehicleRepository _userVehicleRepository;
        private readonly UserRepository _userRepository;
        private readonly VehicleGroupRepository _vehicleGroupRepository;
        private readonly FcmService _fcmService;

        public UserVehicleController(UserVehicleRepository userVehicleRepository, UserRepository userRepository,
            VehicleGroupRepository vehicleGroupRepository, FcmService fcmService)
        {
            _userVehicleRepository = userVehicleRepository;
            _userRepository = userRepository;
            _vehicleGroupRepository = vehicleGroupRepository;
            _fcmService = fcmService;
        }

        [HttpPost]
        public JsonResult Create([FromBody] UserVehicleRequest userVehicleRequest)
        {
            var userId = userVehicleRequest.UserId ?? User.Identity.GetId();
            if (_userRepository.GetById(userId) == null)
            {
                return ResponseHelper<string>.ErrorResponse("userId", "Người dùng không tồn tại");
            }

            if (_vehicleGroupRepository.GetById(userVehicleRequest.VehicleGroupId) == null)
            {
                return ResponseHelper<string>.ErrorResponse("vehicleGroupId", "Không tìm thấy loại xe");
            }

            var newUserVehicle = new UserVehicle
            {
                Color = userVehicleRequest.Color,
                UserId = userId,
                VehicleGroupId = userVehicleRequest.VehicleGroupId,
                EngineNumber = userVehicleRequest.EngineNumber,
                PlateNumber = userVehicleRequest.PlateNumber,
                ChassisNumber = userVehicleRequest.ChassisNumber,
                Name = userVehicleRequest.Name,
            };

            _userVehicleRepository.Insert(newUserVehicle);
            _userVehicleRepository.Save();
            _fcmService.SendMessages(
                new List<int> {userId},
                FcmData.CreateFcmData("create_bike_success", new Dictionary<string, string> {{"status", "done"}}));
            return ResponseHelper<string>.OkResponse(null, "Thêm xe thành công");
        }

        [HttpGet]
        [Route("self")]
        [Authorize(Roles = Role.User)]
        public JsonResult GetAllSelf()
        {
            var userId = User.Identity.GetId();
            var userVehicles = _userVehicleRepository.FindByUserId(userId);
            return ResponseHelper<IEnumerable<UserVehicle>>.OkResponse(userVehicles);
        }

        [HttpGet]
        [Authorize(Roles = Role.Staff)]
        public JsonResult QueryAll([FromQuery] UserVehicleQuery query)
        {
            var userVehicles =
                _userVehicleRepository.Get(x => x.PlateNumber.Equals(query.PlateNumber), includeProperties: "User");
            return ResponseHelper<IEnumerable<UserVehicle>>.OkResponse(userVehicles);
        }

        [HttpGet]
        [Route("{userVehicleId}")]
        public JsonResult Get(int userVehicleId)
        {
            var userVehicle = _userVehicleRepository
                .Get(x => x.Id.Equals(userVehicleId), includeProperties: "Maintenance,VehicleGroup").FirstOrDefault();
            return ResponseHelper<UserVehicle>.OkResponse(userVehicle);
        }

        [HttpDelete]
        [Route("{userVehicleId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Delete(int userVehicleId)
        {
            try
            {
                var targetUserVehicle = _userVehicleRepository.GetById(userVehicleId);
                if (targetUserVehicle == null)
                {
                    return ResponseHelper<string>.ErrorResponse(null, "Không tìm thấy xe cần xóa");
                }

                _userVehicleRepository.Delete(targetUserVehicle);
                _userVehicleRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Xóa thành công");
            }
            catch (Exception e)
            {
                return ResponseHelper<string>.ErrorResponse(e.Message, "Lỗi khi xóa xe");
            }
        }

        [HttpGet]
        [Route("{userVehicleId}/schedule")]
        [Authorize(Roles = Role.All)]
        public JsonResult GetVehicleSchedule(int userVehicleId)
        {
            var result = _userVehicleRepository.GetSchedule(userVehicleId);
            return ResponseHelper<IEnumerable<MaintenanceSchedule>>.OkResponse(result);
        }
    }
}