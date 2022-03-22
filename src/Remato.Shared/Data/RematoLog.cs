using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoLog
    {
        [JsonProperty("id")] public int LogId { get; set; }
        [JsonProperty("type")] public RematoLogType Type { get; set; }
        [JsonProperty("date")] public DateTime Date { get; set; }
        [JsonProperty("message")] public string Message { get; set; }
        
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("user_name")] public string UserName { get; set; }
        
        [JsonProperty("content_id")] public string ContentId { get; set; }
        [JsonProperty("content_type")] public RematoContentType ContentType { get; set; }
    }

    public enum RematoLogType
    {
        Error,
        Added,
        Changed,
        Deleted
    }
    
        
    [JsonObject]
    public class RematoLogList : RematoList<RematoLog>
    {
        [JsonProperty("logs")] 
        public override IEnumerable<RematoLog> Items { get; set; } = new List<RematoLog>();
    }
    
    [DataContract]
    public class RematoLogListArgs : RematoListArgs
    {
        [QueryMember("content_id")] public string ContentId { get; set; }
    }
}