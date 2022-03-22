using System;
using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableIssue)]
    public class IssueEntity
    {
        [ExplicitKey] public string Id { get; set; }
        
        public bool IsDeleted { get; set; }
        public string Issuer { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }

        public static implicit operator RematoIssue([NotNull] IssueEntity entity) => new()
        {
            IssueId = entity.Id,
            Issuer = entity.Issuer,
            Message = entity.Message,
            CreatedAt = entity.CreatedAt,
            ChangedAt = entity.ChangedAt
        };
        
        public static implicit operator IssueEntity([NotNull] RematoIssue entity) => new()
        {
            Id = entity.IssueId, 
            Issuer = entity.Issuer,
            Message = entity.Message,
            CreatedAt = entity.CreatedAt,
            ChangedAt = entity.ChangedAt
        };
    }
}