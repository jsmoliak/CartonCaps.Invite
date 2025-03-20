using CartonCaps.Invite.Data.Repositories;
using CartonCaps.Invite.Model.Entities;

namespace CartonCaps.Invite.IntegrationTests.Repositories
{
    public class RepositoryBaseTests
    {
        // Tests that the Add method correctly inserts an entity into the database.
        [Fact]
        public async Task Add_Should_Add_Entity()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var user = new User(authId);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            // Act
            repository.Add(user);
            await repository.SaveChangesAsync(CancellationToken.None);

            var result = await repository.GetByIdAsync(user.UserId, CancellationToken.None);

            // Assert
            Assert.Equivalent(user, result);
        }

        // Tests that the AddAsync method correctly inserts an entity into the database asynchronously.
        [Fact]
        public async Task AddAsync_Should_Add_Entity()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var user = new User(authId);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            // Act
            await repository.AddAsync(user, CancellationToken.None);
            var result = await repository.GetByIdAsync(user.UserId, CancellationToken.None);

            // Assert
            Assert.Equivalent(user, result);
        }

        // Tests that the GetByIdAsync method retrieves the correct entity from the database.
        [Fact]
        public async Task GetByIdAsync_Should_Get_Entity()
        {
            // Arrange
            var authId = Guid.NewGuid().ToString();
            var user = new User(authId);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            await repository.AddAsync(user, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetByIdAsync(user.UserId, CancellationToken.None);

            // Assert
            Assert.Equivalent(user, result);
        }

        // Tests that the GetAllAsync method retrieves all entities from the database.
        [Fact]
        public async Task GetAllAsync_Should_Get_Entities()
        {
            // Arrange
            var authId1 = Guid.NewGuid().ToString();
            var authId2 = Guid.NewGuid().ToString();
            var user1 = new User(authId1);
            var user2 = new User(authId2);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            await repository.AddAsync(user1, CancellationToken.None);
            await repository.AddAsync(user2, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Equivalent(new[] { user1, user2 }, result);
        }

        // Tests that the FindAsync method retrieves entities based on a predicate.
        [Fact]
        public async Task FindAsync_Should_Get_Entities()
        {
            // Arrange
            var authId1 = Guid.NewGuid().ToString();
            var authId2 = Guid.NewGuid().ToString();
            var user1 = new User(authId1);
            var user2 = new User(authId2);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            await repository.AddAsync(user1, CancellationToken.None);
            await repository.AddAsync(user2, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.FindAsync(u => u.AuthId == authId1, CancellationToken.None);

            // Assert
            Assert.Equivalent(new[] { user1 }, result);
        }

        // Tests that the FindFirstOrDefaultAsync method retrieves the first entity matching a predicate.
        [Fact]
        public async Task FindFirstOrDefaultAsync_Should_Get_Entity()
        {
            // Arrange
            var authId1 = Guid.NewGuid().ToString();
            var authId2 = Guid.NewGuid().ToString();
            var user1 = new User(authId1);
            var user2 = new User(authId2);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            await repository.AddAsync(user1, CancellationToken.None);
            await repository.AddAsync(user2, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            var result = await repository.FindFirstOrDefaultAsync(u => u.AuthId == authId1, CancellationToken.None);

            // Assert
            Assert.Equivalent(user1, result);
        }

        // Tests that the Update method correctly modifies an entity in the database.
        [Fact]
        public async Task Update_Should_Set_Entity()
        {
            // Arrange
            var code = "123456";
            var authId1 = Guid.NewGuid().ToString();
            var user1 = new User(authId1);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            await repository.AddAsync(user1, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            var referralCode = new ReferralCode(code, user1.UserId);
            user1.ReferralCode = referralCode;

            // Act
            repository.Update(user1);
            await repository.SaveChangesAsync(CancellationToken.None);
            var result = await repository.GetByIdAsync(user1.UserId, CancellationToken.None);

            // Assert
            Assert.Equivalent(user1.ReferralCode, result?.ReferralCode);
        }

        // Tests that the Remove method correctly deletes an entity from the database.
        [Fact]
        public async Task Remove_Should_Remove_Entity()
        {
            // Arrange
            var authId1 = Guid.NewGuid().ToString();
            var user1 = new User(authId1);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            await repository.AddAsync(user1, CancellationToken.None);
            await repository.SaveChangesAsync(CancellationToken.None);

            // Act
            repository.Remove(user1);
            await repository.SaveChangesAsync(CancellationToken.None);
            var result = await repository.GetByIdAsync(user1.UserId, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        // Tests that the Dispose method correctly disposes of the database context.
        [Fact]
        public void Dispose_Should_Teardown()
        {
            // Arrange
            var authId1 = Guid.NewGuid().ToString();
            var user1 = new User(authId1);
            var dbContext = InMemoryDbContextFactory.Create();
            var repository = new RepositoryBase<User>(dbContext);

            // Act
            repository.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => dbContext.Database.EnsureCreated());
        }
    }
}