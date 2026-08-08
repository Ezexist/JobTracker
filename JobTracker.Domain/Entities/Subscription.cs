using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool RemoteOnly { get; set; }
        public int? MinSalary{ get; set; }
        public string? Currency { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public ICollection<SubscriptionKeyword> Keywords { get; set; } = [];

        public ICollection<SubscriptionLocation> Locations { get; set; } = [];

        public ICollection<SubscriptionMatch> Matches { get; set; } = [];
        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
        public void Toggle()
        {
            IsActive = !IsActive;
        }
        public void Touch(DateTime utcNow)
        {
            UpdatedAt = utcNow;
        }

    }
}
