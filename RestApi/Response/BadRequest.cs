using System.Net;
using main_service.RestApi.Response.Base;

namespace main_service.RestApi.Response
{
    public class BadRequest : ResponseWrapped<object>
    {
        public BadRequest(string message, object log) : base((int)HttpStatusCode.BadRequest, message, false, log) {}
    }
}