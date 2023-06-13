using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{

    [HttpGet()]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok("Server is running");
    }
}
