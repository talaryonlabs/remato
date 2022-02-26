using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoVehicle
    {
        [JsonProperty("id")] public string VehicleId { get; set; }
    }
    
    [JsonObject]
    public class RematoVehicleList : RematoList<RematoVehicle>
    {
        [JsonProperty("vehicles")] 
        public override IEnumerable<RematoVehicle> Items { get; set; } = new List<RematoVehicle>();
    }
    
    [DataContract]
    public class RematoVehicleListArgs : RematoListArgs
    {
        // TODO
        // [QueryMember("id")] public string Id { get; set; }
        // [QueryMember("username")] public string Username { get; set; }
        // [QueryMember("is_enabled")] public bool IsEnabled { get; set; }
        // [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }
}