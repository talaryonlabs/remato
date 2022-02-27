using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoDevice
    {
        [JsonProperty("id")] public string DeviceId { get; set; }
        [JsonProperty("vehicle_id")] public string VehicleId { get; set; }
        [JsonProperty("state")] public RematoDeviceState State { get; set; }
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
        [QueryMember("state")] public RematoDeviceState State { get; set; }
        // [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }
    
    public enum RematoDeviceState
    {
        None = 0,
        IsActive = 1,
        InBackup = 2,
        InMaintenance = 3,
        IsDiscarded = 4
    }
}