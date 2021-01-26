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
    [Route("api/vehicle-type/")]
    public class VehicleTypeController : ControllerBase
    {
        private readonly VehicleTypeRepository _vehicleTypeRepository;
        public VehicleTypeController(VehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }
        [HttpGet]
        public JsonResult Get()
        {
            var companies = _vehicleTypeRepository.Get();
            return ResponseHelper<IEnumerable<VehicleType>>.OkResponse(companies);
        }

        [HttpPost]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Create([FromBody] VehicleTypeRequest vehicleTypeRequest)
        {
            try
            {
                _vehicleTypeRepository.Insert(new VehicleType
                {
                    TypeName = vehicleTypeRequest.TypeName
                });
                _vehicleTypeRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Thêm thành công");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
        
        [HttpDelete]
        [Route("{typeId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Delete(int typeId)
        {
            try
            {
                var targetCompany = _vehicleTypeRepository.GetById(typeId);
                if (targetCompany == null)
                {
                    return ResponseHelper<string>.ErrorResponse(null, "Không tìm thấy dòng xe cần xóa");
                }
                _vehicleTypeRepository.Delete(targetCompany);
                _vehicleTypeRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Xóa thành công");
            }
            catch (Exception e)
            {
                return ResponseHelper<string>.ErrorResponse(e.Message, "Lỗi khi xóa dòng xe");
            }
        }
    }
}