using System;
using System.Net;
using System.Threading.Tasks;
using main_service.Helpers;
using main_service.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Utils
{
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly StorageManager _storageManager;

        public UtilsController(StorageManager storageManager)
        {
            _storageManager = storageManager;
        }
        
        [HttpPost]
        [Route("api/utils/image")]
        public async Task<JsonResult> UploadImage(IFormFile file)
        {
            try
            {
                string path = await _storageManager.UploadToAwsS3(file);
                return ResponseHelper<string>.OkResponse(path, "Upload thành công");
            }
            catch (Exception e)
            {
                return ResponseHelper<Exception>.ErrorResponse(e, e.Message, e.StackTrace, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}