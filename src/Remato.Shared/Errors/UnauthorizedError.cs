using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Remato.Shared
{
    [DataContract]
    public class UnauthorizedError : RematoError
    {
        public UnauthorizedError(string message) 
            : base(StatusCodes.Status401Unauthorized, message)
        {
        }
    }
}