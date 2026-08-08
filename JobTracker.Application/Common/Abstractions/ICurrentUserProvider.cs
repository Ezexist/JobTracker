using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Abstractions
{
    public interface ICurrentUserProvider
    {
        Guid UserId { get; }
    }
}
