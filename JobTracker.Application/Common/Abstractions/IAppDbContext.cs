using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Abstractions
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Subscription> Subscriptions { get; }

        DbSet<SubscriptionKeyword> SubscriptionKeywords { get; }

        DbSet<SubscriptionLocation> SubscriptionLocations { get; }

        DbSet<Vacancy> Vacancies { get; }

        DbSet<SubscriptionMatch> SubscriptionMatches { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
