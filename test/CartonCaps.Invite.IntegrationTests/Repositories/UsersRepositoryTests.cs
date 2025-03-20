using CartonCaps.Invite.Data.Repositories;
using CartonCaps.Invite.Model.Entities;

namespace CartonCaps.Invite.IntegrationTests.Repositories
{
    public class UsersRepositoryTests
    {
        // Tests that GetByAuthIdAsync returns the correct User when given a valid AuthId.
        [Fact]
        public async Task GetByAuthIdAsync_Should_Return_User()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var user = new User(authId);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new UsersRepository(dbContext);

            await repository.AddAsync(user, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetByAuthIdAsync(authId);

            // Assert
            Assert.Equivalent(user, result);
        }

        // Tests that GetByReferralCodeAsync returns the correct User when given a valid ReferralCode.
        [Fact]
        public async Task GetByReferralCodeAsync_Should_Return_User()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var user = new User(authId);
            var code = "123456";
            user.ReferralCode = new ReferralCode(code, user.UserId);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new UsersRepository(dbContext);

            await repository.AddAsync(user, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetByReferralCodeAsync(code);

            // Assert
            Assert.Equivalent(user, result);
        }
    }
}