using CartonCaps.Invite.API.Infrastructure;
using CartonCaps.Invite.Model.ValueObjects;
using NSubstitute;
using System.Net;

namespace CartonCaps.Invite.UnitTests.Infrastructure
{
    public class ProfileManagementApiClientTests
    {
        private readonly IHttpClientFactory _mockHttpClientFactory;

        public ProfileManagementApiClientTests()
        {
            _mockHttpClientFactory = Substitute.For<IHttpClientFactory>();
        }

        // Tests that GetUserProfileAsync returns a UserProfile object when the HTTP response is successful.
        [Fact]
        public async Task GetUserProfileAsync_Valid_Should_Return_UserProfile()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"authId\":\"12345\", \"firstName\":\"John\", \"lastName\":\"Doe\", \"referralCode\":\"123456\"}")
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(response));

            _mockHttpClientFactory.CreateClient().Returns(httpClient);

            var profileManagementApiClient = new ProfileManagementApiClient(_mockHttpClientFactory);

            // Act
            var result = await profileManagementApiClient.GetUserProfileAsync(userId);

            // Assert
            Assert.IsType<UserProfile>(result);
        }

        // Tests that GetUserProfileAsync throws a ProfileManagementException when the HTTP response is not successful.
        [Fact]
        public async Task GetUserProfileAsync_Valid_Should_Throw_ProfileManagementException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(new FakeHttpMessageHandler(response));

            _mockHttpClientFactory.CreateClient().Returns(httpClient);

            var profileManagementApiClient = new ProfileManagementApiClient(_mockHttpClientFactory);

            // Act, Assert
            await Assert.ThrowsAsync<ProfileManagementException>(async () => await profileManagementApiClient.GetUserProfileAsync(userId));
        }
    }
}