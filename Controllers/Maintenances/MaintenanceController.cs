using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace main_service.Controllers.Maintenances
{
    [ApiController]
    [Route("/api/maintenance")]
    public class MaintenanceController : ControllerBase
    {
        private readonly MaintenanceRepository _maintenanceRepository;
        private readonly UserRepository _userRepository;
        private readonly UserVehicleRepository _userVehicleRepository;
        private readonly PdfService _pdfService;

        private readonly NotificationsRepository _notificationsRepository;
        private readonly FcmService _fcmService;

        public MaintenanceController(MaintenanceRepository maintenanceRepository, UserRepository userRepository,
            NotificationsRepository notificationsRepository, FcmService fcmService,
            UserVehicleRepository userVehicleRepository, PdfService pdfService)
        {
            _maintenanceRepository = maintenanceRepository;
            _userRepository = userRepository;
            _notificationsRepository = notificationsRepository;
            _fcmService = fcmService;
            _userVehicleRepository = userVehicleRepository;
            _pdfService = pdfService;
        }

        [HttpGet]
        [Authorize(Roles = Role.All)]
        public JsonResult Query([FromQuery] int? userVehicleId, [FromQuery] int? staffId, [FromQuery] int? branchId,
            [FromQuery] DateTime? date)
        {
            return ResponseHelper<IEnumerable<Maintenance>>.OkResponse(
                _maintenanceRepository.Query(userVehicleId, staffId, branchId, date));
        }

        [HttpPost]
        [Authorize(Roles = Role.StaffMaintenance)]
        public JsonResult CreateMaintenance([FromBody] MaintenanceRequest request)
        {
            try
            {
                var staff = _userRepository
                    .Get(x => x.Id.Equals(User.Identity.GetId()), includeProperties: "Branch").FirstOrDefault();
                if (staff?.BranchId != null)
                {
                    var newMaintenance = new Maintenance
                    {
                        BranchId = staff.BranchId,
                        Notes = request.Notes,
                        Odometer = request.Odometer,
                        CreatedDate = DateTime.Now,
                        ReceptionStaffId = staff.Id,
                        UserVehicleId = request.UserVehicleId,
                        Status = 0,
                        MotorWash = request.MotorWash,
                        SparepartBack = request.SparepartBack,
                        Title = request.Title,
                    };
                    _maintenanceRepository.Insert(newMaintenance);
                    _maintenanceRepository.Save();

                    if (request.Images != null)
                    {
                        _maintenanceRepository.InsertMaintenanceImages(newMaintenance.Id, request.Images);
                    }

                    var userId = _userVehicleRepository.GetById(request.UserVehicleId)?.UserId;

                    if (userId != null)
                    {
                        var data = FcmData.CreateFcmData("new_maintenance", null);
                        var notify = FcmData.CreateFcmNotification(
                            "Bạn vừa được tạo một lượt bảo dưỡng",
                            "Bạn có thể xem chi tiết bảo dưỡng tại màn hình xe",
                            null);
                        _fcmService.SendMessage(userId.Value, data, notify);
                        _notificationsRepository.Insert(new Notification
                        {
                            UserId = userId,
                            Description = "Bạn vừa được tạo một lượt bảo dưỡng",
                            Title = "Bạn có thể xem chi tiết bảo dưỡng tại màn hình xe",
                            Activity = "new_maintenance",
                            CreatedDate = DateTime.Now,
                        });
                        _notificationsRepository.Save();
                    }

                    return ResponseHelper<Maintenance>.OkResponse(newMaintenance, "Tạo lượt bảo dưỡng thành công");
                }
                else
                {
                    return ResponseHelper<object>.ErrorResponse(null, "Bạn hiện tại không ở chi nhánh nào cả!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
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
        [Authorize(Roles = Role.All)]
        public JsonResult GetMaintenance(int maintenanceId)
        {
            var maintenance = _maintenanceRepository.GetMaintenance(maintenanceId);
            return ResponseHelper<Maintenance>.OkResponse(maintenance);
        }

        [HttpGet]
        [Route("{maintenanceId}/pdf")]
        [Authorize(Roles = Role.All)]
        public async Task<JsonResult> GetMaintenancePdf(int maintenanceId)
        {
            try
            {
                var maintenance = _maintenanceRepository.GetMaintenanceForPdf(maintenanceId);
                var result = await _pdfService.MaintenancePdf(maintenance);
                return ResponseHelper<string>.OkResponse(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpPost]
        [Route("{maintenanceId}/check")]
        [Authorize(Roles = Role.StaffMaintenance)]
        public JsonResult UpdateMaintenanceCheck(int maintenanceId, [FromBody] SparePartMaintenanceCheckRequest request)
        {
            try
            {
                var result =
                    _maintenanceRepository.InsertMaintenanceChecks(maintenanceId, request.SparePartMaintenanceChecks);
                return result
                    ? ResponseHelper<List<SparepartCheckDetail>>.OkResponse(null, "Cập nhật thành công")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpPost]
        [Route("{maintenanceId}/bill")]
        [Authorize(Roles = Role.Staff)]
        public JsonResult UpdateMaintenanceBill(int maintenanceId, [FromBody] MaintenanceBillRequest request)
        {
            try
            {
                var result =
                    _maintenanceRepository.InsertMaintenanceBill(request, maintenanceId);
                return result
                    ? ResponseHelper<List<SparepartCheckDetail>>.OkResponse(null, "Cập nhật thành công")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpPost]
        [Route("{maintenanceId}/images")]
        public JsonResult AddMaintenanceImage(int maintenanceId, [FromBody] ImageRequest image)
        {
            try
            {
                var result =
                    _maintenanceRepository.AddMaintenanceImage(maintenanceId, image.ImageUrl);
                return result
                    ? ResponseHelper<dynamic>.OkResponse(null, "Cập nhật thành công")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpPost]
        [Route("{maintenanceId}/start")]
        [Authorize(Roles = Role.StaffMaintenance)]
        public JsonResult StartMaintenance(int maintenanceId)
        {
            try
            {
                var maintainerId = User.Identity.GetId();
                var maintainer = _userRepository.GetById(maintainerId);
                if (maintainer == null || maintainer.Role != Role.StaffMaintenance)
                {
                    return ResponseHelper<dynamic>.ErrorResponse(null,
                        "Chỉ có nhân viên bảo dưỡng mới tiến hành được!");
                }

                var result = _maintenanceRepository.UpdateMaintainer(maintenanceId, maintainerId);
                if (result)
                {
                    var maintenance = _maintenanceRepository.GetById(maintenanceId);
                    return ResponseHelper<dynamic>.OkResponse(maintenance);
                }
                else
                {
                    return ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpPost]
        [Route("{maintenanceId}/finish")]
        [Authorize(Roles = Role.StaffMaintenance)]
        public JsonResult FinishMaintenance(int maintenanceId)
        {
            try
            {
                var maintainerId = User.Identity.GetId();
                var maintainer = _userRepository.GetById(maintainerId);
                if (maintainer == null || maintainer.Role != Role.StaffMaintenance)
                {
                    return ResponseHelper<dynamic>.ErrorResponse(null,
                        "Chỉ có nhân viên bảo dưỡng mới tiến hành được!");
                }

                var result = _maintenanceRepository.FinishMaintenance(maintenanceId);

                var maintenance = _maintenanceRepository.GetMaintenance(maintenanceId);
                var userId = maintenance.UserVehicle.UserId;
                if (userId != null)
                {
                    var data = FcmData.CreateFcmData("finish_maintenance", null);
                    var notify = FcmData.CreateFcmNotification(
                        "Lượt bảo dưỡng vừa kết thúc",
                        "Bạn có thể xem chi tiết bảo dưỡng tại màn hình xe",
                        null);
                    _fcmService.SendMessage(userId.Value, data, notify);
                    var newNotification = new Notification
                    {
                        UserId = userId.Value,
                        Description = "Bạn có thể xem chi tiết bảo dưỡng tại màn hình xe của tôi",
                        Title = "Lượt bảo dưỡng vừa kết thúc",
                        Activity = "finish_maintenance",
                        CreatedDate = DateTime.Now,
                    };
                    _notificationsRepository.Insert(newNotification);
                    _notificationsRepository.Save();
                }

                return result
                    ? ResponseHelper<dynamic>.OkResponse(null, "Cập nhật thành công")
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
        [Route("{maintenanceId}/images/{imageId}")]
        [Authorize(Roles = Role.Staff)]
        public JsonResult RemoveMaintenanceImage(int maintenanceId, int imageId)
        {
            try
            {
                var result =
                    _maintenanceRepository.DeleteMaintenanceImage(imageId);
                return result
                    ? ResponseHelper<dynamic>.OkResponse(null, "Cập nhật thành công")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpPost]
        [Route("{maintenanceId}/schedule")]
        [Authorize(Roles = Role.Staff)]
        public JsonResult AddMaintenanceSchedule(int maintenanceId, [FromBody] MaintenanceScheduleRequest request)
        {
            try
            {
                var result =
                    _maintenanceRepository.InsertSchedule(maintenanceId, request);
                return result
                    ? ResponseHelper<List<SparepartCheckDetail>>.OkResponse(null, "Cập nhật thành công")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpPost]
        [Route("{maintenanceId}/review")]
        [Authorize(Roles = Role.User)]
        public JsonResult AddReview(int maintenanceId, [FromBody] ReviewRequest request)
        {
            try
            {
                var userId = User.Identity.GetId();
                var result =
                    _maintenanceRepository.InsertReview(maintenanceId, request, userId);
                return result
                    ? ResponseHelper<List<SparepartCheckDetail>>.OkResponse(null, "Cập nhật thành công")
                    : ResponseHelper<dynamic>.ErrorResponse(null, "Có lỗi xảy ra, vui lòng thử lại!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
    }
}