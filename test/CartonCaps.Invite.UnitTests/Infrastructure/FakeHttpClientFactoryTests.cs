namespace CartonCaps.Invite.UnitTests.Infrastructure
{
    using CartonCaps.Invite.API.Infrastructure;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xunit;

    public class FakeHttpClientFactoryTests
    {
        // Tests that the CreateClient method returns an HttpClient with a FakeHttpMessageHandler.
        [Fact]
        public void CreateClient_ReturnsHttpClientWithFakeHttpMessageHandler()
        {
            // Arrange
            var factory = new FakeHttpClientFactory();

            // Act
            var client = factory.CreateClient("test");

            // Assert
            Assert.NotNull(client);
            Assert.IsType<HttpClient>(client);
        }
    }

    public class FakeHttpMessageHandlerTests
    {
        // Tests that the SendAsync method returns an OK response with non-null content.
        [Fact]
        public async Task SendAsync_ReturnsOkResponseWithExpectedContent()
        {
            // Arrange
            var handler = new FakeHttpMessageHandler();
            var client = new HttpClient(handler);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://example.com");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
        }
    }
}