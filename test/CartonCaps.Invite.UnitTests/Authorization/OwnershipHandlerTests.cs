using CartonCaps.Invite.API.Authorization;
using CartonCaps.Invite.Model.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using System.Security.Claims;

namespace CartonCaps.Invite.UnitTests.Authorization
{
    public class OwnershipHandlerTests
    {
        // Tests that HandleRequirementAsync succeeds when the JWT contains a valid AuthId matching the resource owner.
        [Fact]
        public async Task HandleRequirementAsync_ValidJWT_Should_Return_Succeeded()
        {
            // Arrange
            var mockResource = Substitute.For<IOwnable>();
            var requirement = new OwnershipRequirement();
            var authId = Guid.NewGuid().ToString();
            var claims = new[] { new Claim(ClaimTypes.Name, authId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var handler = new OwnershipHandler();
            var httpContext = new DefaultHttpContext { User = principal };
            var routeData = new RouteData();
            routeData.Values.Add("resource", mockResource);
            httpContext.Items[typeof(RouteData)] = routeData;

            var context = new AuthorizationHandlerContext(new List<OwnershipRequirement>([requirement]), httpContext.User, mockResource);

            mockResource.GetOwnerId().Returns(authId);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.True(context.HasSucceeded);
        }

        // Tests that HandleRequirementAsync fails when the JWT is invalid or missing.
        [Fact]
        public async Task HandleRequirementAsync_InvalidJWT_Should_Return_Failed()
        {
            // Arrange
            var mockResource = Substitute.For<IOwnable>();
            var requirement = new OwnershipRequirement();
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            var handler = new OwnershipHandler();
            var httpContext = new DefaultHttpContext { User = principal };
            var routeData = new RouteData();
            routeData.Values.Add("resource", mockResource);
            httpContext.Items[typeof(RouteData)] = routeData;

            var context = new AuthorizationHandlerContext(new List<OwnershipRequirement>([requirement]), httpContext.User, mockResource);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.False(context.HasSucceeded);
        }

        // Tests that HandleRequirementAsync fails when the JWT's AuthId does not match the resource owner's AuthId.
        [Fact]
        public async Task HandleRequirementAsync_ValidJWTMismatchedAuthId_Should_Return_Failed()
        {
            // Arrange
            var mockResource = Substitute.For<IOwnable>();
            var requirement = new OwnershipRequirement();
            var authId1 = Guid.NewGuid().ToString();
            var authId2 = Guid.NewGuid().ToString();
            var claims = new[] { new Claim(ClaimTypes.Name, authId1) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var handler = new OwnershipHandler();
            var httpContext = new DefaultHttpContext { User = principal };
            var routeData = new RouteData();
            routeData.Values.Add("resource", mockResource);
            httpContext.Items[typeof(RouteData)] = routeData;

            var context = new AuthorizationHandlerContext(new List<OwnershipRequirement>([requirement]), httpContext.User, mockResource);

            mockResource.GetOwnerId().Returns(authId2);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.False(context.HasSucceeded);
        }
    }
}