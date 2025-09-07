namespace Trackin.Api
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
            string databaseName = Environment.GetEnvironmentVariable("DATABASE__NAME");
            string databaseUser = Environment.GetEnvironmentVariable("DATABASE__USER");
            string databasePass = Environment.GetEnvironmentVariable("DATABASE__PASSWORD");

            string connectString = _configuration.GetConnectionString("SqlServer");
            connectString = string.Format(connectString, databaseSource, databaseName, databaseUser, databasePass);

            return connectString;
        }
    }
}
