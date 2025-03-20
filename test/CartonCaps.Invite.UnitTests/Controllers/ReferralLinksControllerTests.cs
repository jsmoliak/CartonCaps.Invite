using CartonCaps.Invite.API.Controllers;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace CartonCaps.Invite.UnitTests.Controllers
{
    public class ReferralLinksControllerTests
    {
        private readonly IUserContextService _mockUserContextService;
        private readonly IProfileManagementApiClient _mockProfileManagementApiClient;

        public ReferralLinksControllerTests()
        {
            _mockUserContextService = Substitute.For<IUserContextService>();
            _mockProfileManagementApiClient = Substitute.For<IProfileManagementApiClient>();
        }
        // Tests that the GetLink action returns an OkObjectResult with a referral link when the user profile is found.
        [Fact]
        public async Task GetLink_ValidAuthId_Should_Return_OkObjectResult()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var source = ReferralSourceEnum.Android;
            var userProfile = new UserProfile(authId, "John", "Doe", "123456");

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockProfileManagementApiClient.GetUserProfileAsync(authId).Returns(Task.FromResult(userProfile));

            var referralLinksController = new ReferralLinksController(_mockUserContextService, _mockProfileManagementApiClient);

            // Act
            var result = await referralLinksController.GetLink(source);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okObjectResult.Value);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockProfileManagementApiClient.Received(1).GetUserProfileAsync(authId);
        }

        // Tests that the GetLink action returns an UnauthorizedResult when the user profile is not found.
        [Fact]
        public async Task GetLink_MissingUserProfile_Should_Return_UnauthorizedResult()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var source = ReferralSourceEnum.Android;

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockProfileManagementApiClient.GetUserProfileAsync(authId).ReturnsNull();

            var referralLinksController = new ReferralLinksController(_mockUserContextService, _mockProfileManagementApiClient);

            // Act
            var result = await referralLinksController.GetLink(source);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockProfileManagementApiClient.Received(1).GetUserProfileAsync(authId);
        }
    }
}