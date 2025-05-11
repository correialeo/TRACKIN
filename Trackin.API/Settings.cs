namespace Trackin.API
{
    public static class Settings
    {
        public static IConfiguration _configuration { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetEnvVars(string env)
        {
            return _configuration.GetValue<string>(env);
        }

        public static string GetConnectionString()
        {
            string databaseSource = Environment.GetEnvironmentVariable("DATABASE__SOURCE");
            string databaseUser = Environment.GetEnvironmentVariable("DATABASE__USER");
            string databasePass = Environment.GetEnvironmentVariable("DATABASE__PASSWORD");

            string connectString = _configuration.GetConnectionString("Oracle");
            connectString = string.Format(connectString, databaseSource, databaseUser, databasePass);

            return connectString;
        }
    }
}
