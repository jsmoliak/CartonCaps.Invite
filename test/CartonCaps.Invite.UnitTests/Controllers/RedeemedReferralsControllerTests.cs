using CartonCaps.Invite.API.Controllers;
using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace CartonCaps.Invite.UnitTests.Controllers
{
    public class RedeemedReferralsControllerTests
    {
        private readonly IRedemptionsService _mockRedemptionsService;

        public RedeemedReferralsControllerTests()
        {
            _mockRedemptionsService = Substitute.For<IRedemptionsService>();
        }
        // Tests that the List action returns an OkObjectResult with the correct redeemed referrals.
        [Fact]
        public async Task List_Should_Return_OkObjectResult()
        {
            // Arrange
            var referrerAuthId = Guid.NewGuid().ToString();
            var redeemerAuthId = Guid.NewGuid().ToString();
            var redeemerReferralCode = "123456";
            var redeemedAt = new DateTime(2025, 1, 1, 12, 0, 0);
            var referrer = Substitute.For<User>(referrerAuthId);
            var redeemer = Substitute.For<User>(redeemerAuthId);
            var redemption = Substitute.For<Redemption>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            var userProfile = new UserProfile(redeemerAuthId, "John", "Doe", redeemerReferralCode);
            var redeemedReferral = new RedeemedReferral(userProfile.FirstName, userProfile.LastName, userProfile.ReferralCode, redeemedAt);
            ICollection<RedeemedReferral> redeemedReferrals = new List<RedeemedReferral>([redeemedReferral]);

            _mockRedemptionsService.ListRedeemedReferralsAsync().Returns(Task.FromResult(redeemedReferrals));

            var redeemedReferralsController = new RedeemedReferralsController(_mockRedemptionsService);

            // Act
            var result = await redeemedReferralsController.List();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            IEnumerable<ReferralRedemption>? results = okObjectResult.Value as IEnumerable<ReferralRedemption>;
            Assert.Equivalent(ReferralRedemption.FromModel(redeemedReferral), results?.First());
            await _mockRedemptionsService.Received(1).ListRedeemedReferralsAsync();
        }
    }
}