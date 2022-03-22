using Microsoft.AspNetCore.Http;
using Talaryon.Data;
using Talaryon.Services;

namespace Remato.Services
{
    public partial class RematoService : IRematoService
    {
        private ICacheService Cache { get; }
        private IDatabaseAdapter Database { get; }
        private IAuthenticationAdapter Authenticator { get; }
        public ITokenService TokenService { get; }      
        
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public RematoService(ICacheService cache, IDatabaseAdapter database, IAuthenticationAdapter authenticator, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            Cache = cache;
            Database = database;
            Authenticator = authenticator;
            TokenService = tokenService;
            
            _httpContextAccessor = httpContextAccessor;
        }


        IRematoServiceAuthorization IRematoService.Authorization() => new Authentication(this, _httpContextAccessor);

        IRematoUserService IRematoService.User(string userIdOrName) => new UserItem(this, userIdOrName);
        IRematoUsersService IRematoService.Users() => new UserList(this);
        IRematoServiceDevice IRematoService.Device(string deviceId)
        {
            throw new System.NotImplementedException();
        }

        IRematoServiceDevices IRematoService.Devices()
        {
            throw new System.NotImplementedException();
        }

        IRematoServiceTrainee IRematoService.Trainee(string traineeId)
        {
            throw new System.NotImplementedException();
        }

        IRematoServiceTrainees IRematoService.Trainees()
        {
            throw new System.NotImplementedException();
        }

        IRematoServiceVehicle IRematoService.Vehicle(string vehicleIdOrName) => new VehicleItem(this, vehicleIdOrName);
        IRematoServiceVehicles IRematoService.Vehicles() => new VehicleList(this);
        IRematoServiceIssue IRematoService.Issue(string issueId)
        {
            throw new System.NotImplementedException();
        }

        IRematoServiceIssues IRematoService.Issues()
        {
            throw new System.NotImplementedException();
        }
    }
}