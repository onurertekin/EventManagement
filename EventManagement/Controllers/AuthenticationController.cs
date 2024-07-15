using Contract.Request.Authentication;
using Contract.Response.AuthenticationResponse;
using DatabaseModel;
using DomainService.Operations;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [ApiController]
    [Route("event-management-api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationOperations authenticationOperations;

        public AuthenticationController(AuthenticationOperations authenticationOperations)
        {
            this.authenticationOperations = authenticationOperations;
        }

        [HttpPost("authentication")]
        public ActionResult<AuthenticationResponse> Authentication([FromBody] AuthenticationRequest request)
        {
            var token = authenticationOperations.Authentication(request.email, request.password);
            var response = new AuthenticationResponse();
            response.token = token;
            return new JsonResult(response);
        }
    }
}
