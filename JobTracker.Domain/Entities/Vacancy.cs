using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class Vacancy : BaseEntity
    {
        public string Source { get; set; } = string.Empty;
        public string ExternalId { get; set; } = string.Empty;
        public string Title {  get; set; } = string.Empty;
        public string? Company { get; set; }

        public string? Location { get; set; }
        public bool IsRemote { get; set; }
        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public string? Currency { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTimeOffset? PublishedAt { get; set; }
        public DateTimeOffset DetectedAt { get; set; }
        public ICollection<SubscriptionMatch> Matches { get; set; } = [];
    }
}
