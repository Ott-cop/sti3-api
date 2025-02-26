using dotenv.net;

namespace sti3_api
{
    public static class GlobalConfig
    {
        public static IDictionary<string, string> Dotenv { get; private set; }

        static GlobalConfig()
        {
            Dotenv = DotEnv.Read();
        }
    }
}