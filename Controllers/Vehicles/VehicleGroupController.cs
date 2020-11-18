using System;
using System.Collections.Generic;
using main_service.Constants;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Vehicles
{
    [ApiController]
    [Route("api/vehicle-group")]
    public class VehicleGroupController : ControllerBase
    {
        private readonly VehicleGroupRepository _vehicleGroupRepository;
        public VehicleGroupController(VehicleGroupRepository vehicleGroupRepository)
        {
            _vehicleGroupRepository = vehicleGroupRepository;
        }
        [HttpGet]
        public JsonResult Get()
        {
            var groups = _vehicleGroupRepository.Get();
            return ResponseHelper<IEnumerable<VehicleGroup>>.OkResponse(groups);
        }

        [HttpPost]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Create([FromBody] VehicleGroupRequest groupRequest)
        {
            _vehicleGroupRepository.Insert(new VehicleGroup
            {
                Capacity = groupRequest.Capacity,
                Name = groupRequest.Name,
                VehicleCompanyId = groupRequest.VehicleCompanyId,
                VehicleTypeId = groupRequest.VehicleTypeId,
                Image = groupRequest.Image
            });
            _vehicleGroupRepository.Save();
            return ResponseHelper<string>.OkResponse(null, "Thêm thành công");
        }
        
        [HttpDelete]
        [Route("{groupId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Delete(int groupId)
        {
            try
            {
                var targetGroup = _vehicleGroupRepository.GetById(groupId);
                if (targetGroup == null)
                {
                    return ResponseHelper<string>.ErrorResponse(null, "Không tìm thấy dòng xe cần xóa");
                }
                _vehicleGroupRepository.Delete(targetGroup);
                _vehicleGroupRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Xóa thành công");
            }
            catch (Exception e)
            {
                return ResponseHelper<string>.ErrorResponse(e.Message, "Lỗi khi xóa dòng xe");
            }
            
        }
    }
}