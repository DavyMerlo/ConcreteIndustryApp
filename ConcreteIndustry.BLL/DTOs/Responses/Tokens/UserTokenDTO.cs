namespace ConcreteIndustry.BLL.DTOs.Responses.UserTokens
{
    public class UserTokenDTO
    {
        public long Id { get; set; }
        public long UserID { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expired { get; set; }
        public DateTime? Revoked { get; set; }
    }
}
