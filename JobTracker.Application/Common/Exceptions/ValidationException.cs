using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IReadOnlyDictionary<string, string[]> Errors { get;}
        public ValidationException(IReadOnlyDictionary<string, string[]> errors)
            : base("One or more validation have occurred")
        {
            Errors = errors;
        }
    }
}
