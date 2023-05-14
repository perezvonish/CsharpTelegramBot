namespace CsharpTelegramBot.Config
{
    public static class BotMessages
    {
        public static string Start(string username) {
            return $"Hello, *{username}*!\n\n" +
                $"This is my pet-project console app with *OpenAI* interaction\n" +
                $"Write your message and take the answer! :)";
        }

        public static string AdditionalInfo() {
            return "Additional about developer:";
        }
    }
}
