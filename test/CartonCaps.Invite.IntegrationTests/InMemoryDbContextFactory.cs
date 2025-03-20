using CartonCaps.Invite.Data;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Invite.IntegrationTests
{
    public static class InMemoryDbContextFactory
    {
        public static InviteContext Create()
        {
            var options = new DbContextOptionsBuilder<InviteContext>()
                .UseSqlite("DataSource=:memory:")
                .UseLazyLoadingProxies()
                .Options;

            var dbContext = new InviteContext(options);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}
