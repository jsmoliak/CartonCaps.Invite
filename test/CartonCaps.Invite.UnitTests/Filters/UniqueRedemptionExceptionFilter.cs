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

    public class UniqueRedemptionExceptionFilterTests
    {
        // Tests that the OnException method handles UniqueRedemptionConstraintException by setting the result to a ConflictObjectResult.
        [Fact]
        public void OnException_UniqueRedemptionException_Should_Set_Result()
        {
            // Arrange
            var filter = new UniqueRedemptionExceptionFilter();
            var exception = new UniqueRedemptionConstraintException("Test argument exception message");
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
            Assert.IsType<ConflictObjectResult>(context.Result);
            Assert.True(context.ExceptionHandled);

            var contentResult = (ConflictObjectResult)context.Result;
            Assert.Equal(409, contentResult.StatusCode);
        }

        // Tests that the OnException method does not handle non-UniqueRedemptionConstraintException exceptions.
        [Fact]
        public void OnException_NonUniqueRedemptionException_Should_Not_Set_Result()
        {
            // Arrange
            var filter = new UniqueRedemptionExceptionFilter();
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