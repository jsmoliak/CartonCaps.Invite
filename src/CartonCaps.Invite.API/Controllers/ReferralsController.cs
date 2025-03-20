using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Invite.API.Controllers
{
    /// <summary>
    /// Controller for managing referrals.
    /// </summary>
    [Route("invite/api/referrals")]
    [ApiController]
    [Authorize]
    public class ReferralsController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IReferralsService _referralsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralsController"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="referralsService">The referrals service.</param>
        public ReferralsController(IAuthorizationService authorizationService, IReferralsService referralsService)
        {
            _authorizationService = authorizationService;
            _referralsService = referralsService;
        }

        /// <summary>
        /// Retrieves a list of referrals.
        /// </summary>
        /// <returns>An IActionResult representing the list of referrals.</returns>
        /// <response code="200">Returns the list of referrals.</response>
        /// <response code="401">Returned if the user is unauthorized.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ReferralResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List()
        {
            var referrals = await _referralsService.ListReferralsAsync();
            return Ok(referrals.Select(r => ReferralResponse.FromModel(r)));
        }

        /// <summary>
        /// Retrieves a referral by its ID.
        /// </summary>
        /// <param name="id">The ID of the referral.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>An IActionResult representing the referral.</returns>
        /// <response code="200">Returns the referral.</response>
        /// <response code="401">Returned if the user is unauthorized.</response>
        /// <response code="404">Returned if the referral is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReferralResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var referral = await _referralsService.GetReferralAsync(id, cancellationToken);

            var authResult = await _authorizationService.AuthorizeAsync(User, referral, "OwnerOnly");
            return authResult.Succeeded ? Ok(ReferralResponse.FromModel(referral)) : NotFound();
        }

        /// <summary>
        /// Creates a new referral.
        /// </summary>
        /// <param name="referralDto">The referral data transfer object.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>An IActionResult representing the result of the creation.</returns>
        /// <response code="201">Returned if the referral is created successfully.</response>
        /// <response code="400">Returned if the referral request is invalid.</response>
        /// <response code="401">Returned if the user is unauthorized.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post(ReferralRequest referralDto, CancellationToken cancellationToken)
        {
            var referralId = await _referralsService.AddReferralAsync(referralDto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = referralId }, null);
        }
    }
}
