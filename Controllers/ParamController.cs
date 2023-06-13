using Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using Services;

namespace Controllers;

[ApiController]
[Route("/api/params")]
[Authorize(Roles = "TI, ENG", AuthenticationSchemes = "Bearer")]
public class ParamController : ControllerBase
{

    private readonly ParamService _paramService;

    public ParamController(IParamRepository paramRepository)
    {
        _paramService = new ParamService(paramRepository);
    }

    [HttpPost()]
    [ProducesResponseType(typeof(Param), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateParamRequest request)
    {
        try
        {
            var param = await _paramService.Create(request);
            return Created("Param Created Success", param);
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
}
