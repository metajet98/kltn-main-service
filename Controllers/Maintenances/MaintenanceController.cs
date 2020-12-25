using System;
using System.Collections.Generic;
using System.Linq;
using main_service.Constants;
using main_service.Databases;
using main_service.Extensions;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Maintenances
{
    [ApiController]
    [Route("/api/maintenance")]
    public class MaintenanceController : ControllerBase
    {
        private readonly MaintenanceRepository _maintenanceRepository;
        private readonly BranchStaffRepository _branchStaffRepository;
        private readonly UserRepository _userRepository;

        public MaintenanceController(MaintenanceRepository maintenanceRepository, UserRepository userRepository,
            BranchStaffRepository branchStaffRepository)
        {
            _maintenanceRepository = maintenanceRepository;
            _userRepository = userRepository;
            _branchStaffRepository = branchStaffRepository;
        }

        [HttpGet]
        [Authorize(Roles = Role.All)]
        public JsonResult GetUserBikeMaintenances([FromQuery] int userVehicleId)
        {
            var result =
                _maintenanceRepository.Get(x => x.UserVehicleId.Equals(userVehicleId), includeProperties: "Branch");
            return ResponseHelper<IEnumerable<Maintenance>>.OkResponse(result);
        }

        [HttpPost]
        [Authorize(Roles = Role.StaffMaintenance)]
        public JsonResult CreateMaintenance([FromBody] MaintenanceRequest request)
        {
            var staff = _branchStaffRepository
                .Get(x => x.StaffId.Equals(User.Identity.GetId()), includeProperties: "Branch").FirstOrDefault();
            if (staff?.Branch != null)
            {
                var newMaintenance = new Maintenance
                {
                    BranchId = staff.BranchId,
                    Notes = request.Notes,
                    Odometer = request.Odometer,
                    CreatedDate = DateTime.Now,
                    ReceptionStaffId = staff.StaffId,
                    UserVehicleId = request.UserVehicleId,
                };
                _maintenanceRepository.Insert(newMaintenance);
                _maintenanceRepository.Save();

                if (request.Images != null)
                {
                    _maintenanceRepository.InsertMaintenanceImages(newMaintenance.Id, request.Images);
                }
                return ResponseHelper<Maintenance>.OkResponse(newMaintenance, "Tạo lượt bảo dưỡng thành công");
            }
            else
            {
                return ResponseHelper<object>.ErrorResponse(null, "Bạn hiện tại không ở chi nhánh nào cả!");
            }
        }
        
        [HttpGet]
        [Route("{maintenanceId}/all")]
        [Authorize(Roles = Role.All)]
        public JsonResult GetMaintenanceAllDetail(int maintenanceId)
        {
            var maintenance = _maintenanceRepository.GetMaintenanceAllDetail(maintenanceId);
            return ResponseHelper<Maintenance>.OkResponse(maintenance);
        }
        
        [HttpGet]
        [Route("{maintenanceId}")]
        [Authorize(Roles = Role.User)]
        public JsonResult GetMaintenance(int maintenanceId)
        {
            var maintenance = _maintenanceRepository.GetMaintenance(maintenanceId);
            return ResponseHelper<Maintenance>.OkResponse(maintenance);
        }

        [HttpPost]
        [Route("{maintenanceId}/check")]
        [Authorize(Roles = Role.StaffMaintenance)]
        public JsonResult UpdateMaintenanceCheck(int maintenanceId, [FromBody] SparePartMaintenanceCheckRequest request)
        {
            var result =
                _maintenanceRepository.InsertMaintenanceCheck(request.StatusId, request.VehicleSparePartId,
                    maintenanceId);
            return result
                ? ResponseHelper<List<SparepartCheckDetail>>.OkResponse(null, "Cập nhật thành công")
                : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
        }
        
        [HttpPost]
        [Route("{maintenanceId}/bill")]
        public JsonResult UpdateMaintenanceBill(int maintenanceId, [FromBody] MaintenanceBillRequest request)
        {
            var result =
                _maintenanceRepository.InsertMaintenanceBill(request, maintenanceId);
            return result
                ? ResponseHelper<List<SparepartCheckDetail>>.OkResponse(null, "Cập nhật thành công")
                : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
        }
        
        [HttpPost]
        [Route("{maintenanceId}/images")]
        public JsonResult AddMaintenanceImage(int maintenanceId, [FromBody] ImageRequest image)
        {
            var result =
                _maintenanceRepository.AddMaintenanceImage(maintenanceId, image.ImageUrl);
            return result
                ? ResponseHelper<dynamic>.OkResponse(null, "Cập nhật thành công")
                : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
        }
        
        [HttpDelete]
        [Route("{maintenanceId}/images/{imageId}")]
        public JsonResult RemoveMaintenanceImage(int maintenanceId, int imageId)
        {
            var result =
                _maintenanceRepository.DeleteMaintenanceImage(imageId);
            return result
                ? ResponseHelper<dynamic>.OkResponse(null, "Cập nhật thành công")
                : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
        }
    }
}