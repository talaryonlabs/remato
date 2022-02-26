using Remato.Params;
using Talaryon;

namespace Remato
{
    public interface IRematoService
    {
        IRematoServiceUser User(string userIdOrName);
        IRematoServiceUsers Users();

        IRematoServiceDevice Device(string deviceId);
        IRematoServiceDevices Devices();

        object Trainee(string traineeId);
        object Trainees();

        IRematoServiceVehicle Vehicle(string vehicleId);
        IRematoServiceVehicles Vehicles();

        object Issue(string issueId);
        object Issues();
    }
    
    public interface IRematoServiceEntity<TEntity, out TParams> : 
        ITalaryonRunner<TEntity>,
        ITalaryonExistable,
        ITalaryonCreatable<TEntity, TParams>,
        ITalaryonUpdatable<TEntity, TParams>,
        ITalaryonDeletable<TEntity>
    {
        
    }

    public interface IRematoServiceEntities<TEntity, out TParams> :
        ITalaryonEnumerable<TEntity, TParams>,
        ITalaryonCountable
    {
        
    }
    
    public interface IRematoServiceUser : IRematoServiceEntity<UserEntity, IUserParams>
    {
        
    }
    
    public interface IRematoServiceUsers : IRematoServiceEntities<UserEntity, IUserParams>
    {
        
    }

    public interface IRematoServiceVehicle : IRematoServiceEntity<VehicleEntity, IVehicleParams>
    {
        
    }
    
    public interface IRematoServiceVehicles : IRematoServiceEntities<VehicleEntity, IVehicleParams>
    {
        
    }
    
    public interface IRematoServiceDevice : IRematoServiceEntity<DeviceEntity, IDeviceParams>
    {
        
    }
    
    public interface IRematoServiceDevices : IRematoServiceEntities<DeviceEntity, IDeviceParams>
    {
        
    }

}