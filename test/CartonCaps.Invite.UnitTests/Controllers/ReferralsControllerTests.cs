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
    public class ReferralsControllerTests
    {
        private readonly IAuthorizationService _mockAuthorizationService;
        private readonly IReferralsService _mockReferralsService;

        public ReferralsControllerTests()
        {
            _mockAuthorizationService = Substitute.For<IAuthorizationService>();
            _mockReferralsService = Substitute.For<IReferralsService>();
        }
        // Tests that the List action returns an OkObjectResult with the correct referrals.
        [Fact]
        public async Task List_Should_Return_OkObjectResult()
        {
            // Arrange
            var referral = new Referral(Guid.NewGuid(), Guid.NewGuid(), 1);
            referral.ReferralCode = new ReferralCode("123456", Guid.NewGuid());
            referral.ReferralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());
            IReadOnlyCollection<Referral> referrals = new List<Referral>([referral]);

            _mockReferralsService.ListReferralsAsync().Returns(Task.FromResult(referrals));

            var referralsController = new ReferralsController(_mockAuthorizationService, _mockReferralsService);

            // Act
            var result = await referralsController.List();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            IEnumerable<ReferralResponse>? results = okObjectResult.Value as IEnumerable<ReferralResponse>;
            Assert.Equivalent(ReferralResponse.FromModel(referral), results?.First());
            await _mockReferralsService.Received(1).ListReferralsAsync();
        }

        // Tests that the Get action returns an OkObjectResult with the correct referral when authorization succeeds.
        [Fact]
        public async Task Get_ValidAuthId_Should_Return_OkObjectResult()
        {
            // Arrange
            var referralId = Guid.NewGuid();
            var referral = new Referral(Guid.NewGuid(), Guid.NewGuid(), 1);
            referral.ReferralCode = new ReferralCode("123456", Guid.NewGuid());
            referral.ReferralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());

            _mockReferralsService.GetReferralAsync(referralId, CancellationToken.None).Returns(Task.FromResult(referral));
            _mockAuthorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), referral, "OwnerOnly").Returns(Task.FromResult(AuthorizationResult.Success()));

            var referralsController = new ReferralsController(_mockAuthorizationService, _mockReferralsService);

            // Act
            var result = await referralsController.Get(referralId, CancellationToken.None);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equivalent(okObjectResult.Value, ReferralResponse.FromModel(referral));
            await _mockReferralsService.Received(1).GetReferralAsync(referralId, CancellationToken.None);
            await _mockAuthorizationService.Received(1).AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), referral, "OwnerOnly");
        }

        // Tests that the Get action returns a NotFoundResult when authorization fails.
        [Fact]
        public async Task Get_ForbiddenAuthId_Should_Return_UnauthorizedResult()
        {
            // Arrange
            var referralId = Guid.NewGuid();
            var referral = new Referral(Guid.NewGuid(), Guid.NewGuid(), 1);
            referral.ReferralCode = new ReferralCode("123456", Guid.NewGuid());
            referral.ReferralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());

            _mockReferralsService.GetReferralAsync(referralId, CancellationToken.None).Returns(Task.FromResult(referral));
            _mockAuthorizationService.AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), referral, "OwnerOnly").Returns(Task.FromResult(AuthorizationResult.Failed()));

            var referralsController = new ReferralsController(_mockAuthorizationService, _mockReferralsService);

            // Act
            var result = await referralsController.Get(referralId, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            await _mockReferralsService.Received(1).GetReferralAsync(referralId, CancellationToken.None);
            await _mockAuthorizationService.Received(1).AuthorizeAsync(Arg.Any<ClaimsPrincipal>(), referral, "OwnerOnly");
        }

        // Tests that the Post action returns a CreatedAtActionResult with the correct route values when adding a referral.
        [Fact]
        public async Task Post_ValidAuthId_Should_Return_CreatedAtActionResult()
        {
            // Arrange
            var referralId = Guid.NewGuid();
            var authId = Guid.NewGuid().ToString();
            var referral = new Referral(Guid.NewGuid(), Guid.NewGuid(), 1);
            referral.ReferralCode = new ReferralCode("123456", Guid.NewGuid());
            referral.ReferralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());
            var referralDto = ReferralRequest.FromModel(referral);

            _mockReferralsService.AddReferralAsync(referralDto, CancellationToken.None).Returns(Task.FromResult(referralId));

            var referralsController = new ReferralsController(_mockAuthorizationService, _mockReferralsService);

            // Act
            var result = await referralsController.Post(referralDto, CancellationToken.None);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(referralId, createdAtActionResult.RouteValues?["id"]);
            await _mockReferralsService.Received(1).AddReferralAsync(referralDto, CancellationToken.None);
        }
    }
}