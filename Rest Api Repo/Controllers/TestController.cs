using Microsoft.AspNetCore.Mvc;

namespace Rest_Api_Repo.Controllers
{
    public class TestController : Controller
    {
        [HttpGet("api/user")]
        public IActionResult Get()
        {
            return Ok(new { name = "Nadir" });
        }

    }
}
