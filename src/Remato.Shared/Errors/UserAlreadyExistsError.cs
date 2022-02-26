using System.Runtime.Serialization;

namespace Remato.Shared
{
    [DataContract]
    public sealed class UserAlreadyExistsError : ConflictError
    {
        [DataMember(Name = "user")] public RematoUser User;
        
        public UserAlreadyExistsError(RematoUser existingUser) 
            : base("User already exists.")
        {
            User = existingUser;
        }
    }
}