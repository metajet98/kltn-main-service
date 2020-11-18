using System.Net;
using main_service.RestApi.Response.Base;
using Microsoft.AspNetCore.Mvc;

namespace main_service.Helpers
{
    public static class ResponseHelper<T>
    {
        public static JsonResult OkResponse(T data, string message = null)
        {
            var response = new
            {
                Data = data,
                Message = message,
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
            };
            return new JsonResult(response) {StatusCode = (int) HttpStatusCode.OK};
        }
        
        public static JsonResult ErrorResponse(T data, string message = null, object traceLog = null, int statusCode = (int)HttpStatusCode.BadRequest)
        {
            var response = new
            {
                Data = data,
                Message = message,
                IsSuccess = false,
                StatusCode = statusCode,
                TraceLog = traceLog,
            };
            return new JsonResult(response) {StatusCode = (int) HttpStatusCode.BadRequest};
        }
        
        public static JsonResult UnauthorizedResponse(T data, string message = null, object traceLog = null, int statusCode = (int)HttpStatusCode.Unauthorized)
        {
            var response = new
            {
                Data = data,
                Message = message,
                IsSuccess = false,
                StatusCode = statusCode,
                TraceLog = traceLog,
            };
            return new JsonResult(response) {StatusCode = (int) HttpStatusCode.Unauthorized};
        }
    }
}