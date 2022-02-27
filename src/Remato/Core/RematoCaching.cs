namespace Remato
{
    public static class RematoCaching
    {
        public const string CachePrefix = "REMATO";
        
        public static string GetUserKey(string userId) => $"{CachePrefix}:USER:{userId}";
        
        public static string GetVehicleKey(string vehicleId) => $"{CachePrefix}:VEHICLE:{vehicleId}";
        
        public static string GetDeviceKey(string deviceId) => $"{CachePrefix}:DEVICE:{deviceId}";
        
        public static string GetTraineeKey(string traineeId) => $"{CachePrefix}:TRAINEE:{traineeId}";
        
        public static string GetIssueKey(string issueId) => $"{CachePrefix}:ISSUE:{issueId}";
    }
}