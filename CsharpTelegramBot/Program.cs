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


            string? botToken = await Task.Run(() =>
            {
                return EnvLoader.GetBotToken();
            });

            if (botToken == ExceptionMessages.TokenDoesNotExistInEnv)
            {
                ExceptionMessages.SendSpecialMessage(ConsoleColor.Red, ExceptionMessages.TokenDoesNotExistInEnv);
                throw new Exception();
            }

            string? openApiKey = await Task.Run(() =>
            {
                return EnvLoader.GetOpenApiKey();
            });

            if (openApiKey == ExceptionMessages.OpenApiKeyDoesNotExistInEnv)
            {
                ExceptionMessages.SendSpecialMessage(ConsoleColor.Red, ExceptionMessages.OpenApiKeyDoesNotExistInEnv);
                throw new Exception();
            }

            TelegramBot bot = new TelegramBot(botToken, openApiKey);

            await Task.Run(() =>
            {
                bot.Init();
            });

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
