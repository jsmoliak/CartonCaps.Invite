using CartonCaps.Invite.Data.Repositories;
using CartonCaps.Invite.Model.Entities;
using CartonCaps.Invite.Model.ValueObjects;

namespace CartonCaps.Invite.IntegrationTests.Repositories
{
    public class RedemptionsRepositoryTests
    {
        // Tests that the Add method correctly adds a Redemption entity to the database.
        [Fact]
        public async Task Add_Should_Add_Entity()
        {
            // Arrange
            var referrer = new User("123");
            var redeemer = new User("234");
            var referralCode = new ReferralCode("123456", referrer.UserId);
            referrer.ReferralCode = referralCode;
            var dbContext = InMemoryDbContextFactory.Create();

            var usersRepository = new UsersRepository(dbContext);
            await usersRepository.AddAsync(referrer, CancellationToken.None);
            await usersRepository.AddAsync(redeemer, CancellationToken.None);
            await usersRepository.SaveChangesAsync(CancellationToken.None);

            var repository = new RedemptionsRepository(dbContext);
            var redemption = new Redemption(redeemer.UserId, referrer.UserId, referrer.ReferralCode.ReferralCodeId, 1);

            // Act
            repository.Add(redemption);
            await repository.SaveChangesAsync(CancellationToken.None);
            var result = await repository.GetByIdAsync(redemption.RedemptionId, CancellationToken.None);

            // Assert
            Assert.Equivalent(redemption, result);
        }

        // Tests that the AddAsync method correctly adds a Redemption entity asynchronously to the database.
        [Fact]
        public async Task AddAsync_Should_Add_Entity()
        {
            // Arrange
            var referrer = new User("123");
            var redeemer = new User("234");
            var referralCode = new ReferralCode("123456", referrer.UserId);
            referrer.ReferralCode = referralCode;
            var dbContext = InMemoryDbContextFactory.Create();

            var usersRepository = new UsersRepository(dbContext);
            await usersRepository.AddAsync(referrer, CancellationToken.None);
            await usersRepository.AddAsync(redeemer, CancellationToken.None);
            await usersRepository.SaveChangesAsync(CancellationToken.None);

            var repository = new RedemptionsRepository(dbContext);
            var redemption = new Redemption(redeemer.UserId, referrer.UserId, referrer.ReferralCode.ReferralCodeId, 1);

            // Act
            await repository.AddAsync(redemption, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);
            var result = await repository.GetByIdAsync(redemption.RedemptionId, CancellationToken.None);

            // Assert
            Assert.Equivalent(redemption, result);
        }

        // Tests that the GetByIdAsync method correctly retrieves a Redemption entity by its ID.
        [Fact]
        public async Task GetByIdAsync_Should_Get_Entity()
        {
            // Arrange
            var referrer = new User("123");
            var redeemer = new User("234");
            var referralCode = new ReferralCode("123456", referrer.UserId);
            referrer.ReferralCode = referralCode;
            var dbContext = InMemoryDbContextFactory.Create();

            var usersRepository = new UsersRepository(dbContext);
            await usersRepository.AddAsync(referrer, CancellationToken.None);
            await usersRepository.AddAsync(redeemer, CancellationToken.None);
            await usersRepository.SaveChangesAsync(CancellationToken.None);

            var repository = new RedemptionsRepository(dbContext);
            var redemption = new Redemption(redeemer.UserId, referrer.UserId, referrer.ReferralCode.ReferralCodeId, 1);

            // Act
            await repository.AddAsync(redemption, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);
            var result = await repository.GetByIdAsync(redemption.RedemptionId, CancellationToken.None);

            // Assert
            Assert.Equivalent(redemption, result);
        }

        // Tests that the GetAllAsync method correctly retrieves all Redemption entities from the database.
        [Fact]
        public async Task GetAllAsync_Should_Get_Entities()
        {
            // Arrange
            var referrer = new User("123");
            var redeemer = new User("234");
            var referralCode = new ReferralCode("123456", referrer.UserId);
            referrer.ReferralCode = referralCode;
            var dbContext = InMemoryDbContextFactory.Create();

            var usersRepository = new UsersRepository(dbContext);
            await usersRepository.AddAsync(referrer, CancellationToken.None);
            await usersRepository.AddAsync(redeemer, CancellationToken.None);
            await usersRepository.SaveChangesAsync(CancellationToken.None);

            var repository = new RedemptionsRepository(dbContext);
            var redemption = new Redemption(redeemer.UserId, referrer.UserId, referrer.ReferralCode.ReferralCodeId, 1);
            await repository.AddAsync(redemption, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Equivalent(new[] { redemption }, result);
        }

        // Tests that the FindAsync method correctly retrieves Redemption entities based on a predicate.
        [Fact]
        public async Task FindAsync_Should_Get_Entities()
        {
            // Arrange
            var referrer = new User("123");
            var redeemer = new User("234");
            var referralCode = new ReferralCode("123456", referrer.UserId);
            referrer.ReferralCode = referralCode;
            var dbContext = InMemoryDbContextFactory.Create();

            var usersRepository = new UsersRepository(dbContext);
            await usersRepository.AddAsync(referrer, CancellationToken.None);
            await usersRepository.AddAsync(redeemer, CancellationToken.None);
            await usersRepository.SaveChangesAsync(CancellationToken.None);

            var repository = new RedemptionsRepository(dbContext);
            var redemption = new Redemption(redeemer.UserId, referrer.UserId, referrer.ReferralCode.ReferralCodeId, 1);

            await repository.AddAsync(redemption, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.FindAsync(r => r.ReferralSource.SourceName == ReferralSourceEnum.Android.ToString(), CancellationToken.None);

            // Assert
            Assert.Equivalent(new[] { redemption }, result);
        }

        // Tests that the FindFirstOrDefaultAsync method correctly retrieves the first Redemption entity that matches a predicate.
        [Fact]
        public async Task FindFirstOrDefaultAsync_Should_Get_Entity()
        {
            // Arrange
            var referrer = new User("123");
            var redeemer = new User("234");
            var referralCode = new ReferralCode("123456", referrer.UserId);
            referrer.ReferralCode = referralCode;
            var dbContext = InMemoryDbContextFactory.Create();

            var usersRepository = new UsersRepository(dbContext);
            await usersRepository.AddAsync(referrer, CancellationToken.None);
            await usersRepository.AddAsync(redeemer, CancellationToken.None);
            await usersRepository.SaveChangesAsync(CancellationToken.None);

            var repository = new RedemptionsRepository(dbContext);
            var redemption = new Redemption(redeemer.UserId, referrer.UserId, referrer.ReferralCode.ReferralCodeId, 1);

            await repository.AddAsync(redemption, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.FindFirstOrDefaultAsync(r => r.RedemptionId == redemption.RedemptionId, CancellationToken.None);

            // Assert
            Assert.Equivalent(redemption, result);
        }

        // Tests that the Update method correctly updates an existing Redemption entity in the database.
        [Fact]
        public async Task Update_Should_Set_Entity()
        {
            // Arrange
            var referrer = new User("123");
            var redeemer = new User("234");
            var referralCode = new ReferralCode("123456", referrer.UserId);
            referrer.ReferralCode = referralCode;
            var dbContext = InMemoryDbContextFactory.Create();

            var usersRepository = new UsersRepository(dbContext);
            await usersRepository.AddAsync(referrer, CancellationToken.None);
            await usersRepository.AddAsync(redeemer, CancellationToken.None);
            await usersRepository.SaveChangesAsync(CancellationToken.None);

            var repository = new RedemptionsRepository(dbContext);
            var redemption = new Redemption(redeemer.UserId, referrer.UserId, referrer.ReferralCode.ReferralCodeId, 1);

            await repository.AddAsync(redemption, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            var referralSourceRepository = new ReferralSourcesRepository(dbContext);
            var referralSource = await referralSourceRepository.GetByNameAsync(ReferralSourceEnum.iOS.ToString());

            redemption.ReferralSource = referralSource;

            // Act
            repository.Update(redemption);
            await repository.SaveChangesAsync(CancellationToken.None);
            var result = await repository.GetByIdAsync(redemption.RedemptionId, CancellationToken.None);


            // Assert
            Assert.Equivalent(referralSource, result?.ReferralSource);
        }

        // Tests that the Remove method correctly removes a Redemption entity from the database.
        [Fact]
        public async Task Remove_Should_Remove_Entity()
        {
            // Arrange
            var referrer = new User("123");
            var redeemer = new User("234");
            var referralCode = new ReferralCode("123456", referrer.UserId);
            referrer.ReferralCode = referralCode;
            var dbContext = InMemoryDbContextFactory.Create();

            var usersRepository = new UsersRepository(dbContext);
            await usersRepository.AddAsync(referrer, CancellationToken.None);
            await usersRepository.AddAsync(redeemer, CancellationToken.None);
            await usersRepository.SaveChangesAsync(CancellationToken.None);

            var repository = new RedemptionsRepository(dbContext);
            var redemption = new Redemption(redeemer.UserId, referrer.UserId, referrer.ReferralCode.ReferralCodeId, 1);

            await repository.AddAsync(redemption, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            repository.Remove(redemption);
            await repository.SaveChangesAsync(CancellationToken.None);
            var result = await repository.GetByIdAsync(redemption.RedemptionId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
