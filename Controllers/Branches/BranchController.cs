using System;
using System.Collections.Generic;
using main_service.Constants;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories;
using main_service.RestApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Branches
{
    [Route("/api/branch/")]
    public class BranchController : ControllerBase
    {
        private readonly BranchRepository _branchRepository;

        public BranchController(BranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        [HttpPost]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Create([FromBody] BranchRequest branchRequest)
        {
            try
            {
                var newBranch = new Branch
                {
                    Address = branchRequest.Address,
                    Latitude = branchRequest.Latitude,
                    Longitude = branchRequest.Longitude,
                    Name = branchRequest.Name,
                    CreatedDate = DateTime.Now,
                    Logo = branchRequest.Logo
                };
            
                _branchRepository.Insert(newBranch);
                _branchRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Thêm chi nhánh thành công");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            try
            {
                var branches = _branchRepository.Get();
                return ResponseHelper<IEnumerable<Branch>>.OkResponse(branches);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }
        
        [HttpGet]
        [Route("{branchId}")]
        public JsonResult Get(int branchId)
        {
            try
            {
                var branch = _branchRepository.GetById(branchId);
                return ResponseHelper<Branch>.OkResponse(branch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ResponseHelper<string>.ErrorResponse(null,
                    "Có lỗi xảy ra, vui lòng thử lại!");
            }
        }

        [HttpDelete]
        [Route("{branchId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Delete(int branchId)
        {
            try
            {
                var targetBranch = _branchRepository.GetById(branchId);
                if (targetBranch == null)
                {
                    return ResponseHelper<string>.ErrorResponse(null, "Không tìm thấy chi nhánh cần xóa");
                }
                _branchRepository.Delete(targetBranch);
                _branchRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Xóa thành công");
            }
            catch (Exception e)
            {
                return ResponseHelper<string>.ErrorResponse(e.Message, "Lỗi khi xóa chi nhánh");
            }
        }
    }
}