using JobTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class SubscriptionMatch : BaseEntity
    {

        public DateTimeOffset MatchedAt { get; set; }
        public MatchStatus Status { get; set; } = MatchStatus.New;

        public Guid SubscriptionId { get; set; }
        public Guid VacancyId { get; set; }
        public Subscription? Subscription { get; set; }
        public Vacancy? Vacancy { get; set; }
        public void MarkViewed()
        {
            Status = MatchStatus.Viewed;
        }

        public void MarkSaved()
        {
            Status = MatchStatus.Saved;
        }

        public void Dismiss()
        {
            Status = MatchStatus.Dismissed;
        }
    }
}
