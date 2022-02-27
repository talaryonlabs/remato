using Talaryon;

namespace Remato
{
    public interface IRematoService
    {
        IRematoUserService User(string userIdOrName);
        IRematoUsersService Users();

        IRematoServiceDevice Device(string deviceId);
        IRematoServiceDevices Devices();
        IRematoServiceTrainee Trainee(string traineeId);
        IRematoServiceTrainees Trainees();

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
    
    public interface IRematoServiceTrainee : IRematoServiceEntity<TraineeEntity, ITraineeParams>
    {
        
    }
    
    public interface IRematoServiceTrainees : IRematoServiceEntities<TraineeEntity, ITraineeParams>
    {
        
    }
}