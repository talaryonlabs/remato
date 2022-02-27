using System;
using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableLog)]
    public class LogEntity
    {
        [Key] public int Id { get; set; }
        public DateTime Date { get; set; }
        public LogLevel Level { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        
        public static implicit operator RematoLog([NotNull] LogEntity entity) => new ()
        {
            LogId = entity.Id,
            Level = entity.Level,
            Date = entity.Date,
            Category = entity.Category,
            Message = entity.Message,
            Exception = entity.Exception
        };
    }
}