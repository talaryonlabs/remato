using System.Runtime.Serialization;

namespace Remato.Shared
{
    [DataContract]
    public sealed class DeviceAlreadyExistsError : ConflictError
    {
        [DataMember(Name = "device")] public RematoDevice Device;
        
        public DeviceAlreadyExistsError(RematoDevice existingDevice) 
            : base("Device already exists.")
        {
            Device = existingDevice;
        }
    }
}