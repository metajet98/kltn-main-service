using System.Collections.Generic;
using main_service.Constants;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.SpareParts
{
    [ApiController]
    [Route("/api/")]
    public class SparePartsController : ControllerBase
    {
        private readonly SparePartRepository _sparePartRepository;
        private readonly SparePartCheckingStatusRepository _sparePartCheckingStatusRepository;

        public SparePartsController(SparePartRepository sparePartRepository,
            SparePartCheckingStatusRepository sparePartCheckingStatusRepository)
        {
            _sparePartRepository = sparePartRepository;
            _sparePartCheckingStatusRepository = sparePartCheckingStatusRepository;
        }

        [HttpGet]
        [Route("spare-part/status")]
        [Authorize(Roles = Role.All)]
        public JsonResult GetAllStatus()
        {
            var status = _sparePartCheckingStatusRepository.Get();
            return ResponseHelper<IEnumerable<SparepartStatus>>.OkResponse(status);
        }

        [HttpPost]
        [Route("spare-part/status")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult CreateStatus([FromBody] SparePartCheckingStatusRequest request)
        {
            var newStatus = new SparepartStatus
            {
                Acronym = request.Acronym,
                Name = request.Name
            };
            _sparePartCheckingStatusRepository.Insert(newStatus);
            _sparePartCheckingStatusRepository.Save();
            return ResponseHelper<IEnumerable<SparepartStatus>>.OkResponse(null, "Tạo trạng thái thành công!Ò");
        }
        
        [HttpGet]
        [Route("vehicle-group/{vehicleGroupId}/spare-part")]
        [Authorize(Roles = Role.Staff)]
        public JsonResult GetVehicleSparePart(int vehicleGroupId)
        {
            var vehicleSparePart = _sparePartRepository.GetByVehicleGroupId(vehicleGroupId);
            return ResponseHelper<IEnumerable<VehicleGroupSparepartItem>>.OkResponse(vehicleSparePart);
        }
        
        [HttpPost]
        [Route("vehicle-group/{vehicleGroupId}/spare-part")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult CreateVehicleSparePart([FromBody] SparePartRequest request, int vehicleGroupId)
        {
            var newSparePart = new VehicleGroupSparepartItem
            {    
                Description = request.Description,
                Name = request.Name,
                VehicleGroupId = vehicleGroupId
            };
            _sparePartRepository.Insert(newSparePart);
            _sparePartRepository.Save();
            return ResponseHelper<string>.OkResponse(null, "Thêm thành công!");
        }
        
        [HttpDelete]
        [Route("spare-part/{id}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult DeleteVehicleSparePart(int id)
        {
            var vehicleSparePart = _sparePartRepository.GetById(id);
            if (vehicleSparePart == null)
            {
                return ResponseHelper<string>.ErrorResponse(null, "Không tìm thấy!");
            }
            _sparePartRepository.Delete(vehicleSparePart);
            _sparePartRepository.Save();
            return ResponseHelper<string>.OkResponse(null, "Xóa thành công!");
        }
    }
}