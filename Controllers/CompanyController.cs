using System.Collections.Generic;
using main_service.Databases;
using main_service.Helpers;
using main_service.Repositories.Company;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompanyController
    {
        private readonly CompanyRepository _companyRepository;

        public CompanyController(CompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpPost]
        public JsonResult Create(VehicleCompany vehicleCompany)
        {
            _companyRepository.Insert(vehicleCompany);
            _companyRepository.Save();
            return ResponseHelper<VehicleCompany>.OkResponse(vehicleCompany);
        }

        [HttpGet]
        public JsonResult GetCompanies()
        {
            var companies = _companyRepository.Get();
            return ResponseHelper<IEnumerable<VehicleCompany>>.OkResponse(companies);
        }
    }
}