using System;
using System.Net.Mime;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoComment
    {
        [JsonProperty("id")] public int CommentId { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
        
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("user_name")] public string UserName { get; set; }
        
        [JsonProperty("content_id")] public string ContentId { get; set; }
        [JsonProperty("content_type")] public RematoContentType ContentType { get; set; }

        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("changed_at")] public DateTime ChangedAt { get; set; }
        [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
    }
}