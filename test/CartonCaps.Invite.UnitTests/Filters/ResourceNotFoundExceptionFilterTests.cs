namespace CartonCaps.Invite.UnitTests.Filters
{
    using CartonCaps.Invite.API.Filters;
    using CartonCaps.Invite.Model.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using NSubstitute;
    using System;
    using Xunit;

    public class ResourceNotFoundExceptionFilterTests
    {
        // Tests that the OnException method handles ResourceNotFoundException by setting the result to a NotFoundObjectResult.
        [Fact]
        public void OnException_ResourceNotFoundException_Should_Set_Result()
        {
            // Arrange
            var filter = new ResourceNotFoundExceptionFilter();
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
            Assert.IsType<NotFoundObjectResult>(context.Result);
            Assert.True(context.ExceptionHandled);

            var contentResult = (NotFoundObjectResult)context.Result;
            Assert.Equal(404, contentResult.StatusCode);
        }

        // Tests that the OnException method does not handle non-ResourceNotFoundException exceptions.
        [Fact]
        public void OnException_NonResourceNotFoundException_Should_Not_Set_Result()
        {
            // Arrange
            var filter = new ResourceNotFoundExceptionFilter();
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