using Microsoft.AspNetCore.Mvc;

namespace ImageTransform.Api.Controllers
{
    [Route("_status")]
    public class StatusController : ControllerBase
    {
        [Route("ping")]
        public JsonResult Ping()
        {
            return new JsonResult(new OkResult());
        }

        private class OkResult
        {
            public string Status { get; } = "Ok";
        }
    }
}