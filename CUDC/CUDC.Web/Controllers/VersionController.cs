using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CUDC.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"{Assembly.GetExecutingAssembly().GetName().Version}");
        }
    }
}
