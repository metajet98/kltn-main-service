using main_service.EFEntities.Users;
using main_service.Repositories.User;
using main_service.RestApi.Response;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers
{
    [ApiController]
    [Route("api/initial")]
    public class InitialController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        
        public InitialController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        [HttpPost]
        [Route("")]
        public JsonResult Post()
        {
            _userRepository.Insert(new User
            {
                RoleId = 1,
                PhoneNumber = "0123456789"
            });
            
            _userRepository.Save();
            return new JsonResult(new OkResponse<string>("OK"));
        }
    }
}