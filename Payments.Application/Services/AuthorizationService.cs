using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Payments.Application.Dtos;
using Payments.Application.Models;
using Payments.Application.Options;
using Payments.Application.Result;
using Payments.Application.Services.Interfaces;
using Payments.Domain.Entities;
using Payments.Domain.Enums;
using Payments.Infrastructure.Repositories.Interfaces;
using System.Text;
using System.Text.Json;
using Authorization = Payments.Domain.Entities.Authorization;

namespace Payments.Application.Services
{
    public class AuthorizationService(IOptions<ExternalServiceOptions> options, IHttpClientFactory httpClientFactory, IMapper mapper, IUnitOfWork unitOfWork, ILogger<IAuthorizationService> logger) : IAuthorizationService
    {
        private readonly ExternalServiceOptions _options = options.Value;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<IAuthorizationService> _logger = logger;

        public async Task<Result<AuthorizationDto>> Authorize(AuthorizationRequestDto request)
        {
            _logger.LogInformation("Authorization request: {request}", request);

            Authorization authorization = _mapper.Map<Authorization>(request);
            authorization.CreationDate = DateTime.UtcNow;

            await _unitOfWork.AuthorizationRepository.Add(authorization);
            await _unitOfWork.SaveAsync();

            var response = await SendExternalRequest(authorization);
            authorization.IsAuthorized = response.IsAutorized;

            if (response.IsAutorized)
            {
                authorization.AuthorizationDate = DateTime.UtcNow;
                await CheckApproval(authorization);
            }

            _unitOfWork.AuthorizationRepository.Update(authorization);
            await _unitOfWork.SaveAsync();

            return Result<AuthorizationDto>.Ok(_mapper.Map<AuthorizationDto>(authorization));
        }


        public async Task<Result<AuthorizationDto>> Confirm(ConfirmationRequestDto request)
        {
            _logger.LogInformation("Confirmation request: {request}", request);

            var authorization = await _unitOfWork.AuthorizationRepository.GetById(request.Id);

            #region Validations
            if (authorization == null)
            {
                return Result<AuthorizationDto>.Fail(ResultType.NotFound, [$"The authorization {request.Id} was not found."]);
            }

            if (authorization.ClientType != ClientType.AuthorizationConfirmation)
            {
                return Result<AuthorizationDto>.Fail(ResultType.Invalid, [$"The authorization {request.Id} doesn't need confirmation."]);
            }

            if (authorization.IsConfirmed ?? false)
            {
                return Result<AuthorizationDto>.Fail(ResultType.Invalid, [$"The authorization {request.Id} is already confirmed."]);
            }

            if (!(authorization.IsAuthorized ?? false))
            {
                return Result<AuthorizationDto>.Fail(ResultType.Unexpected, [$"The authorization {request.Id} is not authorized."]);
            }

            if (DateTime.UtcNow > authorization.CreationDate.AddMinutes(5))
            {
                return Result<AuthorizationDto>.Fail(ResultType.Unexpected, [$"The authorization {request.Id} is too old."]);
            }
            #endregion

            authorization.ConfirmationDate = DateTime.UtcNow;
            authorization.IsConfirmed = true;

            await CheckApproval(authorization);

            _unitOfWork.AuthorizationRepository.Update(authorization);
            await _unitOfWork.SaveAsync();

            return Result<AuthorizationDto>.Ok(_mapper.Map<AuthorizationDto>(authorization));
        }

        public async Task<Result<List<ApprovedAuthorizationDto>>> GetApprovedAuthorizations()
        {
            _logger.LogInformation("Getting all approved authorizations");
            var result = await _unitOfWork.ApprovedAuthorizationRepository.GetAll();
            var resultDto = result.Select(x => _mapper.Map<ApprovedAuthorizationDto>(x)).ToList();

            return Result<List<ApprovedAuthorizationDto>>.Ok(resultDto);
        }

        public async Task<Result<List<AuthorizationDto>>> GetAuthorizations()
        {
            _logger.LogInformation("Getting all authorizations");
            var result = await _unitOfWork.AuthorizationRepository.GetAll();
            var resultDto = result.Select(x => _mapper.Map<AuthorizationDto>(x)).ToList();

            return Result<List<AuthorizationDto>>.Ok(resultDto);
        }

        public async Task<Result<List<AuthorizationDto>>> ReverseUnconfirmed()
        {
            _logger.LogInformation("Getting all unconfirmed old authorizations");
            var result = (await _unitOfWork.AuthorizationRepository.GetWhere(
                a => a.IsConfirmed == null &&
                    a.ClientType == ClientType.AuthorizationConfirmation &&
                    DateTime.UtcNow > a.CreationDate.AddMinutes(5)
                )).ToList();

            result.ForEach(x => x.IsConfirmed = false);

            await MakeReversal(result);
            await _unitOfWork.SaveAsync();

            var resultDto = result.Select(x => _mapper.Map<AuthorizationDto>(x)).ToList();

            return Result<List<AuthorizationDto>>.Ok(resultDto);
        }

        /// <summary>
        /// Used to call external API
        /// </summary>
        /// <param name="body">Info on authorization to be posted</param>
        /// <returns>Reponse given by the external API</returns>
        private async Task<ExternalResponse> SendExternalRequest(Authorization body)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(_mapper.Map<ExternalRequest>(body));
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var externalResult = await httpClient.PostAsync(_options.ApiUrl, content);

            if (!externalResult.IsSuccessStatusCode)
            {
                _logger.LogError("Post was unsuccesfull. {info}", new { HttpCode = externalResult.StatusCode, externalResult.Content });
            }

            return JsonSerializer.Deserialize<ExternalResponse>(await externalResult.Content.ReadAsStringAsync(), _options.JsonSerializerOptions)!;
        }

        /// <summary>
        /// After a successful authorization, checks and
        /// may accordingly save the approved authorization
        /// </summary>
        /// <param name="authorization">Confirmed authorization to be checked</param>
        private async Task CheckApproval(Authorization authorization)
        {
            if (authorization.ClientType == ClientType.Authorization || (authorization.IsConfirmed ?? false))
            {
                var approved = _mapper.Map<ApprovedAuthorization>(authorization);
                await _unitOfWork.ApprovedAuthorizationRepository.Add(approved);
            }
        }

        /// <summary>
        /// Makes almost identical reversal authorizations from an
        /// already existing list of authorizations
        /// </summary>
        /// <param name="list">Authorizations to be processed</param>
        private async Task MakeReversal(List<Authorization> list)
        {
            foreach (var authorization in list)
            {
                authorization.IsConfirmed = false;

                var reversal = new Authorization
                {
                    AuthorizationDate = authorization.AuthorizationDate,
                    AuthorizationType = AuthorizationType.Reversal,
                    ClientId = authorization.ClientId,
                    ClientType = authorization.ClientType,
                    CreationDate = DateTime.UtcNow,
                    IsAuthorized = authorization.IsAuthorized,
                    IsConfirmed = true,
                    Observations = $"{authorization.Id} reversal",
                    Total = authorization.Total
                };
                _unitOfWork.AuthorizationRepository.Update(authorization);
                await _unitOfWork.AuthorizationRepository.Add(reversal);
                // TODO: determine if it's send to ApprovedAuthorizations
            }
        }
    }
}
