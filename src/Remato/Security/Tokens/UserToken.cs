using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Remato.Shared.Security;

namespace Remato.Security.Tokens
{
    public class UserToken
    {
        [TokenClaim(Name = RematoConstants.UniqueTokenId)] 
        [TokenClaim(Name = JwtRegisteredClaimNames.Sub)]
        public string UserId { get; set; }
        
        [TokenClaim(Name = ClaimTypes.Role)] 
        public string Role { get; set; }
    }
}