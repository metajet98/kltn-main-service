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
        private readonly BranchStaffRepository _branchStaffRepository;
        private readonly BranchRepository _branchRepository;

        public StaffController(UserRepository userRepository, UserAuthRepository userAuthRepository, IEncryptionHelper encryptionHelper, BranchStaffRepository branchStaffRepository, BranchRepository branchRepository)
        {
            _userRepository = userRepository;
            _userAuthRepository = userAuthRepository;
            _encryptionHelper = encryptionHelper;
            _branchStaffRepository = branchStaffRepository;
            _branchRepository = branchRepository;
        }
        
        [HttpPost]
        [Route("maintenance")]
        [Authorize(Roles = Constants.Role.CenterManager)]
        public JsonResult CreateMaintenanceStaff([FromBody] UserRequest userRequest)
        {
            var newUser = new User
            {
                Address = userRequest.Address,
                Birthday = userRequest.Birthday, 
                Email = userRequest.Email,
                FullName = userRequest.FullName,
                PhoneNumber = userRequest.PhoneNumber,
                Role = Constants.Role.StaffMaintenance,
                CreatedDate = DateTime.Now
            };
            _userRepository.Insert(newUser);
            _userRepository.Save();
            var newUserAuth = _encryptionHelper.HashPassword(userRequest.Password, newUser.Id);
            _userAuthRepository.Insert(newUserAuth);
            _userAuthRepository.Save();

            return ResponseHelper<string>.OkResponse(null, "Tạo tài khoản thành công!");
        }
        
        [HttpPost]
        [Route("desk")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult CreateDeskStaff([FromBody] UserRequest userRequest)
        {
            var newUser = new User
            {
                Address = userRequest.Address,
                Birthday = userRequest.Birthday, 
                Email = userRequest.Email,
                FullName = userRequest.FullName,
                PhoneNumber = userRequest.PhoneNumber,
                Role = Constants.Role.StaffDesk,
                CreatedDate = DateTime.Now
            };
            _userRepository.Insert(newUser);
            _userRepository.Save();
            var newUserAuth = _encryptionHelper.HashPassword(userRequest.Password, newUser.Id);
            _userAuthRepository.Insert(newUserAuth);
            _userAuthRepository.Save();
            
            return ResponseHelper<string>.OkResponse(null, "Tạo tài khoản thành công!");
        }

        [HttpGet]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult GetAll()
        {
            var staff = _branchStaffRepository.Get(includeProperties: "Branch,Staff");
            return ResponseHelper<IEnumerable<BranchStaff>>.OkResponse(staff);
        }
        
        [HttpPost]
        [Route("{staffId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult ModifyStaff(int staffId, [FromBody] BranchStaffRequest request)
        {
            var branch = _branchRepository.GetById(request.BranchId);
            if(branch == null) return ResponseHelper<string>.ErrorResponse(null, "Chi nhánh không tồn tại!");
            var staff = _branchStaffRepository.Get(x => x.StaffId.Equals(staffId)).FirstOrDefault();
            if (staff != null)
            {
                staff.BranchId = request.BranchId;
                _branchStaffRepository.Update(staff);
                _branchStaffRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Thanh đổi chi nhánh cho nhân viên thành công!");
            }
            else
            {
                var newBranchStaff = new BranchStaff
                {
                    BranchId = request.BranchId,
                    StaffId = staffId
                };
                _branchStaffRepository.Insert(newBranchStaff);
                _branchStaffRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Thêm nhân viên vào chi nhánh thành công!");
            }
        }
    }
}