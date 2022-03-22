namespace Remato
{
    public enum RematoCachingName
    {
        User,
        Vehicle,
        Device,
        Trainee,
        Issue,
        Job,
        Comment,
        Log
    }
    
    public static class RematoCaching
    {
        private const string CachePrefix = "REMATO";

        public static string GetKey(RematoCachingName name, string id) => $"{CachePrefix}:{name}:{id}";
    }
}