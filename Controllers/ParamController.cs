using System.Globalization;
using System.Text;
using Exceptions;
using Generics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using Services;
using Utils;

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

    [HttpGet()]
    [ProducesResponseType(typeof(DataResponse<Param>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] ParamQueryRequest query)
    {
        return Ok(await _paramService.GetAll(query));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Param), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            return Ok(await _paramService.GetById(id));
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

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Param), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(string id, UpdateParamRequest request)
    {
        try
        {
            return Ok(await _paramService.Update(id, request));
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

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Param), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            return Ok(await _paramService.Delete(id));
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

    [HttpPost("import")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        try
        {
            await _paramService.Import(file);
            return Ok("Import Success");
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

internal class CsvReader
{
    private StreamReader reader;
    private object invariantCulture;

    public CsvReader(StreamReader reader, object invariantCulture)
    {
        this.reader = reader;
        this.invariantCulture = invariantCulture;
    }
}