using Microsoft.Net.Http.Headers;

namespace Remato.Shared
{
    public class RematoMediaType : MediaTypeHeaderValue
    {
        public RematoMediaType() : base("application/json") { }
    }
    
    
}