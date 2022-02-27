using System.Runtime.Serialization;

namespace Remato.Shared
{
    [DataContract]
    public sealed class VehicleNotFoundError : NotFoundError
    {
        public VehicleNotFoundError() 
            : base("Vehicle not found.")
        {
        }
    }
}