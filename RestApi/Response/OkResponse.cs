using System.Net;
using main_service.RestApi.Response.Base;

namespace main_service.RestApi.Response
{
    public class OkResponse<T> : ResponseWrapped<T>
    {
        public OkResponse(T data, string message = null) : base((int) HttpStatusCode.OK, message, data, true) { }
    }
}