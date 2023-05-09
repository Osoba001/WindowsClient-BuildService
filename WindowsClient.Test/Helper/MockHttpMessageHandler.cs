using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WindowsClientBuildSelfService.Share.Constants;

namespace WindowsClient.Test.Helper
{
    internal static class MockHttpMessageHandler
    {
        internal static Mock<HttpMessageHandler> SetupGetResource<T>(T expectedResponse,HttpStatusCode statusCode =HttpStatusCode.OK)
        {
            var mockHttpResponse = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
            };

            mockHttpResponse.Content.Headers.ContentType= new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var handlerMock= new Mock<HttpMessageHandler>();

            var httpRequestMsg = new HttpRequestMessage()
            {
                
                RequestUri = new Uri("https://jsonplaceholder.typicode.com/users"),
                Method = HttpMethod.Get,
            };
            

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    httpRequestMsg,
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockHttpResponse);

            return handlerMock;
        }
    }
}
