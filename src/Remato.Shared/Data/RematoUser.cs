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
        [JsonProperty("full_name")] public string FullName { get; set; }
        [JsonProperty("password")] public string Password { get; set; }
        [JsonProperty("is_admin")] public bool IsAdmin { get; set; }
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
        [QueryMember("is_enabled")] public bool IsEnabled { get; set; }
        [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }
}