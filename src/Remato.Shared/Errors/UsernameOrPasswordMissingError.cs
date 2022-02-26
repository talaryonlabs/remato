using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Remato.Shared
{
    [DataContract]
    public class UsernameOrPasswordMissingError : RematoError
    {
        public UsernameOrPasswordMissingError() : base(StatusCodes.Status422UnprocessableEntity, "Username or Password missing or empty!")
        {
        }
    }
}