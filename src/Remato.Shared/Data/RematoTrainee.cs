using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoTrainee
    {
        [JsonProperty("id")] public string TraineeId { get; set; }
        
        [JsonProperty("name")] public string Name { get; set; }
        
        [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("changed_at")] public DateTime ChangedAt { get; set; }
    }
        
    [JsonObject]
    public class RematoTraineeList : RematoList<RematoTrainee>
    {
        [JsonProperty("trainees")] 
        public override IEnumerable<RematoTrainee> Items { get; set; } = new List<RematoTrainee>();
    }
    
    [DataContract]
    public class RematoTraineeListArgs : RematoListArgs
    {
        // TODO
        // [QueryMember("id")] public string Id { get; set; }
        // [QueryMember("username")] public string Username { get; set; }
        // [QueryMember("is_enabled")] public bool IsEnabled { get; set; }
        // [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }
}