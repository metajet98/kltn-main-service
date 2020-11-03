using main_service.Constants;
using main_service.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = Role.CenterManager)]
        public JsonResult Get()
        {
            return ResponseHelper<string>.OkResponse(null, "OK");
        }
    }
}