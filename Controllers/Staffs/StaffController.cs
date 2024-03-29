using System;
using System.Collections.Generic;
using System.Linq;
using main_service.Constants;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using main_service.Utils.EncryptionHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Staffs
{
    [ApiController]
    [Route("/api/staff")]
    public class StaffController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly UserAuthRepository _userAuthRepository;
        private readonly IEncryptionHelper _encryptionHelper;
        private readonly BranchRepository _branchRepository;

        public StaffController(UserRepository userRepository, UserAuthRepository userAuthRepository,
            IEncryptionHelper encryptionHelper, BranchRepository branchRepository)
        {
            _userRepository = userRepository;
            _userAuthRepository = userAuthRepository;
            _encryptionHelper = encryptionHelper;
            _branchRepository = branchRepository;
        }

        [HttpPost]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult CreateStaff([FromBody] UserRequest userRequest)
        {
            try
            {
                var newUser = new User
                {
                    Address = userRequest.Address,
                    Birthday = userRequest.Birthday,
                    Email = userRequest.Email,
                    FullName = userRequest.FullName,
                    PhoneNumber = userRequest.PhoneNumber,
                    Role = userRequest.Role,
                    CreatedDate = DateTime.Now,
                    BranchId = userRequest.BranchId
                };
                _userRepository.Insert(newUser);
                _userRepository.Save();
                var newUserAuth = _encryptionHelper.HashPassword(userRequest.Password, newUser.Id);
                _userAuthRepository.Insert(newUserAuth);
                _userAuthRepository.Save();

                return ResponseHelper<string>.OkResponse(null, "Tạo tài khoản thành công!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpGet]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult GetAll([FromQuery] int branchId)
        {
            var staff = _userRepository.Get(
                x => (x.Role.Equals(Role.StaffMaintenance) || x.Role.Equals(Role.StaffDesk)) &&
                     x.BranchId.Equals(branchId), includeProperties: "Branch");
            return ResponseHelper<IEnumerable<User>>.OkResponse(staff);
        }

        [HttpPost]
        [Route("{staffId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult ModifyStaff(int staffId, [FromBody] BranchStaffRequest request)
        {
            try
            {
                var branch = _branchRepository.GetById(request.BranchId);
                if (branch == null) return ResponseHelper<string>.ErrorResponse(null, "Chi nhánh không tồn tại!");
                var staff = _userRepository.Get(x => x.Id.Equals(staffId)).FirstOrDefault();
                if (staff == null) return ResponseHelper<string>.ErrorResponse(null, "Nhân viên!");

                staff.BranchId = request.BranchId;
                _userRepository.Update(staff);
                _userRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Thanh đổi chi nhánh cho nhân viên thành công!");
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