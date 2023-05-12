using CsharpTelegramBot.Config;
using Telegram.Bot;

namespace CsharpTelegramBot
{
    public class Program
    {
       static async Task Main(string[] args)
        {
            await Task.Run(() =>
            {
                EnvLoader.LoadEnv();
            });


            string? _token = await Task.Run(() =>
            {
                return EnvLoader.GetBotToken();
            });

            if (_token == ExceptionMessages.TokenDoesNotExistInEnv)
            {
                ExceptionMessages.SendSpecialMessage(ConsoleColor.Red, ExceptionMessages.TokenDoesNotExistInEnv);
                throw new Exception();
            }

            TelegramBot bot = new TelegramBot(_token);
            bot.Init();

            await Task.Run(() =>
            {
                bot.StartReceiving();
            });

            var about = await bot.TelegramBotClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{about.Username}");

            for(; ;)
            {
                await ConsoleHandler.CommandHandler();
            }
        }
    }
}
