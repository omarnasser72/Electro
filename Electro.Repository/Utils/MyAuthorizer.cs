using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Electro.Repository.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyAuthorizer : Attribute, IAuthorizationFilter
    {
        private readonly string[]? roles;

        public MyAuthorizer(string role)
        {
            roles = new string[] { role };
        }

        public MyAuthorizer(string[] roles)
        {
            this.roles = roles;
        }

        public MyAuthorizer() { }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new { message = "You'ren't authenticated." })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            if (roles == null || roles?.Length == 0)
                return;

            bool authourized = false;

            foreach (string role in roles)
            {
                if (user.IsInRole(role))
                {
                    authourized = true;
                    break;
                }
            }
            if (!authourized)
            {
                context.Result = new JsonResult(new { message = $"You'ren't authorized." })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }
        }
    }
}
