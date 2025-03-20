namespace CartonCaps.Invite.UnitTests.Filters
{
    using CartonCaps.Invite.API.Filters;
    using CartonCaps.Invite.Model.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using System;
    using Xunit;

    public class ResourceNotFoundExceptionFilterTests
    {
        private readonly ILogger<ResourceNotFoundExceptionFilter> _mockLogger;

        public ResourceNotFoundExceptionFilterTests()
        {
            _mockLogger = Substitute.For<ILogger<ResourceNotFoundExceptionFilter>>();
        }

        // Tests that the OnException method handles ResourceNotFoundException by setting the result to a NotFoundObjectResult.
        [Fact]
        public void OnException_ResourceNotFoundException_Should_Set_Result()
        {
            // Arrange
            var filter = new ResourceNotFoundExceptionFilter(_mockLogger);
            var exception = new ResourceNotFoundException("Test argument exception message");
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
            Assert.IsType<NotFoundResult>(context.Result);
            Assert.True(context.ExceptionHandled);

            var contentResult = (NotFoundResult)context.Result;
            Assert.Equal(404, contentResult.StatusCode);
            _mockLogger.ReceivedWithAnyArgs(1).LogWarning(exception, "test");
        }

        // Tests that the OnException method does not handle non-ResourceNotFoundException exceptions.
        [Fact]
        public void OnException_NonResourceNotFoundException_Should_Not_Set_Result()
        {
            // Arrange
            var filter = new ResourceNotFoundExceptionFilter(_mockLogger);
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