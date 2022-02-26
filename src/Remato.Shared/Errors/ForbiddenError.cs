using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Remato.Shared
{
    [DataContract]
    public class ForbiddenError : RematoError
    {
        public ForbiddenError() 
            : base(StatusCodes.Status403Forbidden, "Forbidden.")
        {
        }
    }
}