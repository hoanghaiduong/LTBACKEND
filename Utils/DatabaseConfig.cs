namespace LTBACKEND.Utils
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; }
        public int Timeout { get; set; } = 3;

        public DatabaseConfig(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
