using System;
using System.Diagnostics.CodeAnalysis;
using Dapper.Contrib.Extensions;
using Remato.Shared;

namespace Remato
{
    [Table(RematoConstants.DatabaseTableVehicle)]
    public class VehicleEntity
    {
        [ExplicitKey] public string Id { get; set; }
        
        public RematoVehicleState State { get; set; }
        
        public string Name { get; set; }
        public string LicensePlate { get; set; }
        public string VIN { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static implicit operator RematoVehicle([NotNull] VehicleEntity entity) => new()
        {
            VehicleId = entity.Id,
            Name = entity.Name,
            LicensePlate = entity.LicensePlate,
            VIN = entity.VIN,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        };
        
        public static implicit operator VehicleEntity([NotNull] RematoVehicle vehicle) => new()
        {
            Id = vehicle.VehicleId,
            Name = vehicle.Name,
            LicensePlate = vehicle.LicensePlate,
            VIN = vehicle.VIN,
            StartDate = vehicle.StartDate,
            EndDate = vehicle.EndDate
        };
    }
}