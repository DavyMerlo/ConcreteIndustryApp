using System.Security.Claims;

namespace ConcreteIndustry.BLL.Handlers
{
    public static class ClaimsHandler
    {
        public static ClaimsIdentity GetUserIdentity(ClaimsPrincipal user)
        {
            if (user.Identity is not ClaimsIdentity claims)
            {
                throw new InvalidOperationException($"{nameof(ClaimsPrincipal)} is either null or not a {nameof(ClaimsIdentity)}");
            }
            return claims;
        }
    }
}
