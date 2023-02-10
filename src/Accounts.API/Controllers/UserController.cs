using Accounts.Core.DTO.Requests;
using Accounts.Core.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserAuthorizationHandler _userAuthorizationHandler;
    
    public UserController(IUserAuthorizationHandler authorizationHandler)
    {
        _userAuthorizationHandler = authorizationHandler ?? throw new ArgumentNullException(nameof(authorizationHandler));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(RegisterRequest request)
    {
        try
        {
            await _userAuthorizationHandler.RegisterAsync(request);
            return Created("",new{});
        }
        catch(Exception e)
        {  
            return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
