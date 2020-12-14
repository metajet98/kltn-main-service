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

namespace main_service.Controllers.Vehicles
{
    [ApiController]
    [Route("api/user-vehicle/")]
    public class UserVehicleController : ControllerBase
    {
        private readonly UserVehicleRepository _userVehicleRepository;
        private readonly UserRepository _userRepository;
        private readonly VehicleGroupRepository _vehicleGroupRepository;

        public UserVehicleController(UserVehicleRepository userVehicleRepository, UserRepository userRepository, VehicleGroupRepository vehicleGroupRepository)
        {
            _userVehicleRepository = userVehicleRepository;
            _userRepository = userRepository;
            _vehicleGroupRepository = vehicleGroupRepository;
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
            return ResponseHelper<string>.OkResponse(null, "Thêm xe thành công");
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            var userId = User.Identity.GetId();
            var userVehicles = _userVehicleRepository.FindByUserId(userId);
            return ResponseHelper<IEnumerable<UserVehicle>>.OkResponse(userVehicles);
        }
        
        [HttpGet]
        [Route("{userVehicleId}")]
        public JsonResult Get(int userVehicleId)
        {
            var userVehicle = _userVehicleRepository.GetById(userVehicleId);
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
    }
}