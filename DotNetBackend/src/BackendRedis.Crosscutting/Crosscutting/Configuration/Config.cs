namespace BackendRedis.Crosscutting.Configuration
{
    public class Config
    {
        public Redis Redis { get; set; }
        public StorageEndpoint StorageEndpoint { get; set; }
    }
}