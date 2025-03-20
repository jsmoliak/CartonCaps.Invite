using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.Model.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Invite.API.Controllers
{
    /// <summary>
    /// Controller for generating referral links.
    /// </summary>
    [Route("invite/api/referral-link")]
    [ApiController]
    [Authorize]
    public class ReferralLinksController : ControllerBase
    {
        private readonly IUserContextService _userContextService;
        private readonly IProfileManagementApiClient _profileManagementApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralLinksController"/> class.
        /// </summary>
        /// <param name="userContextService">The user context service.</param>
        /// <param name="profileManagementApiClient">The profile management API client.</param>
        public ReferralLinksController(IUserContextService userContextService, IProfileManagementApiClient profileManagementApiClient)
        {
            _userContextService = userContextService;
            _profileManagementApiClient = profileManagementApiClient;
        }

        /// <summary>
        /// Retrieves a referral link for the authenticated user.
        /// </summary>
        /// <param name="source">The referral source.</param>
        /// <returns>An IActionResult representing the referral link.</returns>
        /// <response code="200">Returns the referral link.</response>
        /// <response code="400">Returned if the request is invalid (e.g., invalid referral source).</response>
        /// <response code="401">Returned if the user is unauthorized or if the user profile is not found.</response>

        [HttpGet]
        [ProducesResponseType(typeof(ReferralLink), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ReferralLink), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetLink(ReferralSourceEnum source)
        {
            var authId = _userContextService.GetAuthId();
            var userProfile = await _profileManagementApiClient.GetUserProfileAsync(authId);
            if (userProfile is null)
                return Unauthorized();

            var result = new ReferralLink($"https://cartoncaps.link/signup?referral_code={userProfile.ReferralCode}");
            return Ok(result);
        }
    }
}
