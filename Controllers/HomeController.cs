using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{

    [HttpGet()]
    public IActionResult Get()
    {
        return Ok("Server is running");
    }
}
