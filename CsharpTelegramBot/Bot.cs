using CsharpTelegramBot.Config;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CsharpTelegramBot
{
    public class TelegramBot
    {
        private string _token { get; } = String.Empty;
        private string _openApiKey { get; } = String.Empty;
        
        public TelegramBotClient TelegramBotClient { get; set; }
        public ReceiverOptions receiverOptions = new ReceiverOptions(){
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        public CancellationTokenSource cts = new CancellationTokenSource();

        public TelegramBot(string botToken, string openApiKey)
        {
            this._token = botToken;
            this._openApiKey = openApiKey;
        }

        public Telegram.Bot.TelegramBotClient Init()
        {
            TelegramBotClient = new TelegramBotClient(_token);
            return TelegramBotClient;
        }

        public void StartReceiving()
        {
            this.TelegramBotClient.StartReceiving(
                updateHandler: this.HandleUpdateAsync,
                pollingErrorHandler: this.HandlePollingErrorAsync,
                receiverOptions: this.receiverOptions,
                cancellationToken: this.cts.Token
            );
        }

        // Error tracking
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            ExceptionMessages.SendSpecialMessage(ConsoleColor.Red, "ErrorMessage");
            return Task.CompletedTask;
        }

        // Message tracking
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(message),
                //{ CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery),
                //{ EditedMessage: { } message } => await BotOnMessageReceived(message),
            };
        }

        private async Task BotOnMessageReceived(Message message)
        {
            var chatId = message.Chat.Id;
            bool adminFlag = false;


            if (chatId == 873126235)
            {
                adminFlag = true;
                ExceptionMessages.SendSpecialMessage(ConsoleColor.Green, $"Admin flag is TRUE for user: {message.From.FirstName} {message.From.LastName}");
            } else
            {
                ExceptionMessages.SendSpecialMessage(ConsoleColor.Yellow, $"Admin flag is FALSE for user: {message.From.FirstName} {message.From.LastName}");
            }

            Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}. Type: {message.Type}");

            switch (message.Text)
            {
                case "/start":
                    await this.StartCommand(message, message.Chat.Id, adminFlag);
                    break;
                case "/info":
                    await this.InfoCommand(message, message.Chat.Id);
                    break;
                default:
                    await this.Usage(message);
                    break;
            }
        }

        private async Task StartCommand(Message message, long chatId, bool adminFlag)
        {
            string usage = BotMessages.Start(message.From.Username);

            await this.TelegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                parseMode: ParseMode.Markdown,
                cancellationToken: this.cts.Token);

            await this.TelegramBotClient.DeleteMessageAsync(message.Chat.Id, message.MessageId, this.cts.Token);
        }

        private async Task InfoCommand(Message message, long chatId)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithWebApp(text: "🖥️ Github", webAppInfo: new WebAppInfo(){Url = "https://github.com/perezvonish"}),
                },
                new []
                {
                    InlineKeyboardButton.WithWebApp(text: "📍 LinkedIn", webAppInfo: new WebAppInfo(){Url = "https://www.linkedin.com/in/vladimir-korobenko-b8a89a253/"}),
                },
                new []
                {
                    InlineKeyboardButton.WithWebApp(text: "🪪 Telegram", webAppInfo: new WebAppInfo(){Url = "https://t.me/perezvonishh"}),
                },
                new []
                {
                    InlineKeyboardButton.WithWebApp(text: "📋 CV", webAppInfo: new WebAppInfo(){Url = "https://docs.google.com/document/d/14b_prdICttEUSnIhtD1YBJggqij9D2UmtanN20oI0Ts/edit?usp=sharing"}),
                }
            });

            await this.TelegramBotClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: BotMessages.AdditionalInfo(),
                parseMode: ParseMode.Markdown,
                replyMarkup: inlineKeyboard,
                cancellationToken: this.cts.Token);
        }

        private async Task<Message> Usage(Message message)
        {
            var chat = OpenApiSender.api.Chat.CreateConversation();

            chat.AppendUserInput(message.Text);
            var answer = await chat.GetResponseFromChatbotAsync();

            return await this.TelegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: answer,
            cancellationToken: this.cts.Token);
        }
    }
}