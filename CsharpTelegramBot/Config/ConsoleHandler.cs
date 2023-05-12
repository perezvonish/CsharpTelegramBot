namespace CsharpTelegramBot.Config
{
    public static class ConsoleHandler
    {
        public static Task CommandHandler()
        {
            string command = Console.ReadLine();

            switch (command)
            {
                case var value when value == ConsoleCommands.Help.Command:
                    ConsoleHandler.HelpCommand();
                    break;
                case var value when value == ConsoleCommands.Exit.Command ||  value == ConsoleCommands.Q.Command:
                    Environment.Exit(0);
                    break;
                case var value when value == ConsoleCommands.SendMessage.Command:
                    
                    break;
                default:
                    ExceptionMessages.SendSpecialMessage(ConsoleColor.Red, "Wrong command!");
                    ExceptionMessages.WriteLine(ConsoleColor.Yellow, "Write /help for more info.");
                    break;
            }

            return Task.CompletedTask;
        }

        private static void HelpCommand()
        {
            Console.WriteLine("Help command");
        }
    }
}
