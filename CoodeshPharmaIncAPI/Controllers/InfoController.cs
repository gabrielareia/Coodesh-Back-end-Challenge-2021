using Microsoft.AspNetCore.Mvc;

namespace CoodeshPharmaIncAPI.Controllers
{
    [ApiController]
    [Route("api/")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public IActionResult GetInformation()
        {
            return Ok("REST Back-end Challenge 20201209 Running");
        }
    }
}
