using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Invite.API.Controllers
{
    /// <summary>
    /// Controller for managing redeemed referrals.
    /// </summary>
    [Route("invite/api/redeemed-referrals")]
    [ApiController]
    [Authorize]
    public class RedeemedReferralsController : ControllerBase
    {
        private readonly IRedemptionsService _redemptionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedeemedReferralsController"/> class.
        /// </summary>
        /// <param name="redemptionsService">The service for managing redemptions.</param>
        public RedeemedReferralsController(IRedemptionsService redemptionsService)
        {
            _redemptionsService = redemptionsService;
        }

        /// <summary>
        /// Retrieves a list of redeemed referrals.
        /// </summary>
        /// <returns>An IActionResult representing the list of redeemed referrals.</returns>
        /// <response code="200">Returns the list of redeemed referrals.</response>
        /// <response code="401">Returned if the user is unauthorized.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ReferralRedemption>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List()
        {
            var redeemedReferrals = await _redemptionsService.ListRedeemedReferralsAsync();

            var referralRedemptions = new List<ReferralRedemption>();
            foreach (var redeemedReferral in redeemedReferrals)
            {
                referralRedemptions.Add(ReferralRedemption.FromModel(redeemedReferral));
            }

            return Ok(referralRedemptions);
        }
    }
}
