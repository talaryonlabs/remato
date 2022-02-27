using Talaryon.Data;

namespace Remato.Services
{
    public partial class RematoService : IRematoService
    {
        private ICacheService Cache { get; }
        private IDatabaseAdapter Database { get; }
        
        public RematoService(ICacheService cache, IDatabaseAdapter database)
        {
            Cache = cache;
            Database = database;
        }

        IRematoServiceVehicle IRematoService.Vehicle(string vehicleIdOrName) => new VehicleItem(this, vehicleIdOrName);
        IRematoServiceVehicles IRematoService.Vehicles() => new VehicleList(this);
    }
}