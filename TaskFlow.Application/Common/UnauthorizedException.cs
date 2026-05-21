using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.Common
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized access.")
        : base(message) { }
    }
}
