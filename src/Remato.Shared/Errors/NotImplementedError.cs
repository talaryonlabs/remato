using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Remato.Shared
{
    [DataContract]
    public class NotImplementedError : RematoError
    {
        public NotImplementedError() 
            : base(StatusCodes.Status501NotImplemented, "Method not implemented.")
        {
        }
    }
}