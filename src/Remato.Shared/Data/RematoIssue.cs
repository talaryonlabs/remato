using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoIssue
    {
        [JsonProperty("id")] public string IssueId { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
        [JsonProperty("issuer")] public string Issuer { get; set; }

        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("changed_at")] public DateTime ChangedAt { get; set; }
        
        [JsonProperty("content_id")] public string ContentId { get; set; }
        [JsonProperty("content_type")] public string ContentType { get; set; }

        [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
    }
    
    [JsonObject]
    public class RematoIssueList : RematoList<RematoIssue>
    {
        [JsonProperty("issues")] 
        public override IEnumerable<RematoIssue> Items { get; set; } = new List<RematoIssue>();
    }
    
    [DataContract]
    public class RematoIssueListArgs : RematoListArgs
    {
        // TODO
        // [QueryMember("id")] public string Id { get; set; }
        // [QueryMember("username")] public string Username { get; set; }
        // [QueryMember("is_enabled")] public bool IsEnabled { get; set; }
        // [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }
}