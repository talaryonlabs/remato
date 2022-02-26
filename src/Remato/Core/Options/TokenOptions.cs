using System;
using Remato.Shared.Security;
using Talaryon;

namespace Remato
{
    public class TokenOptions : TalaryonOptions<TokenOptions>
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public TimeSpan Expiration { get; set; }
        
        public static implicit operator RematoTokenParameters(TokenOptions options) => 
            new RematoTokenParameters(options.Issuer, options.Audience, options.Secret);
    }
}