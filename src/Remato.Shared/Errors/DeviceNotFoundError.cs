using System.Runtime.Serialization;

namespace Remato.Shared
{
    [DataContract]
    public sealed class DeviceNotFoundError : NotFoundError
    {
        public DeviceNotFoundError() 
            : base("Device not found.")
        {
        }
    }
}