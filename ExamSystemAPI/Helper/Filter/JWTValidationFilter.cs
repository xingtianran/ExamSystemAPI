using ExamSystemAPI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ExamSystemAPI.Helper.Filter
{
    public class JWTValidationFilter : IAsyncActionFilter
    {

        private readonly UserManager<User> userManager;

        public JWTValidationFilter(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ControllerActionDescriptor? descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor == null)
            {
                await next();
                return;
            }
            if (descriptor.MethodInfo.GetCustomAttributes(typeof(NotCheckJWTValiadationAttribute), true).Any())
            {
                await next();
                return;
            }
            var claimJWTVersion = context.HttpContext.User.FindFirst("JWTVersion");
            if (claimJWTVersion == null)
            {
                var result = new ObjectResult("Unauthorized");
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = result;
                return;
            }
            long claimJWTVersionValue = Convert.ToInt64(claimJWTVersion.Value);
            User? user = await userManager.FindByIdAsync(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (user == null)
            {
                var result = new ObjectResult("Unauthorized");
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = result;
                return;
            }

            if (claimJWTVersionValue < user.JWTVersion)
            {
                var result = new ObjectResult("Unauthorized");
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = result;
                return;
            }
            await next();
        }
    }
}
