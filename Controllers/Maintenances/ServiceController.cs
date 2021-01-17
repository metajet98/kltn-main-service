using System;
using System.Collections.Generic;
using System.Linq;
using main_service.Constants;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Maintenances
{
    [ApiController]
    [Route("/api/")]
    public class ServiceController : ControllerBase
    {
        private readonly BranchServicePriceRepository _branchServicePriceRepository;
        private readonly MaintenanceServiceRepository _maintenanceServiceRepository;

        public ServiceController(MaintenanceServiceRepository maintenanceServiceRepository, BranchServicePriceRepository branchServicePriceRepository)
        {
            _maintenanceServiceRepository = maintenanceServiceRepository;
            _branchServicePriceRepository = branchServicePriceRepository;
        }
        
        [HttpGet]
        [Route("maintenance/service")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult GetServices([FromQuery] int vehicleGroupId)
        {
            var services = _maintenanceServiceRepository.Get(includeProperties: "VehicleGroup", filter: x => x.VehicleGroupId == vehicleGroupId);
            return ResponseHelper<IEnumerable<MaintenanceService>>.OkResponse(services);
        }
        
        [HttpPost]
        [Route("maintenance/service")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult CreateServices([FromBody] MaintenanceServiceRequest request)
        {
            var newMaintenanceService = new MaintenanceService
            {
                Name = request.Name,
                Description = request.Description,
                VehicleGroupId = request.VehicleGroupId,
                WarrantyOdo = request.WarrantyOdo,
                WarrantyPeriod = request.WarrantyPeriod
            };
            _maintenanceServiceRepository.Insert(newMaintenanceService);
            _maintenanceServiceRepository.Save();
            return ResponseHelper<string>.OkResponse(null, "Thêm thành công");
        }
        
        [HttpDelete]
        [Route("maintenance/service/{serviceId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult DeleteService(int serviceId)
        {
            try
            {
                _maintenanceServiceRepository.Delete(serviceId);
                _maintenanceServiceRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Xóa thành công");
            }
            catch (Exception e)
            {
                return ResponseHelper<string>.ErrorResponse(e.Message, "Lỗi khi xóa dịch vụ (Kiểm tra lại các chi nhánh con đang có giá của dịch vụ này");
            }
        }
        
        [HttpGet]
        [Route("branch/{branchId}/price")]
        [Authorize(Roles = Role.Staff)]
        public JsonResult GetBranchServices(int branchId, [FromQuery] int vehicleGroupId)
        {
            var branchServices = _branchServicePriceRepository.GetAllPriceByVehicleGroupId(vehicleGroupId, branchId);
            var result = branchServices.Select(x => new
            {
                Id = x.Id,
                Name = x.MaintenanceService.Name,
                Description = x.MaintenanceService.Description,
                LaborCost = x.LaborCost,
                SparePartPrice = x.SparePartPrice,
                ServiceId = x.MaintenanceService.Id
            }).ToList();
            return ResponseHelper<IEnumerable<object>>.OkResponse(result);
        }
        
        [HttpPost]
        [Route("branch/{branchId}/price")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult CreateOrUpdateBranchPrice([FromBody] BranchServicePriceRequest request, int branchId)
        {
            _branchServicePriceRepository.CreateOrUpdatePrice(request.MaintenanceServiceId, request.LaborCost, request.SparePartPrice, branchId);
            _branchServicePriceRepository.Save();
            return ResponseHelper<string>.OkResponse(null, "Thêm/Sửa thành công!");
        }
        
        [HttpDelete]
        [Route("branch/{branchId}/price/{priceId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult DeleteBranchServicePrice(int priceId)
        {
            try
            {
                _branchServicePriceRepository.Delete(priceId);
                _branchServicePriceRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Xóa thành công");
            }
            catch (Exception e)
            {
                return ResponseHelper<string>.ErrorResponse(e.Message, "Lỗi khi xóa giá dịch vụ");
            }
        }
    }
}