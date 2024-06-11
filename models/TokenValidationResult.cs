
namespace CustomClaimsPOC3.Models
{
    public class TokenValidationResult
    {
        public TokenClaims Claims { get; set; }
        public bool Success { get; set; }
    }
}