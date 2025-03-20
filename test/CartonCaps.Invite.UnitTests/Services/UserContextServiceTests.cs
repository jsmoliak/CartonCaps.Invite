using CartonCaps.Invite.API.Services;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System.Security.Claims;

namespace CartonCaps.Invite.UnitTests.Services
{
    public class UserContextServiceTests
    {
        private readonly IHttpContextAccessor _mockHttpContextAccessor;

        public UserContextServiceTests()
        {
            _mockHttpContextAccessor = Substitute.For<IHttpContextAccessor>();
        }

        // Tests that GetAuthId returns the correct authId when a valid claim is present.
        [Fact]
        public void GetAuthId_Valid_Should_Return_UserId()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var httpContext = Substitute.For<HttpContext>();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("sub", authId) }));

            httpContext.User.Returns(claimsPrincipal);
            _mockHttpContextAccessor.HttpContext.Returns(httpContext);

            var userContextService = new UserContextService(_mockHttpContextAccessor);

            // Act
            var result = userContextService.GetAuthId();

            // Assert
            Assert.Equal(authId, result);
        }

        // Tests that GetAuthId returns an empty string when the claim is missing.
        [Fact]
        public void GetAuthId_Missing_Should_Return_EmptyString()
        {
            // Arrange
            var httpContext = Substitute.For<HttpContext>();
            var claimsPrincipal = new ClaimsPrincipal();

            httpContext.User.Returns(claimsPrincipal);
            _mockHttpContextAccessor.HttpContext.Returns(httpContext);

            var userContextService = new UserContextService(_mockHttpContextAccessor);

            // Act
            var authId = userContextService.GetAuthId();

            // Assert
            Assert.Equal(string.Empty, authId);
        }
    }
}