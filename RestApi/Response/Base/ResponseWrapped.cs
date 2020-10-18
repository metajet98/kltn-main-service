using System.Text.Json.Serialization;

namespace main_service.RestApi.Response.Base
{
    public abstract class ResponseWrapped<T>
    {
        [JsonPropertyName("status_code")] private int StatusCode { get; set; }
        [JsonPropertyName("message")] private string Message { get; set; }
        [JsonPropertyName("data")] private T Data { get; set; }
        [JsonPropertyName("is_success")] private bool IsSuccess { get; set; }
        [JsonPropertyName("trace_log")] private object TraceLog { get; set; }

        protected ResponseWrapped(int statusCode, string message, T data, bool isSuccess)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
            IsSuccess = isSuccess;
        }

        protected ResponseWrapped(int statusCode, string message, bool isSuccess, object traceLog)
        {
            StatusCode = statusCode;
            Message = message;
            IsSuccess = isSuccess;
            TraceLog = traceLog;
        }
    }
}