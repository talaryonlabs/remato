using System;
using Remato.Shared;

namespace Remato
{
    public interface IVehicleParams
    {
        IVehicleParams Id(string vehicleId);
        IVehicleParams Name(string name);
        IVehicleParams State(string state) => State(Enum.Parse<RematoVehicleState>(state, true));
        IVehicleParams State(RematoVehicleState state);
        IVehicleParams LicensePlate(string licensePlate);
        IVehicleParams VIN(string vin);
        IVehicleParams StartDate(DateTime startDate);
        IVehicleParams EndDate(DateTime endDate);
    }
}