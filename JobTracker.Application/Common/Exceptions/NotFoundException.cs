using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
            
        }

        public NotFoundException(string entityName,object key)
            : base($"Entity \"{entityName}\" ({key}) was not found")
        {
            
        }
    }
}
