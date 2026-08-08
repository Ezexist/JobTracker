using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class SubscriptionKeyword : BaseEntity
    {
        public Guid SubscriptionId { get; set; }

        public string Value { get; set; } = string.Empty;
    }
}
