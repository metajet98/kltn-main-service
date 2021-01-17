using System;
using System.Net;
using System.Threading.Tasks;
using main_service.Helpers;
using main_service.Repositories;
using main_service.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Controllers.Utils
{
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly StorageManager _storageManager;
        private readonly StatisticRepository _statisticRepository;

        public UtilsController(StorageManager storageManager, StatisticRepository statisticRepository)
        {
            _storageManager = storageManager;
            _statisticRepository = statisticRepository;
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
        
        [HttpGet]
        [Route("api/utils/statistic")]
        public async Task<JsonResult> Statistic([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _statisticRepository.GetStatistic(startDate, endDate);
                return ResponseHelper<object>.OkResponse(result);
            }
            catch (Exception e)
            {
                return ResponseHelper<Exception>.ErrorResponse(e, e.Message, e.StackTrace, (int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpGet]
        [Route("api/utils/current-statistic")]
        public async Task<JsonResult> CurrentStatistic()
        {
            try
            {
                var result = await _statisticRepository.GetCurrentStatistic();
                return ResponseHelper<object>.OkResponse(result);
            }
            catch (Exception e)
            {
                return ResponseHelper<Exception>.ErrorResponse(e, e.Message, e.StackTrace, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}