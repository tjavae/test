using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CUDC.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Route("~/")]
        [Route("~/dos")]
        [Route("~/dos/{a}")]
        [Route("~/dos/{a}/{b}")]
        [Route("~/dos/{a}/{b}/{c}")]
        [Route("~/survey")]
        [Route("~/survey/{a}")]
        [Route("~/survey/{a}/{b}")]
        [Route("~/cat")]
        [Route("~/cat/{a}")]
        [Route("~/cat/{a}/{b}")]
        [Route("~/cat/{a}/{b}/{c}")]
        [Route("~/se")]
        [Route("~/se/{a}")]
        [Route("~/se/{a}/{b}")]
        [Route("~/se/{a}/{b}/{c}")]
        [Route("~/admin")]
        [Route("~/admin/{a}")]
        [Route("~/admin/{a}/{b}")]
        [Route("~/admin/{a}/{b}/{c}")]
        public IActionResult Index()
        {
            var version = $"{Assembly.GetExecutingAssembly().GetName().Version}";
            return View(model: version);
        }
    }
}
