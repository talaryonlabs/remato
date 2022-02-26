using System.Runtime.Serialization;

namespace Remato.Shared
{
    [DataContract]
    public sealed class UserNotFoundError : NotFoundError
    {
        public UserNotFoundError() 
            : base("User not found.")
        {
        }
    }
}