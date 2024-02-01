using Microsoft.AspNetCore.Mvc;
using Moq;
using Payments.Application.Dtos;
using Payments.Application.Result;
using Payments.Application.Services.Interfaces;
using Payments.Presentacion.Controllers;
using Payments.UnitTests.Helpers;

namespace Payments.UnitTests.Presentation.Controllers
{
    public class AuthorizationControllerTests
    {
        private readonly Mock<IAuthorizationService> _authorizationService;
        private readonly AuthorizationController _classUnderTest;

        public AuthorizationControllerTests()
        {
            _authorizationService = new Mock<IAuthorizationService>();
            _classUnderTest = new AuthorizationController(_authorizationService.Object);
        }

        [Fact]
        public async Task Authorize_ReturnsBadReq()
        {
            // Arrange
            _authorizationService
                .Setup(x => x.Authorize(It.IsAny<AuthorizationRequestDto>()))
                .ReturnsAsync(Result<AuthorizationDto>.Fail(ResultType.Invalid, ["Unexpected error"]));

            // Act
            var actionresult = await _classUnderTest.Authorize(null!);
            var result = actionresult.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actionresult);
            Assert.IsType<BadRequestObjectResult>(actionresult.Result);
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async Task Authorize_ReturnsOk()
        {
            // Arrange
            _authorizationService
                .Setup(x => x.Authorize(It.IsAny<AuthorizationRequestDto>()))
                .ReturnsAsync(Result<AuthorizationDto>.Ok(Factory.GetAuthorizationDto()));

            // Act
            var actionresult = await _classUnderTest.Authorize(Factory.GetAuthorizationRequestDto());
            var result = actionresult.Result as OkObjectResult;

            // Assert
            Assert.NotNull(actionresult);
            Assert.IsType<ActionResult<AuthorizationDto>>(actionresult);
            Assert.IsType<AuthorizationDto>(result?.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Confirm_ReturnsBadReq()
        {
            // Arrange
            _authorizationService
                .Setup(x => x.Confirm(It.IsAny<ConfirmationRequestDto>()))
                .ReturnsAsync(Result<AuthorizationDto>.Fail(ResultType.Invalid, ["Unexpected error"]));

            // Act
            var actionresult = await _classUnderTest.Confirm(null!);
            var result = actionresult.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actionresult);
            Assert.IsType<BadRequestObjectResult>(actionresult.Result);
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async Task Confirm_ReturnsOk()
        {
            // Arrange
            _authorizationService
                .Setup(x => x.Confirm(It.IsAny<ConfirmationRequestDto>()))
                .ReturnsAsync(Result<AuthorizationDto>.Ok(Factory.GetAuthorizationDto()));

            // Act
            var actionresult = await _classUnderTest.Confirm(Factory.GetConfirmationRequestDto());
            var result = actionresult.Result as OkObjectResult;

            // Assert
            Assert.NotNull(actionresult);
            Assert.IsType<ActionResult<AuthorizationDto>>(actionresult);
            Assert.IsType<AuthorizationDto>(result?.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetApprovedAuthorizations_ReturnsBadReq()
        {
            // Arrange
            _authorizationService
                .Setup(x => x.GetApprovedAuthorizations())
                .ReturnsAsync(Result<List<ApprovedAuthorizationDto>>.Fail(ResultType.Invalid, ["Unexpected error"]));

            // Act
            var actionresult = await _classUnderTest.GetApproveduthorizations();
            var result = actionresult.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actionresult);
            Assert.IsType<BadRequestObjectResult>(actionresult.Result);
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async Task GetApprovedAuthorizations_ReturnsOk()
        {
            // Arrange
            _authorizationService
                .Setup(x => x.GetApprovedAuthorizations())
                .ReturnsAsync(Result<List<ApprovedAuthorizationDto>>.Ok(Factory.GetApprovedAuthorizationDtos(5)));

            // Act
            var actionresult = await _classUnderTest.GetApproveduthorizations();
            var result = actionresult.Result as OkObjectResult;

            // Assert
            Assert.NotNull(actionresult);
            Assert.IsType<ActionResult<List<ApprovedAuthorizationDto>>>(actionresult);
            Assert.IsType<List<ApprovedAuthorizationDto>>(result?.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAuthorizations_ReturnsBadReq()
        {
            // Arrange
            _authorizationService
                .Setup(x => x.GetAuthorizations())
                .ReturnsAsync(Result<List<AuthorizationDto>>.Fail(ResultType.Invalid, ["Unexpected error"]));

            // Act
            var actionresult = await _classUnderTest.GetAuthorizations();
            var result = actionresult.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(actionresult);
            Assert.IsType<BadRequestObjectResult>(actionresult.Result);
            Assert.Equal(400, result?.StatusCode);
        }

        [Fact]
        public async Task GetAuthorizations_ReturnsOk()
        {
            // Arrange
            _authorizationService
                .Setup(x => x.GetAuthorizations())
                .ReturnsAsync(Result<List<AuthorizationDto>>.Ok(Factory.GetAuthorizationDtos(5)));

            // Act
            var actionresult = await _classUnderTest.GetAuthorizations();
            var result = actionresult.Result as OkObjectResult;

            // Assert
            Assert.NotNull(actionresult);
            Assert.IsType<ActionResult<List<AuthorizationDto>>>(actionresult);
            Assert.IsType<List<AuthorizationDto>>(result?.Value);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
