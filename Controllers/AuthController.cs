using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using Services;

namespace Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _authService = new AuthService(userRepository, configuration);
    }

    [HttpPost("/login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            return Ok(await _authService.Login(request));
        }
        catch (ErrorExceptions e)
        {
            return e.Error["status"] switch
            {
                400 => BadRequest(e.Error),
                404 => NotFound(e.Error),
                _ => StatusCode(500, e.Error)
            };
        }
    }
}