using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Remato.Shared
{
    [JsonObject]
    public class RematoVehicle
    {
        [JsonProperty("id")] public string VehicleId { get; set; }
        
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("license_plate")] public string LicensePlate { get; set; }
        [JsonProperty("vin")] public string VIN { get; set; }
        
        [JsonProperty("start_date")] public DateTime StartDate { get; set; }
        [JsonProperty("end_date")] public DateTime EndDate { get; set; }
        
        [JsonProperty("is_deleted")] public bool IsDeleted { get; set; }
        
        [JsonProperty("created_at")] public DateTime CreatedAt { get; set; }
        [JsonProperty("changed_at")] public DateTime ChangedAt { get; set; }
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
        [QueryMember("id")] public string Id { get; set; }
        [QueryMember("name")] public string Name { get; set; }
        // [QueryMember("is_enabled")] public bool IsEnabled { get; set; }
        // [QueryMember("is_admin")] public bool IsAdmin { get; set; }
    }

    public enum RematoVehicleState
    {
        None = 0,
        IsActive = 1,
        InMaintenance = 2,
        IsDiscarded = 3
    }
}