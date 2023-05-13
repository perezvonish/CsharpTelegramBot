namespace CsharpTelegramBot.Config
{
    public static class EnvLoader
    {
        public static void LoadEnv()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Loading environment.");
            try
            {
                DotNetEnv.Env.TraversePath().Load();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                ExceptionMessages.SendSpecialMessage(ConsoleColor.Red, ExceptionMessages.EnvLoadException);
                throw new Exception();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Environment is loaded!");
            Console.ResetColor();
        }

        public static string? GetBotToken()
        {
            return DotNetEnv.Env.GetString("TELEGRAM_BOT_TOKEN", ExceptionMessages.TokenDoesNotExistInEnv);
        }

        public static string? GetOpenApiKey()
        {
            return DotNetEnv.Env.GetString("OPEN_API_KEY", ExceptionMessages.OpenApiKeyDoesNotExistInEnv);
        }
    }
}
