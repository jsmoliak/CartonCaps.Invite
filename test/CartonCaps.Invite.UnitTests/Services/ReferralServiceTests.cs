using CartonCaps.Invite.API.Dto;
using CartonCaps.Invite.API.Interfaces;
using CartonCaps.Invite.API.Services;
using CartonCaps.Invite.Data.Interfaces;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.Exceptions;
using CartonCaps.Invite.Model.ValueObjects;
using NSubstitute;

namespace CartonCaps.Invite.UnitTests.Services
{
    public class ReferralsServiceTests
    {
        private readonly IUserContextService _mockUserContextService;
        private readonly IUsersRepository _mockUsersRepository;
        private readonly IReferralsRepository _mockReferralsRepository;
        private readonly IReferralSourcesRepository _mockReferralSourcesRepository;
        private readonly IProfileManagementApiClient _mockProfileManagementApiClient;

        public ReferralsServiceTests()
        {
            _mockUserContextService = Substitute.For<IUserContextService>();
            _mockUsersRepository = Substitute.For<IUsersRepository>();
            _mockReferralsRepository = Substitute.For<IReferralsRepository>();
            _mockReferralSourcesRepository = Substitute.For<IReferralSourcesRepository>();
            _mockProfileManagementApiClient = Substitute.For<IProfileManagementApiClient>();
        }

        // Tests that ListReferralsAsync returns the correct referrals when a valid authId is provided.
        [Fact]
        public async Task ListReferralsAsync_Existing_Should_Return_Referral()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid();
            var referral = Substitute.For<Referral>(Guid.NewGuid(), Guid.NewGuid(), 1);
            var referrer = Substitute.For<User>(authId);

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockUsersRepository.GetByAuthIdAsync(authId).Returns(Task.FromResult<User?>(referrer));
            referrer.Referrals.Returns([referral]);
            referral.Referrer.Returns(referrer);

            var referralsService = new ReferralsService(_mockUserContextService, _mockUsersRepository, _mockReferralsRepository, _mockReferralSourcesRepository);

            // Act
            var result = await referralsService.ListReferralsAsync();

            // Assert
            Assert.Equivalent(referrer.Referrals, result);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(authId);
        }

        // Tests that ListReferralsAsync returns an empty collection when the referrer is not found.
        [Fact]
        public async Task ListReferralsAsync_MissingReferrer_Should_Return_EmptyCollection()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid();
            var referral = Substitute.For<Referral>(Guid.NewGuid(), Guid.NewGuid(), 1);
            var referrer = Substitute.For<User>(authId);

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockUsersRepository.GetByAuthIdAsync(authId).Returns(Task.FromResult<User?>(null));

            var referralsService = new ReferralsService(_mockUserContextService, _mockUsersRepository, _mockReferralsRepository, _mockReferralSourcesRepository);

            // Act
            var result = await referralsService.ListReferralsAsync();

            // Assert
            Assert.Empty(result);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(authId);
        }

        // Tests that GetReferralAsync returns the correct referral when a valid referralId is provided.
        [Fact]
        public async Task GetReferralAsync_Existing_Should_Return_Referral()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid();
            var referral = Substitute.For<Referral>(Guid.NewGuid(), Guid.NewGuid(), 1);
            var referrer = Substitute.For<User>(authId);

            _mockReferralsRepository.GetByIdAsync(referralId, CancellationToken.None).Returns(Task.FromResult<Referral?>(referral));
            referral.Referrer.Returns(referrer);

            var referralsService = new ReferralsService(_mockUserContextService, _mockUsersRepository, _mockReferralsRepository, _mockReferralSourcesRepository);

            // Act
            var result = await referralsService.GetReferralAsync(referralId, CancellationToken.None);

            // Assert
            Assert.Equivalent(referral, result);
            await _mockReferralsRepository.Received(1).GetByIdAsync(referralId, CancellationToken.None);
        }

        // Tests that GetReferralAsync throws a ResourceNotFoundException when the referral is not found.
        [Fact]
        public async Task GetReferralAsync_Missing_Should_Throw_ResourceNotFoundException()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var referralId = Guid.NewGuid();

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockReferralsRepository.GetByIdAsync(referralId, CancellationToken.None).Returns(Task.FromResult<Referral?>(null));

            var referralsService = new ReferralsService(_mockUserContextService, _mockUsersRepository, _mockReferralsRepository, _mockReferralSourcesRepository);

            // Act, Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
                await referralsService.GetReferralAsync(referralId, CancellationToken.None)
            );
            await _mockReferralsRepository.Received(1).GetByIdAsync(referralId, CancellationToken.None);
        }

        // Tests that AddReferral returns the correct referralId when a valid referral is added.
        [Fact]
        public async Task AddReferral_Valid_Should_Return_ReferralId()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var code = "123456";
            var referralCode = new ReferralCode(code, Guid.NewGuid());
            var referralSourceEnum = ReferralSourceEnum.Android;
            var referralSource = new ReferralSource(1, referralSourceEnum.ToString());
            var referralId = Guid.NewGuid();
            var referral = Substitute.For<Referral>(Guid.NewGuid(), Guid.NewGuid(), 1);
            var referrer = Substitute.For<User>(authId);
            var referralDto = new ReferralRequest(code, referralSourceEnum);

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockUsersRepository.GetByAuthIdAsync(authId).Returns(Task.FromResult<User?>(referrer));
            _mockReferralSourcesRepository.GetByNameAsync(referralSourceEnum.ToString()).Returns(Task.FromResult(referralSource));
            referrer.ReferralCode.Returns(referralCode);
            _mockUsersRepository.SaveChangesAsync(CancellationToken.None).Returns(Task.FromResult(1));

            var referralsService = new ReferralsService(_mockUserContextService, _mockUsersRepository, _mockReferralsRepository, _mockReferralSourcesRepository);

            // Act
            var result = await referralsService.AddReferralAsync(referralDto, CancellationToken.None);

            // Assert
            Assert.Equivalent(Guid.Empty, result);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(authId);
            await _mockReferralSourcesRepository.Received(1).GetByNameAsync(referralSourceEnum.ToString());
            referrer.Received(1).AddReferral(referralCode.ReferralCodeId, referralSource.ReferralSourceId);
            await _mockUsersRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }

        // Tests that AddReferral creates a new referrer if one does not exist.
        [Fact]
        public async Task AddReferral_MissingReferrer_Should_Return_ReferralId()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var code = "123456";
            var referralCode = new ReferralCode(code, Guid.NewGuid());
            var referralSourceEnum = ReferralSourceEnum.Android;
            var referralSource = new ReferralSource(1, referralSourceEnum.ToString());
            var referralId = Guid.NewGuid();
            var referral = Substitute.For<Referral>(Guid.NewGuid(), Guid.NewGuid(), 1);
            var referrer = Substitute.For<User>(authId);
            var referralDto = new ReferralRequest(code, referralSourceEnum);

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockUsersRepository.GetByAuthIdAsync(authId).Returns(Task.FromResult<User?>(null));
            _mockUsersRepository.AddAsync(Arg.Any<User>(), CancellationToken.None).Returns(Task.CompletedTask);
            _mockReferralSourcesRepository.GetByNameAsync(referralSourceEnum.ToString()).Returns(Task.FromResult(referralSource));
            referrer.ReferralCode.Returns(referralCode);
            _mockUsersRepository.SaveChangesAsync(CancellationToken.None).Returns(Task.FromResult(1));

            var referralsService = new ReferralsService(_mockUserContextService, _mockUsersRepository, _mockReferralsRepository, _mockReferralSourcesRepository);

            // Act
            var result = await referralsService.AddReferralAsync(referralDto, CancellationToken.None);

            // Assert
            Assert.Equivalent(Guid.Empty, result);
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(authId);
            await _mockUsersRepository.Received(1).AddAsync(Arg.Any<User>(), CancellationToken.None);
            await _mockReferralSourcesRepository.Received(1).GetByNameAsync(referralSourceEnum.ToString());
            referrer.Received(1).AddReferral(referralCode.ReferralCodeId, referralSource.ReferralSourceId);
            await _mockUsersRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }

        // Tests that AddReferral throws a ReferralCodeNotFoundException when the referral code is mismatched
        [Fact]
        public async Task AddReferral_MismatchedReferralCode_Should_Throw_ReferralCodeNotFoundException()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var code1 = "123456";
            var code2 = "234567";
            var referralCode1 = new ReferralCode(code1, Guid.NewGuid());
            var referralCode2 = new ReferralCode(code2, Guid.NewGuid());
            var referralSourceEnum = ReferralSourceEnum.Android;
            var referralSource = new ReferralSource(1, referralSourceEnum.ToString());
            var referralId = Guid.NewGuid();
            var referral = Substitute.For<Referral>(Guid.NewGuid(), Guid.NewGuid(), 1);
            var referrer = Substitute.For<User>(authId);
            var referralDto = new ReferralRequest(code1, referralSourceEnum);

            _mockUserContextService.GetAuthId().Returns(authId);
            _mockUsersRepository.GetByAuthIdAsync(authId).Returns(Task.FromResult<User?>(referrer));
            _mockReferralSourcesRepository.GetByNameAsync(referralSourceEnum.ToString()).Returns(Task.FromResult(referralSource));
            referrer.ReferralCode.Returns(referralCode2);
            _mockUsersRepository.SaveChangesAsync(CancellationToken.None).Returns(Task.FromResult(1));

            var referralsService = new ReferralsService(_mockUserContextService, _mockUsersRepository, _mockReferralsRepository, _mockReferralSourcesRepository);

            // Act, Assert
            await Assert.ThrowsAsync<ReferralCodeNotFoundException>(async () =>
                await referralsService.AddReferralAsync(referralDto, CancellationToken.None)
            );
            _mockUserContextService.Received(1).GetAuthId();
            await _mockUsersRepository.Received(1).GetByAuthIdAsync(authId);
            await _mockReferralSourcesRepository.Received(1).GetByNameAsync(referralSourceEnum.ToString());
            referrer.Received(0).AddReferral(referralCode1.ReferralCodeId, referralSource.ReferralSourceId);
            await _mockUsersRepository.Received(0).SaveChangesAsync(CancellationToken.None);
        }
    }
}