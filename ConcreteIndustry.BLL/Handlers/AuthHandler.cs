using ConcreteIndustry.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using ConcreteIndustry.BLL.Services.Security;

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
            var accesToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var hashed = JwtProvider.HashToken(accesToken);

            if (!string.IsNullOrWhiteSpace(hashed))
            {
                var userToken = await unitOfWork.Tokens.GetUserTokenByTokenAsync(hashed);

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
