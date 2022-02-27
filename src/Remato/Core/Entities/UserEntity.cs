using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableUser)]
    public class UserEntity
    {
        [ExplicitKey] public string Id { get; set; }
        public string AuthAdapter { get; set; }
        public string AuthId { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsAdmin { get; set; }
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        [Computed] public string Token { get; set; }
        
        public static implicit operator RematoUser([NotNull] UserEntity entity) => new()
        {
            UserId = entity.Id,
            IsEnabled = entity.IsEnabled,
            IsAdmin = entity.IsAdmin,
            Username = entity.Username,
        };
        
        public static implicit operator UserEntity([NotNull] RematoUser user) => new()
        {
            Id = user.UserId,
            Username = user.Username,
            IsAdmin = user.IsAdmin,
            IsEnabled = user.IsEnabled
        };
    }
}