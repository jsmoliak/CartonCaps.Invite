using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CartonCaps.Invite.API.Controllers
{
    /// <summary>
    /// Controller for managing redemptions.
    /// </summary>
    [Route("invite/api/redemptions")]
    [ApiController]
    [Authorize]
    public class RedemptionsController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IRedemptionsService _redemptionsService;
        /// <summary>
        /// Initializes a new instance of the <see cref="RedemptionsController"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service.</param>
        /// <param name="redemptionsService">The redemptions service.</param>

        public RedemptionsController(IAuthorizationService authorizationService, IRedemptionsService redemptionsService)
        {
            _authorizationService = authorizationService;
            _redemptionsService = redemptionsService;
        }

        /// <summary>
        /// Retrieves a redemption by its ID.
        /// </summary>
        /// <param name="id">The ID of the redemption.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>An IActionResult representing the redemption.</returns>
        /// <response code="200">Returns the redemption.</response>
        /// <response code="401">Returned if the user is unauthorized.</response>
        /// <response code="404">Returned if the redemption is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RedemptionRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var redemption = await _redemptionsService.GetRedemptionAsync(id, cancellationToken);
            var authResult = await _authorizationService.AuthorizeAsync(User, redemption, "OwnerOnly");

            return authResult.Succeeded ? Ok(RedemptionResponse.FromModel(redemption)) : NotFound();
        }

        /// <summary>
        /// Creates a new redemption.
        /// </summary>
        /// <param name="redemptionDto">The redemption data transfer object.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the asynchronous operation.</param>
        /// <returns>An IActionResult representing the result of the creation.</returns>
        /// <response code="201">Returned if the redemption is created successfully.</response>
        /// <response code="401">Returned if the user is unauthorized.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post(RedemptionRequest redemptionDto, CancellationToken cancellationToken)
        {
            var redemptionId = await _redemptionsService.AddRedemptionAsync(redemptionDto, cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = redemptionId }, "");
        }
    }
}
