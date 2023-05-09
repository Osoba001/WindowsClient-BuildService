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
        [Fact]
        public async void DownloadRelease_WhenCalled_InvokesHttpGetRequest()
        {
            var mockHttpHandler = MockHttpMessageHandler.SetupGetResource(It.IsAny<string>());

            var httpClient=new HttpClient(mockHttpHandler.Object);

            var sut=new PullRequestService(httpClient);

            await sut.DownloadRelease(It.IsAny<string>());

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
