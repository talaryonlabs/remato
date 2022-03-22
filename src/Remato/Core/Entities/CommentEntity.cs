using System;
using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableComment)]
    public class CommentEntity
    {
        [Key] public int Id { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }
        public string Message { get; set; }
        
        public string UserId { get; set; }
        public string ContentId { get; set; }

        [Computed] public string UserName { get; set; }

        public static implicit operator RematoComment([NotNull] CommentEntity entity) => new()
        {
            CommentId = entity.Id,
            IsDeleted = entity.IsDeleted,
            Message = entity.Message,
            CreatedAt = entity.CreatedAt,
            ChangedAt = entity.ChangedAt,
            UserId = entity.UserId,
            UserName = entity.UserName,
            ContentId = entity.ContentId
        };
        
        public static implicit operator CommentEntity([NotNull] RematoComment comment) => new()
        {
            Id = comment.CommentId,
            CreatedAt = comment.CreatedAt,
            ChangedAt = comment.ChangedAt,
            IsDeleted = comment.IsDeleted,
            Message = comment.Message,
            UserId = comment.UserId,
            ContentId = comment.ContentId
        };
    }
}