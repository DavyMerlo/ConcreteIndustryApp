using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ConcreteIndustry.BLL.Handlers
{
    public class AuthHandler
    {
        private readonly RequestDelegate next;

        public AuthHandler(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrWhiteSpace(token))
            {
                var userToken = await unitOfWork.Tokens.GetUserTokenByTokenAsync(token);

                if (userToken?.Revoked != null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token has been revoked.");
                    return;
                }
            }
            await next(context);
        }
    }
}
