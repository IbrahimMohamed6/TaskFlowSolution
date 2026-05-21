using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.Common
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors)
            : base("One or more validation failures occurred.")
        {
            Errors = errors;
        }
    }
}
