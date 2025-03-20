using System.Net;

namespace CartonCaps.Invite.API.Infrastructure

{
    /// <summary>
    /// A fake implementation of IHttpClientFactory for testing purposes.
    /// </summary>
    public class FakeHttpClientFactory : IHttpClientFactory
    {
        /// <summary>
        /// Creates a new HttpClient instance with a FakeHttpMessageHandler.
        /// </summary>
        /// <param name="name">The logical name of the client to create.</param>
        /// <returns>A new HttpClient instance.</returns>
        public HttpClient CreateClient(string name)
        {
            var httpMessageHandler = new FakeHttpMessageHandler();
            return new HttpClient(httpMessageHandler);
        }
    }

    /// <summary>
    /// A fake HttpMessageHandler for testing purposes.
    /// </summary>
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"authId\":\"12345\", \"firstName\":\"John\", \"lastName\":\"Doe\", \"referralCode\":\"123456\"}")
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpMessageHandler"/> class.
        /// </summary>

        public FakeHttpMessageHandler() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeHttpMessageHandler"/> class with a custom fake response.
        /// </summary>
        /// <param name="fakeResponse">The fake HTTP response message.</param>

        public FakeHttpMessageHandler(HttpResponseMessage fakeResponse)
        {
            _fakeResponse = fakeResponse;
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The HTTP response message.</returns>

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(_fakeResponse);
        }
    }
}
