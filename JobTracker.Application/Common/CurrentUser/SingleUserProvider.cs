using JobTracker.Application.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.CurrentUser
{
    public sealed class SingleUserProvider : ICurrentUserProvider
    {
        public Guid UserId => DefaultUserId;

        public static readonly Guid DefaultUserId =
            Guid.Parse("00000000-0000-0000-0000-000000000001");
    }
}
