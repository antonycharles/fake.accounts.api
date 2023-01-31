using Accounts.Core.DTO.Requests;
using Accounts.Core.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAuthenticationController : ControllerBase
{
    private readonly IAuthorizationHandler _authorizationHandler;
    public UserAuthenticationController(IAuthorizationHandler authorizationHandler)
    {
        _authorizationHandler = authorizationHandler ?? throw new ArgumentNullException(nameof(authorizationHandler));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateAsync(LoginRequest request)
    {
        try
        {
            await _authorizationHandler.ValidateAsync(request);
            return Ok();
        }
        catch(Exception e)
        {
            return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
