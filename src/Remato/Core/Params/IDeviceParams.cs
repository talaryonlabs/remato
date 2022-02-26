namespace Remato.Params
{
    public interface IDeviceParams
    {
        IDeviceParams Id(string userId);
        IDeviceParams IsActive(bool isActive);
        IDeviceParams Name(string name);
        IDeviceParams Manufacturer(string manufacturer);
    }
}