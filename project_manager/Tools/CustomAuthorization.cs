using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace project_manager.Tools
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorization : AuthorizeAttribute,  IAsyncAuthorizationFilter
    {
        public CustomAuthorization(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var response = new common.Models.Response<string>
            {
                Success = false,
            };
            switch (context.HttpContext.Response.StatusCode)
            {
                case 401:
                    response.Message = "Unauthorized";
                    context.Result = new JsonResult(response);
                    break;
                case 403:
                    response.Message = "Forbidden";
                    context.Result = new JsonResult(response);
                    break;
            }

            await Task.CompletedTask;
        }

    }
}
