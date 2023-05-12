using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsClient.Test.Helper;
using WindowsClientBuildSelfService.PR.Services;

namespace WindowsClient.Test.Systems.Services
{
    public class PullRequestServiceTest
    {
        private readonly Uri url = new Uri("https://jsonplaceholder.typicode.com/");
        
        [Fact]
        public async void DownloadRelease_WhenCalled_InvokesHttpGetRequest()
        {
            // Arrange
            var mockHttpHandler = SetupGetResource(It.IsAny<string>());
            var httpClient = new HttpClient(mockHttpHandler.Object)
            {
                BaseAddress = url
            };
            var sut = new PullRequestService(httpClient);

            // Act
            await sut.DownloadRelease(It.IsAny<string>());
            // Assert
            mockHttpHandler
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == url),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Fact]
        public async void DownloadRelease_WhenNoInternet_ReturnNoInternetMessage()
        {
            // Arrange

            var httpClient = SetupHttpResponseForNetworkFailure();
            httpClient.BaseAddress = url;

            var sut = new PullRequestService(httpClient);

            // Act
           var result= await sut.DownloadRelease(It.IsAny<string>());
            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ReasonPhrase.Should().Be(NoInternetErrorMessage);
        }

        [Fact]
        public async void GetPullRequests_WhenCalled_InvokesHttpGetRequest()
        {
            // Arrange
            var mockHttpHandler = SetupGetResource(It.IsAny<string>());
            var httpClient = new HttpClient(mockHttpHandler.Object)
            {
                BaseAddress = url
            };
            var sut = new PullRequestService(httpClient);

            // Act
            await sut.GetPullRequests(It.IsAny<string>());

            // Assert
            mockHttpHandler
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}
