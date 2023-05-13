using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

        public string GetToken() {  
            return _token;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(message),
            };


            //if (update.Message is not { } message)
            //    return;
            //if (message.Text is not { } messageText)
            //    return;

            //var chatId = message.Chat.Id;

            //Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            //Message sentMessage = await botClient.SendTextMessageAsync(
            //    chatId: chatId,
            //    text: "You said:\n" + messageText,
            //    cancellationToken: cancellationToken);
        }

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

        private async Task BotOnMessageReceived(Message message)
        {
            var chatId = message.Chat.Id;

            if (chatId == 873126235)
            {
                Console.WriteLine("Perezvonish check");
            }

            Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}. Type: {message.Type}");

            await this.TelegramBotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + message.Text,
                cancellationToken: this.cts.Token);

            var chat = OpenApiSender.api.Chat.CreateConversation();
            //chat.AppendSystemMessage(""); 

            chat.AppendUserInput(message.Text);
            var answer = await chat.GetResponseFromChatbotAsync();

            await this.TelegramBotClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + answer,
                cancellationToken: this.cts.Token);
        }
    }
}