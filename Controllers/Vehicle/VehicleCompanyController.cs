using main_service.Constants;
using main_service.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Vehicle
{
    [ApiController]
    [Route("api/vehicle-company")]
    public class VehicleCompanyController : ControllerBase
    {
        [HttpGet]
        public JsonResult Get()
        {
            return ResponseHelper<string>.OkResponse(null, "OK");
        }

        [HttpPost]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Create()
        {
            return ResponseHelper<string>.OkResponse(null, "OK");
        }
    }
}