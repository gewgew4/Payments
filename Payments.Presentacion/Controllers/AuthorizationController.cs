using Microsoft.AspNetCore.Mvc;
using Payments.Application.Dtos;
using Payments.Application.Helpers;
using Payments.Application.Services.Interfaces;

namespace Payments.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController(IAuthorizationService authorizationService) : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService = authorizationService;

        [HttpPost]
        public async Task<ActionResult<AuthorizationDto>> Authorize(AuthorizationRequestDto request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            var result = await _authorizationService.Authorize(request);

            if (!result.Success)
                return WebApiResponse.GetErrorResponse(result);

            return Ok(result.Data);
        }

        [HttpPatch("Confirm")]
        public async Task<ActionResult<AuthorizationDto>> Confirm(ConfirmationRequestDto request)
        {
            if (!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            var result = await _authorizationService.Confirm(request);

            if (!result.Success)
                return WebApiResponse.GetErrorResponse(result);

            return Ok(result.Data);
        }

        [HttpGet("Approved")]
        public async Task<ActionResult<List<ApprovedAuthorizationDto>>> GetApproveduthorizations()
        {
            var result = await _authorizationService.GetApprovedAuthorizations();

            if (!result.Success)
                return WebApiResponse.GetErrorResponse(result);

            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorizationDto>>> GetAuthorizations()
        {
            var result = await _authorizationService.GetAuthorizations();

            if (!result.Success)
                return WebApiResponse.GetErrorResponse(result);

            return Ok(result.Data);
        }

#if DEBUG
        /// <summary>
        /// Only available for debugging purposes.
        /// Done by cronjob at ExternalProcessor
        /// </summary>
        [HttpPatch]
        public async Task<ActionResult<List<AuthorizationDto>>> Patch()
        {
            var result = await _authorizationService.ReverseUnconfirmed();

            if (!result.Success)
                return WebApiResponse.GetErrorResponse(result);

            return Ok(result.Data);
        }
#endif
    }
}
