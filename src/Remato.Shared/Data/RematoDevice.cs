using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoDevice
    {
        [JsonProperty("id")] public string DeviceId { get; set; }
    }
    
    [JsonObject]
    public class RematoDeviceList : RematoList<RematoDevice>
    {
        [JsonProperty("devices")] 
        public override IEnumerable<RematoDevice> Items { get; set; } = new List<RematoDevice>();
    }
    
    [DataContract]
    public class RematoDeviceListArgs : RematoListArgs
    {
        // TODO
        [QueryMember("id")] public string Id { get; set; }
        [QueryMember("name")] public string Name { get; set; }
        [QueryMember("is_active")] public bool IsActive { get; set; }
        // [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }
}