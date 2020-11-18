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
    [Route("api/vehicle-company/")]
    public class VehicleCompanyController : ControllerBase
    {
        private readonly CompanyRepository _companyRepository;
        public VehicleCompanyController(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        [HttpGet]
        public JsonResult Get()
        {
            var companies = _companyRepository.Get();
            return ResponseHelper<IEnumerable<VehicleCompany>>.OkResponse(companies);
        }

        [HttpPost]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Create([FromBody] VehicleCompanyRequest companyRequest)
        {
            _companyRepository.Insert(new VehicleCompany
            {
                Logo = companyRequest.Logo,
                Name = companyRequest.Name
            });
            _companyRepository.Save();
            return ResponseHelper<string>.OkResponse(null, "Thêm công ty thành công");
        }
        
        [HttpDelete]
        [Route("{companyId}")]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Delete(int companyId)
        {
            try
            {
                var targetCompany = _companyRepository.GetById(companyId);
                if (targetCompany == null)
                {
                    return ResponseHelper<string>.ErrorResponse(null, "Không tìm thấy công ty cần xóa");
                }
                _companyRepository.Delete(targetCompany);
                _companyRepository.Save();
                return ResponseHelper<string>.OkResponse(null, "Xóa thành công");
            }
            catch (Exception e)
            {
                return ResponseHelper<string>.ErrorResponse(e.Message, "Lỗi khi xóa công ty");
            }
        }
    }
}