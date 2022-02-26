using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Remato.Shared
{
    [DataContract]
    public class ConflictError : RematoError
    {
        public ConflictError(string message) 
            : base(StatusCodes.Status409Conflict, message)
        {
        }
    }
}