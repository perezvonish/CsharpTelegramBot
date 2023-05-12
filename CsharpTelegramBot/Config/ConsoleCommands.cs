namespace CsharpTelegramBot.Config
{
    public static class ConsoleCommands
    {
        public static ConsoleCommand Help = new ConsoleCommand("/help", null, null);
        public static ConsoleCommand Q = new ConsoleCommand("/q", "Stop application.", null);
        public static ConsoleCommand Exit = new ConsoleCommand("/exit", "Stop application.", null);
        public static ConsoleCommand SendMessage = new ConsoleCommand("/smg", "Send message to user", "/smg [chatId] [message]");
    }

    public class ConsoleCommand
    {
        public string Command { get; set; }
        public string? Description { get; set; }
        public string? CommandRule { get; set; }

        public ConsoleCommand(string command, string? desc, string? rule) { 
            this.Command = command;
            this.Description = desc;
            this.CommandRule = rule;
        }
    }
}
