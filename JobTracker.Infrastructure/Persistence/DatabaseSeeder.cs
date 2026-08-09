using JobTracker.Application.Common.CurrentUser;
using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace JobTracker.Infrastructure.Persistence
{
    public class DatabaseSeeder
    {
        public static async Task SeedDefaultUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var deafaultUserId = SingleUserProvider.DefaultUserId;

            var userExist = await dbContext.Users
                .AnyAsync(x => x.Id == deafaultUserId);

            if(!userExist)
            {
                var user = new User
                {
                    Id = deafaultUserId,
                    Email = "owner@local.com",
                    CreatedAt = DateTimeOffset.UtcNow
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
