using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Remato.Shared.Security
{
    public class RematoTokenParameters : TokenValidationParameters
    {
        public RematoTokenParameters(string issuer, string audience, string secret)
        {
            if(string.IsNullOrEmpty(issuer)) throw new ArgumentNullException(nameof(issuer));
            if(string.IsNullOrEmpty(audience)) throw new ArgumentNullException(nameof(audience));
            if(string.IsNullOrEmpty(secret)) throw new ArgumentNullException(nameof(secret));
            
            ValidIssuer = issuer;
            ValidAudience = audience;
            ValidateIssuer = true;
            ValidateAudience = true;
            ValidateLifetime = true;
            ValidateIssuerSigningKey = true;
            ClockSkew = TimeSpan.Zero;
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
    }
}