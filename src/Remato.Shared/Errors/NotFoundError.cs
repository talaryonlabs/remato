using Microsoft.AspNetCore.Http;

namespace Remato.Shared
{
    public class NotFoundError : RematoError
    {
        public NotFoundError(string message) 
            : base(StatusCodes.Status404NotFound, message)
        {
            
        }
    }
}