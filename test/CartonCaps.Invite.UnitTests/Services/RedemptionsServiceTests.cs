using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.API.Services;
using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.Exceptions;
using CartonCaps.Invite.Model.ValueObjects;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CartonCaps.Invite.UnitTests.Services
{
    public class RedemptionsServiceTests
    {
        private readonly IUserContextService _mockUserContextService;
        private readonly IUsersRepository _mockUsersRepository;
        private readonly IRedemptionsRepository _mockRedemptionsRepository;
        private readonly IReferralSourcesRepository _mockReferralSourcesRepository;
        private readonly IProfileManagementApiClient _mockProfileManagementApiClient;

        public RedemptionsServiceTests()
        {
            _mockUserContextService = Substitute.For<IUserContextService>();
            _mockUsersRepository = Substitute.For<IUsersRepository>();
            _mockRedemptionsRepository = Substitute.For<IRedemptionsRepository>();
            _mockReferralSourcesRepository = Substitute.For<IReferralSourcesRepository>();
            _mockProfileManagementApiClient = Substitute.For<IProfileManagementApiClient>();
        }

        // Tests that GetRedemptionAsync returns the correct Redemption when a valid authId is provided.
        [Fact]
        public async Task GetRedemptionAsync_ValidAuthId_Should_Return_Redemption()
        {
            // Arrange
            var redemptionId = Guid.NewGuid();
            var authId = Guid.NewGuid().ToString();
            var redemption = new Redemption(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            redemption.Redeemer = new User(authId);

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockRedemptionsRepository.GetByIdAsync(redemptionId, CancellationToken.None).Returns(Task.FromResult<Redemption?>(redemption));

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act
            var result = await redemptionsService.GetRedemptionAsync(redemptionId, CancellationToken.None);

            // Assert
            Assert.Equivalent(redemption, result);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockRedemptionsRepository.Received(1).GetByIdAsync(redemptionId, CancellationToken.None);
        }

        // Tests that GetRedemptionAsync throws ResourceNotFoundException when an invalid authId is provided.
        [Fact]
        public async Task GetRedemptionAsync_InvalidAuthId_Should_Throw_ResourceNotFoundException()
        {
            // Arrange
            var redemptionId = Guid.NewGuid();
            var authId = Guid.NewGuid().ToString();
            var badId = Guid.NewGuid().ToString();
            var redemption = new Redemption(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            redemption.Redeemer = new User(badId);

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockRedemptionsRepository.GetByIdAsync(redemptionId, CancellationToken.None).Returns(Task.FromResult<Redemption?>(redemption));

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act, Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
                await redemptionsService.GetRedemptionAsync(redemptionId, CancellationToken.None)
            );
            _mockUserContextService.Received(1).GetAuthId();
            await _mockRedemptionsRepository.Received(1).GetByIdAsync(redemptionId, CancellationToken.None);
        }

        // Tests that GetRedemptionAsync throws ResourceNotFoundException when the Redemption is not found.
        [Fact]
        public async Task GetRedemptionAsync_MissingRedemption_Should_Throw_ResourceNotFoundException()
        {
            // Arrange
            var redemptionId = Guid.NewGuid();
            var authId = Guid.NewGuid().ToString();

            _mockRedemptionsRepository.GetByIdAsync(redemptionId, CancellationToken.None).Returns(Task.FromResult<Redemption?>(null));

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act, Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
                await redemptionsService.GetRedemptionAsync(redemptionId, CancellationToken.None)
            );
            _mockUserContextService.Received(1).GetAuthId();
            await _mockRedemptionsRepository.Received(1).GetByIdAsync(redemptionId, CancellationToken.None);
        }

        // Tests that ListRedeemedReferralsAsync returns a list of UserProfiles when a valid authId is provided.
        [Fact]
        public async Task ListRedeemedReferralsAsync_ValidAuthId_Should_Return_UserProfiles()
        {
            // Arrange
            var referrerAuthId = Guid.NewGuid().ToString();
            var redeemerAuthId = Guid.NewGuid().ToString();
            var redeemerReferralCode = "123456";
            var referrer = Substitute.For<User>(referrerAuthId);
            var redeemer = Substitute.For<User>(redeemerAuthId);
            var redemption = Substitute.For<Redemption>(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            var userProfile = new UserProfile(redeemerAuthId, "John", "Doe", redeemerReferralCode);
            var redeemedReferral = new RedeemedReferral(userProfile.FirstName, userProfile.LastName, userProfile.ReferralCode, redemption.RedeemedAt);
            ICollection<RedeemedReferral> redeemedReferrals = new List<RedeemedReferral>([redeemedReferral]);

            _mockUserContextService.GetAuthId().Returns(referrerAuthId);
            _mockUsersRepository.GetByAuthIdAsync(referrerAuthId).Returns(Task.FromResult<User?>(referrer));
            referrer.RedeemedReferrals.Returns([redemption]);
            redemption.Redeemer.Returns(redeemer);
            _mockProfileManagementApiClient.GetUserProfileAsync(redeemerAuthId).Returns(userProfile);

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act
            var result = await redemptionsService.ListRedeemedReferralsAsync();

            // Assert
            Assert.Equivalent(redeemedReferral, result.First());
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(referrerAuthId);
            await _mockProfileManagementApiClient.Received(1).GetUserProfileAsync(redeemerAuthId);
        }

        // Tests that ListReferralsAsync returns an empty collection when the referrer is not found.
        [Fact]
        public async Task ListReferralsAsync_MissingReferrer_Should_Return_EmptyCollection()
        {
            // Arrange
            var referrerAuthId = Guid.NewGuid().ToString();

            _mockUserContextService.GetAuthId().Returns(referrerAuthId);
            _mockUsersRepository.GetByAuthIdAsync(referrerAuthId).Returns(Task.FromResult<User?>(null));

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act
            var result = await redemptionsService.ListRedeemedReferralsAsync();

            // Assert
            Assert.Empty(result);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(referrerAuthId);
            await _mockProfileManagementApiClient.Received(0).GetUserProfileAsync(Arg.Any<string>());
        }

        // Tests that AddRedemptionAsync returns a RedemptionId when valid input is provided.
        [Fact]
        public async Task AddRedemptionAsync_Valid_Should_Return_RedemptionId()
        {
            // Arrange
            var code = "123456";
            var referralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());
            var referralSourceEnum = ReferralSourceEnum.Android;
            var redemptionDto = new RedemptionRequest(code, referralSourceEnum);
            var redeemerAuthId = Guid.NewGuid().ToString();
            var referrerAuthId = Guid.NewGuid().ToString();
            var redeemerId = Guid.NewGuid();
            var referrerId = Guid.NewGuid();
            var redeemer = Substitute.For<User>(redeemerId.ToString());
            var referrer = Substitute.For<User>(referrerId.ToString());
            var referralCode = new ReferralCode(code, redeemerId);

            var redemption = new Redemption(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            redemption.Redeemer = new User(redeemerId.ToString());

            _mockUserContextService.GetAuthId().Returns(redeemerAuthId);
            _mockUsersRepository.GetByReferralCodeAsync(code).Returns(Task.FromResult<User?>(referrer));
            _mockUsersRepository.GetByAuthIdAsync(redeemerAuthId).Returns(Task.FromResult<User?>(redeemer));
            _mockReferralSourcesRepository.GetByNameAsync(referralSourceEnum.ToString()).Returns(referralSource);
            referrer.ReferralCode.Returns(referralCode);
            _mockUsersRepository.SaveChangesAsync(CancellationToken.None).Returns(Task.FromResult(1));

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act
            var redemptionId = await redemptionsService.AddRedemptionAsync(redemptionDto, CancellationToken.None);

            // Assert
            Assert.Equal(redemptionId, Guid.Empty);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByReferralCodeAsync(code);
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(redeemerAuthId);
            await _mockReferralSourcesRepository.Received(1).GetByNameAsync(referralSourceEnum.ToString());
            redeemer.Received(1).AddRedemption(referrerId, referralCode.ReferralCodeId, referralSource.ReferralSourceId);
            await _mockUsersRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }

        // Tests that AddRedemptionAsync throws ReferralCodeNotFoundException when the referrer is not found.
        [Fact]
        public async Task AddRedemptionAsync_MissingReferrer_Should_Throw_ReferralCodeNotFoundException()
        {
            // Arrange
            var code = "123456";
            var referralSourceEnum = ReferralSourceEnum.Android;
            var redemptionDto = new RedemptionRequest(code, referralSourceEnum);
            var redeemerId = Guid.NewGuid();

            _mockUsersRepository.GetByReferralCodeAsync(code).Returns(Task.FromResult<User?>(null));

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act, Assert
            await Assert.ThrowsAsync<ReferralCodeNotFoundException>(async () =>
                await redemptionsService.AddRedemptionAsync(redemptionDto, CancellationToken.None)
            );
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByReferralCodeAsync(code);
            await _mockUsersRepository.Received(0).GetByAuthIdAsync(Arg.Any<string>());
            await _mockReferralSourcesRepository.Received(0).GetByNameAsync(Arg.Any<string>());
            await _mockUsersRepository.Received(0).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        // Tests that AddRedemptionAsync adds a new redeemer if they do not exist, and correctly creates a redemption.
        [Fact]
        public async Task AddRedemptionAsync_MissingRedeemer_Should_Return_RedemptionId()
        {
            // Arrange
            var code = "123456";
            var referralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());
            var referralSourceEnum = ReferralSourceEnum.Android;
            var redemptionDto = new RedemptionRequest(code, referralSourceEnum);
            var redeemerAuthId = Guid.NewGuid().ToString();
            var referrerAuthId = Guid.NewGuid().ToString();
            var redeemerId = Guid.NewGuid();
            var referrerId = Guid.NewGuid();
            var redeemer = Substitute.For<User>(redeemerAuthId.ToString());
            var referrer = Substitute.For<User>(referrerAuthId.ToString());
            var referralCode = new ReferralCode(code, referrerId);

            var redemption = new Redemption(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            redemption.Redeemer = new User(redeemerAuthId.ToString());

            _mockUserContextService.GetAuthId().Returns(redeemerAuthId);
            _mockUsersRepository.GetByReferralCodeAsync(code).Returns(Task.FromResult<User?>(referrer));
            _mockUsersRepository.GetByAuthIdAsync(redeemerAuthId).Returns(Task.FromResult<User?>(null));
            _mockUsersRepository.AddAsync(Arg.Any<User>(), CancellationToken.None).Returns(Task.CompletedTask);
            _mockReferralSourcesRepository.GetByNameAsync(referralSourceEnum.ToString()).Returns(referralSource);
            referrer.ReferralCode.Returns(referralCode);
            _mockUsersRepository.SaveChangesAsync(CancellationToken.None).Returns(Task.FromResult(1));

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act
            var redemptionId = await redemptionsService.AddRedemptionAsync(redemptionDto, CancellationToken.None);

            // Assert
            Assert.Equal(redemptionId, Guid.Empty);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByReferralCodeAsync(code);
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(redeemerAuthId.ToString());
            await _mockUsersRepository.Received(1).AddAsync(Arg.Any<User>(), CancellationToken.None);
            await _mockReferralSourcesRepository.Received(1).GetByNameAsync(referralSourceEnum.ToString());
            redeemer.Received(1).AddRedemption(referrerId, referralCode.ReferralCodeId, referralSource.ReferralSourceId);
            await _mockUsersRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }

        // Tests that AddRedemptionAsync throws UniqueRedemptionConstraintException when SaveChangesAsync fails due to a unique constraint violation.
        [Fact]
        public async Task AddRedemptionAsync_SaveChangesAsyncConflict_Should_Throw_UniqueRedemptionConstraintException()
        {
            // Arrange
            var code = "123456";
            var referralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());
            var referralSourceEnum = ReferralSourceEnum.Android;
            var redemptionDto = new RedemptionRequest(code, referralSourceEnum);
            var redeemerAuthId = Guid.NewGuid().ToString();
            var referrerAuthId = Guid.NewGuid().ToString();
            var redeemerId = Guid.NewGuid();
            var referrerId = Guid.NewGuid();
            var redeemer = Substitute.For<User>(redeemerId.ToString());
            var referrer = Substitute.For<User>(referrerId.ToString());
            var referralCode = new ReferralCode(code, redeemerId);
            var redemption = new Redemption(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            redemption.Redeemer = new User(redeemerAuthId);
            var sqliteException = new SqliteException("SQLite Error 19: UNIQUE constraint failed", 19);
            var dbUpdateException = new DbUpdateException("An error occurred while updating the database.", sqliteException);

            _mockUserContextService.GetAuthId().Returns(redeemerAuthId);
            _mockUsersRepository.GetByReferralCodeAsync(code).Returns(Task.FromResult<User?>(referrer));
            _mockUsersRepository.GetByAuthIdAsync(redeemerAuthId).Returns(Task.FromResult<User?>(redeemer));
            _mockReferralSourcesRepository.GetByNameAsync(referralSourceEnum.ToString()).Returns(referralSource);
            referrer.ReferralCode.Returns(referralCode);
            _mockUsersRepository.SaveChangesAsync(CancellationToken.None).ThrowsAsync(dbUpdateException);

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act, Assert
            await Assert.ThrowsAsync<UniqueRedemptionConstraintException>(async () =>
                await redemptionsService.AddRedemptionAsync(redemptionDto, CancellationToken.None)
            );
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByReferralCodeAsync(code);
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(redeemerAuthId);
            await _mockReferralSourcesRepository.Received(1).GetByNameAsync(referralSourceEnum.ToString());
            redeemer.Received(1).AddRedemption(referrerId, referralCode.ReferralCodeId, referralSource.ReferralSourceId);
            await _mockUsersRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }

        // Tests that AddRedemptionAsync throws DbUpdateException when SaveChangesAsync fails with a generic DbUpdateException.
        [Fact]
        public async Task AddRedemptionAsync_SaveChangesAsyncDbUpdateException_Should_Throw_DbUpdateException()
        {
            // Arrange
            var code = "123456";
            var referralSource = new ReferralSource(1, ReferralSourceEnum.Android.ToString());
            var referralSourceEnum = ReferralSourceEnum.Android;
            var redemptionDto = new RedemptionRequest(code, referralSourceEnum);
            var redeemerAuthId = Guid.NewGuid().ToString();
            var referrerAuthId = Guid.NewGuid().ToString();
            var redeemerId = Guid.NewGuid();
            var referrerId = Guid.NewGuid();
            var redeemer = Substitute.For<User>(redeemerId.ToString());
            var referrer = Substitute.For<User>(referrerId.ToString());
            var referralCode = new ReferralCode(code, redeemerId);
            var redemption = new Redemption(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
            redemption.Redeemer = new User(redeemerId.ToString());
            var dbUpdateException = new DbUpdateException("An error occurred while updating the database.");

            _mockUserContextService.GetAuthId().Returns(redeemerAuthId);
            _mockUsersRepository.GetByReferralCodeAsync(code).Returns(Task.FromResult<User?>(referrer));
            _mockUsersRepository.GetByAuthIdAsync(redeemerAuthId).Returns(Task.FromResult<User?>(redeemer));
            _mockReferralSourcesRepository.GetByNameAsync(referralSourceEnum.ToString()).Returns(referralSource);
            referrer.ReferralCode.Returns(referralCode);
            _mockUsersRepository.SaveChangesAsync(CancellationToken.None).ThrowsAsync(dbUpdateException);

            var redemptionsService = new RedemptionsService(_mockUserContextService, _mockUsersRepository, _mockRedemptionsRepository, _mockReferralSourcesRepository, _mockProfileManagementApiClient);

            // Act, Assert
            await Assert.ThrowsAsync<DbUpdateException>(async () =>
                await redemptionsService.AddRedemptionAsync(redemptionDto, CancellationToken.None)
            );
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByReferralCodeAsync(code);
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(redeemerAuthId);
            await _mockReferralSourcesRepository.Received(1).GetByNameAsync(referralSourceEnum.ToString());
            redeemer.Received(1).AddRedemption(referrerId, referralCode.ReferralCodeId, referralSource.ReferralSourceId);
            await _mockUsersRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }
    }
}