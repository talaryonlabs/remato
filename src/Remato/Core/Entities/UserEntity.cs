using System;
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
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        
        [Computed] public string Token { get; set; }
        
        public static implicit operator RematoUser([NotNull] UserEntity entity) => new()
        {
            UserId = entity.Id,
            IsEnabled = entity.IsEnabled,
            IsAdmin = entity.IsAdmin,
            IsDeleted = entity.IsDeleted,
            Username = entity.Username,
            Mail = entity.Mail,
            Name = entity.Name,
            CreatedAt = entity.CreatedAt,
            ChangedAt = entity.ChangedAt
        };
        
        public static implicit operator UserEntity([NotNull] RematoUser user) => new()
        {
            Id = user.UserId,
            Username = user.Username,
            Mail = user.Mail,
            Name = user.Name,
            IsAdmin = user.IsAdmin,
            IsEnabled = user.IsEnabled,
            IsDeleted = user.IsDeleted,
            ChangedAt = user.ChangedAt,
            CreatedAt = user.CreatedAt
        };
    }
}