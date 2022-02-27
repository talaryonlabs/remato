using Remato.Shared;

namespace Remato
{
    public interface IDeviceParams
    {
        IDeviceParams Id(string userId);
        IDeviceParams State(RematoDeviceState state);
        IDeviceParams Name(string name);
        IDeviceParams Manufacturer(string manufacturer);
    }
}