using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Remato.Shared
{
    [DataContract]
    public class BadRequestError : RematoError
    {
        public BadRequestError() : base(StatusCodes.Status403Forbidden, "Forbidden.")
        {
        }
    }
}