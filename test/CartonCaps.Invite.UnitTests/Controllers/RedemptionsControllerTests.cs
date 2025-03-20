using CartonCaps.Invite.API.Controllers;
using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;

namespace CartonCaps.Invite.UnitTests.Controllers
{
    public class RedemptionsControllerTests
    {
        private readonly IAuthorizationService _mockAuthorizationService;
        private readonly IRedemptionsService _mockRedemptionsService;

        public RedemptionsControllerTests()
        {
            _mockAuthorizationService = Substitute.For<IAuthorizationService>();
            _mockRedemptionsService = Substitute.For<IRedemptionsService>();
        }

        // Tests that the Get action returns an OkResult with the correct RedemptionResponse when authorization succeeds.
        [Fact]
        public async Task Get_ValidAuthId_Should_Return_OkResult()
        {
            // Arrange
            var redemptionId = Guid.NewGuid();
            var referralCode = new ReferralCode("123456", Guid.NewGuid());
            var referralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());
            var redemption = new Redemption(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            redemption.ReferralCode = referralCode;
            redemption.ReferralSource = referralSource;

            _mockRedemptionsService.GetRedemptionAsync(redemptionId, CancellationToken.None).Returns(Task.FromResult(redemption));
            _mockAuthorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), redemption, "OwnerOnly").Returns(Task.FromResult(AuthorizationResult.Success()));

            var redemptionsController = new RedemptionsController(_mockAuthorizationService, _mockRedemptionsService);

            // Act
            var result = await redemptionsController.Get(redemptionId, CancellationToken.None);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equivalent(okObjectResult.Value, RedemptionResponse.FromModel(redemption));
            await _mockRedemptionsService.Received(1).GetRedemptionAsync(redemptionId, CancellationToken.None);
            await _mockAuthorizationService.Received(1).AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), redemption, "OwnerOnly");
        }

        // Tests that the Get action returns a NotFoundResult when authorization fails.
        [Fact]
        public async Task Get_ForbiddenAuthId_Should_Return_NotFoundResult()
        {
            // Arrange
            var redemptionId = Guid.NewGuid();
            var referralCode = new ReferralCode("123456", Guid.NewGuid());
            var referralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());
            var redemption = new Redemption(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            redemption.ReferralCode = referralCode;
            redemption.ReferralSource = referralSource;

            _mockRedemptionsService.GetRedemptionAsync(redemptionId, CancellationToken.None).Returns(Task.FromResult(redemption));
            _mockAuthorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), redemption, "OwnerOnly").Returns(Task.FromResult(AuthorizationResult.Failed()));

            var redemptionsController = new RedemptionsController(_mockAuthorizationService, _mockRedemptionsService);

            // Act
            var result = await redemptionsController.Get(redemptionId, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            await _mockRedemptionsService.Received(1).GetRedemptionAsync(redemptionId, CancellationToken.None);
            await _mockAuthorizationService.Received(1).AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), redemption, "OwnerOnly");
        }

        // Tests that the Post action returns a CreatedAtActionResult with the correct route values when adding a redemption.
        [Fact]
        public async Task Post_ValidAuthId_Should_Return_CreatedAtActionResult()
        {
            // Arrange
            var redemptionId = Guid.NewGuid();
            var referralCode = "123456";
            var referralSource = ReferralSourceEnum.Android;
            var redemptionDto = new RedemptionRequest(referralCode, referralSource);

            _mockRedemptionsService.AddRedemptionAsync(redemptionDto, CancellationToken.None).Returns(Task.FromResult(redemptionId));

            var redemptionsController = new RedemptionsController(_mockAuthorizationService, _mockRedemptionsService);

            // Act
            var result = await redemptionsController.Post(redemptionDto, CancellationToken.None);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(redemptionId, createdAtActionResult.RouteValues?["id"]);
            await _mockRedemptionsService.Received(1).AddRedemptionAsync(redemptionDto, CancellationToken.None);
        }
    }
}