using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = [];
    }
}
