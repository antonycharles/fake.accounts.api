using Accounts.Application.Exceptions;
using Accounts.Core.DTO.Requests;
using Accounts.Core.DTO.Responses;
using Accounts.Core.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAuthenticationController : ControllerBase
{
    private readonly IUserAuthorizationHandler _userAuthorizationHandler;
    public UserAuthenticationController(IUserAuthorizationHandler authorizationHandler)
    {
        _userAuthorizationHandler = authorizationHandler ?? throw new ArgumentNullException(nameof(authorizationHandler));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AuthenticationAsync(LoginRequest request)
    {
        try
        {
            var result = await _userAuthorizationHandler.AuthenticationAsync(request);
            return Ok(result);
        }
        catch(ConflictException e)
        {
            return Problem(e.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch(Exception e)
        {
            return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost("ssignup")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(RegisterRequest request)
    {
        try
        {
            var result = await _userAuthorizationHandler.RegisterAsync(request);
            return Created("",result);
        }
        catch(ConflictException e)
        {
            return Problem(e.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch(Exception e)
        {  
            return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
