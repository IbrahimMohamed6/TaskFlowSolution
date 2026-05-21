using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlowDomain.DTOs.IdentityDTOs
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Token { get; set; }
    }
}
