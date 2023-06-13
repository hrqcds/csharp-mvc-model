using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Repositories;
using Exceptions;
using Utils;
using Microsoft.AspNetCore.Authorization;
using Generics;

namespace Controllers;

[ApiController]
[Route("/api/users")]
[Authorize(Roles = "TI, ADM", AuthenticationSchemes = "Bearer")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(IUserRepository userRepository)
    {
        _userService = new UserService(userRepository);
    }

    [HttpGet()]
    [ProducesResponseType(typeof(DataResponse<User>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] UserQueryRequest query)
    {
        return Ok(await _userService.GetALL(query));
    }

    [HttpPost()]
    [ProducesResponseType(typeof(CreateUserResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        try
        {
            var u = await _userService.Create(request);
            return Created("User Created Success", u);
        }
        catch (ErrorExceptions e)
        {
            return e.Error["status"] switch
            {
                400 => BadRequest(e.Error),
                _ => StatusCode(500, e.Error)
            };
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            return Ok(await _userService.GetUserById(id));
        }
        catch (ErrorExceptions e)
        {
            return e.Error["status"] switch
            {
                404 => NotFound(e.Error),
                _ => StatusCode(500, e.Error)
            };
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(string id, UpdateUserRequest request)
    {
        try
        {
            return Ok(await _userService.Update(id, request));
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

    [HttpPut("enable/{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> Enable(string id)
    {
        try
        {
            return Ok(await _userService.Enable(id));
        }
        catch (ErrorExceptions e)
        {
            return e.Error["status"] switch
            {
                404 => NotFound(e.Error),
                _ => StatusCode(500, e.Error)
            };
        }
    }
    [HttpPut("disable/{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> Disable(string id)
    {
        try
        {
            return Ok(await _userService.Disable(id));
        }
        catch (ErrorExceptions e)
        {
            return e.Error["status"] switch
            {
                404 => NotFound(e.Error),
                _ => StatusCode(500, e.Error)
            };
        }
    }
}

