using Accounts.Core.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenKeyController : ControllerBase
{
    private readonly ITokenKeyHandler _tokenKeyHandler;

    public TokenKeyController(ITokenKeyHandler tokenKeyHandler)
    {
        _tokenKeyHandler = tokenKeyHandler ?? throw new ArgumentNullException(nameof(tokenKeyHandler));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IList<JsonWebKey>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public IActionResult GetPublic()
    {
        try
        {
            var result = _tokenKeyHandler.GetPublicKey();
            return Ok(result);
        }
        catch (Exception e)
        {
            return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

}
