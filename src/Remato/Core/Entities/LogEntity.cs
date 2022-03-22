using System;
using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableLog)]
    public class LogEntity
    {
        [Key] public int Id { get; set; }
        public DateTime Date { get; set; }
        public RematoLogType Type { get; set; }
        public string Message { get; set; }

        public string ContentId { get; set; }
        public RematoContentType ContentType { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        
        [Computed] public string ContentTitle { get; set; }
        
        public static implicit operator RematoLog([NotNull] LogEntity entity) => new ()
        {
            LogId = entity.Id,
            Date = entity.Date,
            Type = entity.Type,
            Message = entity.Message,
            ContentId = entity.ContentId,
            ContentType = entity.ContentType,
            UserId = entity.UserId,
            UserName = entity.UserName
        };
    }
}