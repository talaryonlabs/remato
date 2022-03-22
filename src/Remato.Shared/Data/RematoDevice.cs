using System;
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
        
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("changed_at")] public DateTime ChangedAt { get; set; }
        
        [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
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
        [QueryMember("device")] public string DeviceIdOrName { get; set; }
        [QueryMember("device_state")] public RematoDeviceState DeviceState { get; set; }
        [QueryMember("vehicle")] public string VehicleIdOrName { get; set; }
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