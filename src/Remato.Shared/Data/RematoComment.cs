using System;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoComment
    {
        [JsonProperty("id")] public string CommentId { get; set; }
        [JsonProperty("content_id")] public string ContentId { get; set; }
        [JsonProperty("content_type")] public string ContentType { get; set; }
        [JsonProperty("date")] public DateTime Date { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
    }
}