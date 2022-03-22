using System;
using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableJob)]
    public class JobEntity
    {
        [ExplicitKey] public string Id { get; set; }
        
        public bool IsDone { get; set; }
        public bool IsDeleted { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public RematoJobType Type { get; set; }
        
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }

        public static implicit operator RematoJob([NotNull] JobEntity entity) => new()
        {
            JobId = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Type = entity.Type,
            IsDone = entity.IsDone,
            Date = entity.Date,
            CreatedAt = entity.CreatedAt,
            ChangedAt = entity.ChangedAt
        };
        
        public static implicit operator JobEntity([NotNull] RematoJob job) => new()
        {
            Id = job.JobId,
            Type = job.Type,
            Title = job.Title,
            Description = job.Description,
            IsDone = job.IsDone,
            Date = job.Date,
            CreatedAt = job.CreatedAt,
            ChangedAt = job.ChangedAt
        };
    }
}