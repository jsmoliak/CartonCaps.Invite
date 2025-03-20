using CartonCaps.Invite.Data.Repositories;
using CartonCaps.Invite.Model.Exceptions;
using CartonCaps.Invite.Model.ValueObjects;

namespace CartonCaps.Invite.IntegrationTests.Repositories
{
    public class ReferralSourcesRepositoryTests
    {
        // Tests that GetByNameAsync returns the correct ReferralSource when it exists.
        [Fact]
        public async Task GetByNameAsync_Should_Return_ReferralSource()
        {
            // Arrange
            var referralSourceAndroid = ReferralSourceEnum.Android.ToString();

            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new ReferralSourcesRepository(dbContext);

            // Act
            var result = await repository.GetByNameAsync(referralSourceAndroid);

            // Assert
            Assert.Equal(referralSourceAndroid, result.SourceName);
        }

        // Tests that GetByNameAsync throws ReferralSourceNotFoundException when the ReferralSource does not exist.
        [Fact]
        public async Task GetByNameAsync_Should_Throw_ReferralSourceNotFoundException()
        {
            // Arrange

            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new ReferralSourcesRepository(dbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ReferralSourceNotFoundException>(async () =>
                await repository.GetByNameAsync("NonExisting")
            );
        }
    }
}