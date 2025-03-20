namespace CartonCaps.Invite.UnitTests.Filters
{
    using CartonCaps.Invite.API.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using NSubstitute;
    using System;
    using Xunit;

    public class ArgumentExceptionFilterTests
    {
        // Tests that the OnException method handles ArgumentException by setting the result to a BadRequest ContentResult.
        [Fact]
        public void OnException_ArgumentException_Should_Set_Result()
        {
            // Arrange
            var filter = new ArgumentExceptionFilter();
            var exception = new ArgumentException("Test argument exception message");
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
            Assert.IsType<ContentResult>(context.Result);
            Assert.True(context.ExceptionHandled);

            var contentResult = (ContentResult)context.Result;
            Assert.Equal(400, contentResult.StatusCode);
            Assert.Equal("application/json", contentResult.ContentType);
            Assert.NotNull(contentResult.Content);
        }

        // Tests that the OnException method does not handle non-ArgumentException exceptions.
        [Fact]
        public void OnException_NonArgumentException_Should_Not_HandleException()
        {
            // Arrange
            var filter = new ArgumentExceptionFilter();
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
        }
    }
}