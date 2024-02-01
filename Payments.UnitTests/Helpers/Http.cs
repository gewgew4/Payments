using Moq;
using Moq.Protected;
using System.Net;

namespace Payments.UnitTests.Helpers
{
    public static class Http
    {
        public static HttpClient GetMockedHttpClient(string externalRequestResponseString, HttpStatusCode httpStatusCode)
        {
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode,
                    Content = new StringContent(externalRequestResponseString)
                });
            var httpClient = new HttpClient(mockMessageHandler.Object);

            return httpClient;
        }
    }
}
