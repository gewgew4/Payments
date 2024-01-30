using Payments.Application.Dtos;
using Payments.Application.Result;

namespace Payments.Application.Services.Interfaces
{
    public interface IAuthorizationService
    {
        /// <summary>
        /// Adds and processes an authorization to the repository
        /// </summary>
        /// <param name="request">Info on authorization to process</param>
        /// <returns>DTO with relevant data</returns>
        Task<Result<AuthorizationDto>> Authorize(AuthorizationRequestDto request);
        /// <summary>
        /// Confirms a relevant and existing authorization
        /// </summary>
        /// <param name="request">Info on authorization to confirm</param>
        /// <returns>DTO with relevant data</returns>
        Task<Result<AuthorizationDto>> Confirm(ConfirmationRequestDto request);
        /// <summary>
        /// Gets all authorizations
        /// </summary>
        /// <returns>List of DTO's authorizations</returns>
        Task<Result<List<AuthorizationDto>>> GetAuthorizations();
        /// <summary>
        /// Gets all approved authorizations
        /// </summary>
        /// <returns>List of DTO's approved authorizations</returns>
        Task<Result<List<ApprovedAuthorizationDto>>> GetApprovedAuthorizations();
        /// <summary>
        /// Looks for unconfirmed expired authorizations and reverses them
        /// </summary>
        /// <returns>List of DTO's reversed authorizations</returns>
        Task<Result<List<AuthorizationDto>>> ReverseUnconfirmed();
    }
}
