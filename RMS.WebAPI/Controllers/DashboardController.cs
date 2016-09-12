using System.Web.Http;

namespace RMS.WebAPI.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("")]
        public IHttpActionResult Index()
        {
            return Ok("Authorised Content");
        }
    }
}
