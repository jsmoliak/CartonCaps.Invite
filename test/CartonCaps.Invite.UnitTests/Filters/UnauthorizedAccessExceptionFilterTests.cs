namespace CartonCaps.Invite.UnitTests.Filters
{
    using CartonCaps.Invite.API.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using System;
    using Xunit;

    public class UnauthorizedAccessExceptionFilterTests
    {
        private readonly ILogger<UnauthorizedAccessExceptionFilter> _mockLogger;

        public UnauthorizedAccessExceptionFilterTests()
        {
            _mockLogger = Substitute.For<ILogger<UnauthorizedAccessExceptionFilter>>();

        }

        // Tests that the OnException method handles UnauthorizedAccessException by setting the result to an UnauthorizedObjectResult.
        [Fact]
        public void OnException_UnauthorizedAccessException_Should_Set_Result()
        {
            // Arrange
            var filter = new UnauthorizedAccessExceptionFilter(_mockLogger);
            var exception = new UnauthorizedAccessException("Test argument exception message");
            var httpContext = Substitute.For<HttpContext>();
            var routeData = new RouteData();
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
            var context = Substitute.For<ExceptionContext>(actionContext, new List<IFilterMetadata>());
            context.Exception = exception;
            context.HttpContext = Substitute.For<HttpContext>();
            context.HttpContext.TraceIdentifier = "test-trace-id";

            // Act
            filter.OnException(context);

            // Assert
            Assert.NotNull(context.Result);
            Assert.IsType<UnauthorizedResult>(context.Result);
            Assert.True(context.ExceptionHandled);

            var contentResult = (UnauthorizedResult)context.Result;
            Assert.Equal(401, contentResult.StatusCode);
            _mockLogger.ReceivedWithAnyArgs(1).LogWarning(exception, "test");
        }

        // Tests that the OnException method does not handle non-UnauthorizedAccessException exceptions.
        [Fact]
        public void OnException_NonUnauthorizedAccessException_Should_Not_Set_Result()
        {
            // Arrange
            var filter = new UnauthorizedAccessExceptionFilter(_mockLogger);
            var exception = new Exception("Test exception message");
            var httpContext = Substitute.For<HttpContext>();
            var routeData = new RouteData();
            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
            var context = Substitute.For<ExceptionContext>(actionContext, new List<IFilterMetadata>());
            context.Exception = exception;

            // Act
            filter.OnException(context);

            // Assert
            Assert.False(context.ExceptionHandled);
            _mockLogger.ReceivedWithAnyArgs(0).LogWarning(exception, "test");
        }
    }
}