using System.Runtime.Serialization;

namespace Remato.Shared
{
    [DataContract]
    public sealed class VehicleAlreadyExistsError : ConflictError
    {
        [DataMember(Name = "vehicle")] public RematoVehicle Vehicle;
        
        public VehicleAlreadyExistsError(RematoVehicle existingVehicle) 
            : base("Vehicle already exists.")
        {
            Vehicle = existingVehicle;
        }
    }
}