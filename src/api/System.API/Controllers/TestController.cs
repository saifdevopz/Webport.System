using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController() : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public ActionResult Verify()
        {
            return Ok("You are authorized");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public ActionResult VerifyAdmin()
        {
            return Ok("You are authorized as Admin");
        }

        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public ActionResult VerifyUser()
        {
            return Ok("You are authorized as User");
        }
    }
}
