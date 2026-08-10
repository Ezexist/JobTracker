using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Models
{
    public sealed record SubscriptionDto(
        Guid Id,
        string Name,
        bool IsActive,
        bool RemoteOnly,
        int? MinSalary,
        string? Currency,
        List<string> Keywords,
        List<string> Locations,
        DateTimeOffset CreatedAt);
}
