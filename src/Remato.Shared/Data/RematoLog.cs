using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoLog
    {
        [JsonProperty("id")] public string LogId { get; set; }
    }
    
        
    [JsonObject]
    public class RematoLogList : RematoList<RematoLog>
    {
        [JsonProperty("vehicles")] 
        public override IEnumerable<RematoLog> Items { get; set; } = new List<RematoLog>();
    }
    
    [DataContract]
    public class RematoLogListArgs : RematoListArgs
    {
        // TODO
        // [QueryMember("id")] public string Id { get; set; }
        // [QueryMember("username")] public string Username { get; set; }
        // [QueryMember("is_enabled")] public bool IsEnabled { get; set; }
        // [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }
}