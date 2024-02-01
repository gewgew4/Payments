using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Payments.Application.Dtos;
using Payments.Application.Mappers;
using Payments.Application.Options;
using Payments.Application.Result;
using Payments.Application.Services;
using Payments.Application.Services.Interfaces;
using Payments.Domain.Entities;
using Payments.Infrastructure.Repositories.Interfaces;
using Payments.UnitTests.Helpers;
using System.Linq.Expressions;
using System.Text.Json;

namespace Payments.UnitTests.Application.Services
{
    public class AuthorizationServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<AuthorizationService>> _logger;
        private readonly IMapper _mapper;
        private readonly IOptions<ExternalServiceOptions> _externalServiceOptions;
        private readonly Mock<IHttpClientFactory> _mockedHttpClientFactory;
        private readonly IAuthorizationService _service;
        private readonly MapperConfiguration mapperConfiguration = new(cfg => cfg.AddProfiles(new List<Profile>() { new ApprovedAuthorizationMapper(), new AuthorizationMapper() }));

        public AuthorizationServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<AuthorizationService>>();
            _mapper = new Mapper(mapperConfiguration);
            _externalServiceOptions = Options.Create(new ExternalServiceOptions() { ApiUrl = "https://url.com/", JsonSerializerOptions = new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true } });
            _mockedHttpClientFactory = new Mock<IHttpClientFactory>();
            _service = new AuthorizationService(_externalServiceOptions, _mockedHttpClientFactory.Object, _mapper, _unitOfWork.Object, _logger.Object);

            // Setups
            _unitOfWork.Setup(x => x.AuthorizationRepository.Add(It.IsAny<Domain.Entities.Authorization>()));
            _unitOfWork.Setup(x => x.ApprovedAuthorizationRepository.Add(It.IsAny<Domain.Entities.ApprovedAuthorization>()));
            _unitOfWork.Setup(x => x.AuthorizationRepository.Update(It.IsAny<Domain.Entities.Authorization>()));
            _unitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(1);
        }

        [Fact]
        public async Task Authorize_ReturnsOk_WhenAuthorized()
        {
            // Arrange
            var request = Factory.GetAuthorizationRequestDto();
            var externalRequestResponseString = JsonSerializer.Serialize(Factory.GetExternalResponse());
            var httpClient = Http.GetMockedHttpClient(externalRequestResponseString, System.Net.HttpStatusCode.OK);

            _mockedHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _service.Authorize(request);

            // Assert
            _unitOfWork.Verify(m => m.AuthorizationRepository.Add(It.IsAny<Domain.Entities.Authorization>()), Times.Once);
            _unitOfWork.Verify(m => m.ApprovedAuthorizationRepository.Add(It.IsAny<Domain.Entities.ApprovedAuthorization>()), Times.Once);
            _unitOfWork.Verify(m => m.AuthorizationRepository.Update(It.IsAny<Domain.Entities.Authorization>()), Times.Once);
            _unitOfWork.Verify(m => m.SaveAsync(), Times.Exactly(2));

            Assert.NotNull(result.Data);
            Assert.Null(result.Errors);
            Assert.True(result.Data?.IsAuthorized);
            Assert.IsType<Result<AuthorizationDto>>(result);
        }

        [Fact]
        public async Task Authorize_ReturnsOk_WhenNotAuthorized()
        {
            // Arrange
            var request = Factory.GetAuthorizationRequestDto();
            var externalRequestResponseString = JsonSerializer.Serialize(Factory.GetExternalResponse(false));
            var httpClient = Http.GetMockedHttpClient(externalRequestResponseString, System.Net.HttpStatusCode.OK);

            _mockedHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _service.Authorize(request);

            // Assert
            _unitOfWork.Verify(m => m.AuthorizationRepository.Add(It.IsAny<Domain.Entities.Authorization>()), Times.Once);
            _unitOfWork.Verify(m => m.ApprovedAuthorizationRepository.Add(It.IsAny<Domain.Entities.ApprovedAuthorization>()), Times.Never);
            _unitOfWork.Verify(m => m.AuthorizationRepository.Update(It.IsAny<Domain.Entities.Authorization>()), Times.Once);
            _unitOfWork.Verify(m => m.SaveAsync(), Times.Exactly(2));

            Assert.NotNull(result.Data);
            Assert.Null(result.Errors);
            Assert.False(result.Data?.IsAuthorized);
            Assert.IsType<Result<AuthorizationDto>>(result);
        }

        [Fact]
        public async Task Confirm_ReturnsOk()
        {
            // Arrange
            var request = Factory.GetConfirmationRequestDto();
            var externalRequestResponseString = JsonSerializer.Serialize(Factory.GetExternalResponse(false));

            _unitOfWork.Setup(x => x.AuthorizationRepository.GetById(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<string[]>()))
                .ReturnsAsync(Factory.GetConfirmableAuthorization(request.Id));

            // Act
            var result = await _service.Confirm(request);

            // Assert
            _unitOfWork.Verify(m => m.AuthorizationRepository.Add(It.IsAny<Domain.Entities.Authorization>()), Times.Never);
            _unitOfWork.Verify(m => m.ApprovedAuthorizationRepository.Add(It.IsAny<Domain.Entities.ApprovedAuthorization>()), Times.Once);
            _unitOfWork.Verify(m => m.AuthorizationRepository.Update(It.IsAny<Domain.Entities.Authorization>()), Times.Once);
            _unitOfWork.Verify(m => m.SaveAsync(), Times.Once);

            Assert.NotNull(result.Data);
            Assert.Null(result.Errors);
            Assert.True(result.Data?.IsAuthorized);
            Assert.True(result.Data?.IsConfirmed);
            Assert.IsType<Result<AuthorizationDto>>(result);
        }

        [Fact]
        public async Task Confirm_ReturnsBadReq()
        {
            // Arrange
            var request = Factory.GetConfirmationRequestDto();
            var externalRequestResponseString = JsonSerializer.Serialize(Factory.GetExternalResponse(false));

            _unitOfWork.Setup(x => x.AuthorizationRepository.GetById(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<string[]>()))
                .ReturnsAsync(Factory.GetUnconfirmableAuthorization(request.Id));

            // Act
            var result = await _service.Confirm(request);

            // Assert
            _unitOfWork.Verify(m => m.AuthorizationRepository.Add(It.IsAny<Domain.Entities.Authorization>()), Times.Never);
            _unitOfWork.Verify(m => m.ApprovedAuthorizationRepository.Add(It.IsAny<Domain.Entities.ApprovedAuthorization>()), Times.Never);
            _unitOfWork.Verify(m => m.AuthorizationRepository.Update(It.IsAny<Domain.Entities.Authorization>()), Times.Never);
            _unitOfWork.Verify(m => m.SaveAsync(), Times.Never);

            Assert.Null(result.Data);
            Assert.NotNull(result.Errors);
            Assert.False(result.Success);
            Assert.IsType<Result<AuthorizationDto>>(result);
        }

        [Fact]
        public async Task ReverseUnconfirmed_ReturnsOk()
        {
            // Arrange
            var quantity = 5;
            List<Authorization> unconfAuthorizations = Factory.GetUnconfirmableAuthorizations(quantity);
            var externalRequestResponseString = JsonSerializer.Serialize(Factory.GetExternalResponse(false));

            _unitOfWork.Setup(x => x.AuthorizationRepository.GetWhere(It.IsAny<Expression<Func<Authorization, bool>>>(), null, null, null, It.IsAny<string[]>()))
                .ReturnsAsync(unconfAuthorizations);

            // Act
            var result = await _service.ReverseUnconfirmed();

            // Assert
            _unitOfWork.Verify(m => m.AuthorizationRepository.Add(It.IsAny<Authorization>()), Times.Exactly(quantity));
            _unitOfWork.Verify(m => m.AuthorizationRepository.Update(It.IsAny<Authorization>()), Times.Exactly(quantity));
            _unitOfWork.Verify(m => m.ApprovedAuthorizationRepository.Add(It.IsAny<ApprovedAuthorization>()), Times.Never);
            _unitOfWork.Verify(m => m.SaveAsync(), Times.Once);

            Assert.NotNull(result.Data);
            Assert.Null(result.Errors);
            Assert.True(result.Data?.Count == quantity);
            Assert.IsType<Result<List<AuthorizationDto>>>(result);
        }
    }
}
