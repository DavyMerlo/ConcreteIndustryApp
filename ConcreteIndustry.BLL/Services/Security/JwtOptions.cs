
namespace ConcreteIndustry.BLL.Services.Security
{
    public class JwtOptions
    {
        public string Issuer { get; init; } = "Jwt:Issuer";
        public string Audience { get; init; } = "Jwt:Audience";
        public string SecretKey { get; init; } = "Jwt:Key";
    }
}
