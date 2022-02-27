using System;
using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableDevice)]
    public class DeviceEntity
    {
        [ExplicitKey] public string Id { get; set; }
        
        public RematoDeviceState State { get; set; }
        
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        
        public DateTime EntryDate { get; set; }
        public DateTime EndDate { get; set; }

        public static implicit operator RematoDevice([NotNull] DeviceEntity entity) => new()
        {
            DeviceId = entity.Id,
            State = entity.State,
        };
        
        public static implicit operator DeviceEntity([NotNull] RematoDevice device) => new()
        {
            Id = device.DeviceId,
            State = device.State
        };
    }
}