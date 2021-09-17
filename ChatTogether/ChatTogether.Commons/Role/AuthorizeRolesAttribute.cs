using Microsoft.AspNetCore.Authorization;
using System;

namespace ChatTogether.Commons.Role
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Role[] roles)
        {
            Roles = String.Join(",", roles);
        }
    }
}
