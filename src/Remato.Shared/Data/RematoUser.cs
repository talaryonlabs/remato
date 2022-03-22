using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoUser
    {
        [JsonProperty("id")] public string UserId { get; set; }
        [JsonProperty("username")] public string Username { get; set; }
        
        [JsonProperty("password")] public string Password { get; set; }
        [JsonProperty("mail")] public string Mail { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("is_enabled")] public bool IsEnabled { get; set; }
        [JsonProperty("is_admin")] public bool IsAdmin { get; set; }
        [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
        
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("changed_at")] public DateTime ChangedAt { get; set; }
    }

    [JsonObject]
    public class RematoUserList : RematoList<RematoUser>
    {
        [JsonProperty("users")] 
        public override IEnumerable<RematoUser> Items { get; set; } = new List<RematoUser>();
    }
    
    [DataContract]
    public class RematoUserListArgs : RematoListArgs
    {
        [QueryMember("id")] public string Id { get; set; }
        [QueryMember("username")] public string Username { get; set; }
        [QueryMember("mail")] public string Mail { get; set; }
        [QueryMember("name")] public string Name { get; set; }
        [QueryMember("is_enabled")] public bool IsEnabled { get; set; }
        [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }
}