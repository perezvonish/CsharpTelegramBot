namespace CsharpTelegramBot
{
    public static class ExceptionMessages
    {
        public static string EnvLoadException = "Exception while loading env.";
        public static string TokenDoesNotExistInEnv = "Token does not exist in env.";

        public static void SendSpecialMessage(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("-------------");
            Console.WriteLine(message);
            Console.WriteLine("-------------");
            Console.ResetColor();
        }

        public static void WriteLine(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
